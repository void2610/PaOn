using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Estimate;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.Video;

public class PoseNet : MonoBehaviour
{
    public RenderTexture _render;

    public ComputeShader _compute;

    public int webcamHeight = 720;

    public int webcamWidth = 1080;

    public int webcamFps = 60;

    public int imageHeight = 360;

    public int imageWidth = 360;

    public bool displayInput = false;

    public GameObject inputScreen;

    public RenderTexture inputTexture;

    // private RenderTexture _inputTexture = null;
    public NNModel modelAsset;

    public WorkerFactory.Type workerType = WorkerFactory.Type.Auto;

    [Range(0, 100)]
    public int minConfidence = 70;

    public GameObject[] KeyPoints;

    private Model _model;

    private IWorker _worker;

    // private string heatmapLayer = "MobilenetV1/heatmap_2/BiasAdd";
    private string heatmapLayer = "float_heatmaps";

    // private string offsetLayer = "MobilenetV1/offset_2/BiasAdd";
    private string offsetLayer = "float_short_offsets";

    private string predictLayer = "MobilenetV1/predict";

    private const int numKeypoints = 17;

    float[][] KeyPointLocations = new float[numKeypoints][];

    private WebCamTexture webcam;

    private int videoHeight;

    private int videoWidth;

    // Start is called before the first frame update
    void Start()
    {
        GameObject videoPlayer = GameObject.Find("Video Player");

        Transform videoScreen = GameObject.Find("VideoScreen").transform;
        webcam = new WebCamTexture(webcamWidth, webcamHeight, webcamFps);

        videoScreen.rotation = Quaternion.Euler(0, 180, 0);

        videoScreen.localScale =
            new Vector3(videoScreen.localScale.x,
                videoScreen.localScale.y,
                -1f);

        webcam.Play();

        GameObject.Find("Video Player").SetActive(false);

        videoHeight = (int) webcam.height;
        videoWidth = (int) webcam.width;

        _render.Release();

        _render =
            new RenderTexture(videoWidth,
                videoHeight,
                24,
                RenderTextureFormat.ARGB32);

        videoPlayer.GetComponent<VideoPlayer>().targetTexture = _render;

        videoScreen
            .gameObject
            .GetComponent<MeshRenderer>()
            .material
            .SetTexture("_MainTex", _render);

        videoScreen.localScale =
            new Vector3(videoWidth, videoHeight, videoScreen.localScale.z);

        videoScreen.position = new Vector3(videoWidth / 2, videoHeight / 2, 1);

        GameObject mainCamera = GameObject.Find("Main Camera");

        mainCamera.transform.position =
            new Vector3(videoWidth / 2, videoHeight / 2, -(videoWidth / 2));

        mainCamera.GetComponent<Camera>().orthographicSize = videoHeight / 2;

        _model = ModelLoader.Load(modelAsset);

        var modelBuilder = new ModelBuilder(_model);
        modelBuilder.Sigmoid (predictLayer, heatmapLayer);

        _worker = WorkerFactory.CreateWorker(workerType, modelBuilder.model);
    }

    private void OnDisable()
    {
        _worker.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        Graphics.Blit (webcam, _render);

        Texture2D processedTexture = PreprocessImage();
        Texture2D scaledTexture = ScaleInputImage(processedTexture);

        // Graphics.Blit (processedTexture, _inputTexture);
        Destroy (scaledTexture);

        Tensor input = new Tensor(processedTexture, channels: 3);
        _worker.Execute (input);

        ProcessOutput(_worker.PeekOutput(predictLayer),
        _worker.PeekOutput(offsetLayer));

        UpdateKeyPointPosition();

        input.Dispose();

        Destroy (processedTexture);
    }

    private Texture2D PreprocessImage()
    {
        Texture2D image =
            new Texture2D(_render.width,
                _render.height,
                TextureFormat.RGBA32,
                false);
        Graphics.CopyTexture (_render, image);

        // Destroy (image);
        return image;
    }

    private Texture2D ScaleInputImage(Texture2D input)
    {
        int numthreads = 8;
        int kernelHandle = _compute.FindKernel("PreprocessMobileNet");
        RenderTexture rTex =
            new RenderTexture(input.width,
                input.height,
                24,
                RenderTextureFormat.ARGBHalf);
        rTex.enableRandomWrite = true;
        rTex.Create();
        _compute.SetTexture(kernelHandle, "Result", rTex);
        _compute.SetTexture(kernelHandle, "InputImage", input);
        _compute
            .Dispatch(kernelHandle,
            input.height / numthreads,
            input.width / numthreads,
            1);
        RenderTexture.active = rTex;
        Texture2D nTex =
            new Texture2D(rTex.width,
                rTex.height,
                TextureFormat.RGBAHalf,
                false);
        Graphics.CopyTexture (rTex, nTex);
        RenderTexture.active = null;
        Destroy (rTex);
        return nTex;
    }

    private void ProcessOutput(Tensor heatmap, Tensor offset)
    {
        // UnityEngine.Debug.Log(heatmap.shape.height);
        float stride = (imageHeight - 1) / (heatmap.shape.height - 1);
        stride -= (stride % 8);

        int minDimension = Mathf.Min(_render.width, _render.height);

        int maxDimension = Mathf.Max(_render.width, _render.height);

        float scale =
            (float) minDimension / (float) Mathf.Min(imageWidth, imageHeight);

        float unsqueezScale = (float) maxDimension / (float) minDimension;

        for (int i = 0; i < numKeypoints; i++)
        {
            var locationInfo = LocateKeyPointIndex(heatmap, offset, i);

            var coords = locationInfo.Item1;

            var offset_vector = locationInfo.Item2;

            var confidence = locationInfo.Item3;

            float xPos = (coords[0] * stride + offset_vector[0]) * scale;

            xPos = _render.width - xPos;

            float yPos =
                (imageHeight - (coords[1] * scale + offset_vector[1])) * scale;

            if (_render.width > _render.height)
            {
                xPos *= unsqueezScale;
            }
            else
            {
                yPos *= unsqueezScale;
            }

            xPos = _render.width - xPos;

            KeyPointLocations[i] = new float[] { xPos, yPos, confidence };
        }
    }

    private (float[], float[], float)
    LocateKeyPointIndex(Tensor heatmap, Tensor offset, int keipointIndex)
    {
        float maxConfidence = 0.0f;

        float[] coords = new float[2];

        float[] offset_vector = new float[2];

        for (int y = 0; y < heatmap.shape[1]; y++)
        {
            for (int x = 0; x < heatmap.shape[2]; x++)
            {
                if (heatmap[0, y, x, keipointIndex] > maxConfidence)
                {
                    maxConfidence = heatmap[0, y, x, keipointIndex];
                    coords = new float[] { x, y };

                    offset_vector =
                        new float[] {
                            offset[0, y, x, keipointIndex + numKeypoints],
                            offset[0, y, x, keipointIndex]
                        };
                }
            }
        }
        return (coords, offset_vector, maxConfidence);
    }

    private void UpdateKeyPointPosition()
    {
        for (int i = 0; i < numKeypoints; i++)
        {
            UnityEngine.Debug.Log(KeyPointLocations[i]);
            if (KeyPointLocations[i][2] >= minConfidence / 100f)
            {
                KeyPoints[i].SetActive(true);
            }
            else
            {
                KeyPoints[i].SetActive(false);
            }
            Vector3 newPos =
                new Vector3(KeyPointLocations[i][0],
                    KeyPointLocations[i][1],
                    -1f);

            KeyPoints[i].transform.position = newPos;
        }
    }
}
