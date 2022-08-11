using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection.Emit;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.Video;

public class PoseEstimate : MonoBehaviour
{
    public Vector2Int webcamDims = new Vector2Int(1280, 720);

    public int webcamFps = 60;

    public Transform videoScreen;

    public ComputeShader posenetShader;

    public bool useGPU = true;

    public Vector2Int imageDims = new Vector2Int(256, 256);

    public NNModel _model;

    public WorkerFactory.Type _workerType = WorkerFactory.Type.Auto;

    [Range(0, 1.0f)]
    public float scoreThreshold = 0.25f;

    public int nmsRadius = 100;

    public float pointScale = 10f;

    public float lineWidth = 5f;

    [Range(0, 100)]
    public int minConfidence = 70;

    private WebCamTexture _webCam;

    private Vector2Int videoDims;

    private RenderTexture videoTexture;

    private Vector2Int targetDims;

    private float aspectRatioScale;

    private RenderTexture rTex;

    private Tensor input;

    private struct Engine
    {
        public WorkerFactory.Type workerType;

        public IWorker worker;

        public Engine(WorkerFactory.Type workerType, Model model)
        {
            this.workerType = workerType;
            worker = WorkerFactory.CreateWorker(workerType, model);
        }
    }

    private Engine engine;

    private string heatmapLayer;

    private string offsetsLayer;

    private string predictionLayer = "predictLayer";

    private Utils.Keypoint[][] poses;

    private PoseSkeleton[] skeletons;

    private void InitVideoScreen(int width, int height, bool isFlipped)
    {
        videoScreen.GetComponent<VideoPlayer>().renderMode =
            VideoRenderMode.RenderTexture;

        videoScreen.GetComponent<VideoPlayer>().targetTexture = videoTexture;

        if (isFlipped)
        {
            videoScreen.rotation = Quaternion.Euler(0, 180, 0);

            videoScreen.localScale =
                new Vector3(videoScreen.localScale.x,
                    videoScreen.localScale.y,
                    -1f);
        }

        videoScreen.gameObject.GetComponent<MeshRenderer>().material.shader =
            Shader.Find("Unlit/Texture");
        videoScreen
            .gameObject
            .GetComponent<MeshRenderer>()
            .material
            .SetTexture("_MainTex", videoTexture);

        videoScreen.localScale =
            new Vector3(width, height, videoScreen.localScale.z);

        videoScreen.position = new Vector3(width / 2, height / 2, 1);
    }

    private void InitCamera()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");

        mainCamera.transform.position =
            new Vector3(webcamDims.x / 2, webcamDims.y / 2, -10f);

        mainCamera.GetComponent<Camera>().orthographic = true;

        mainCamera.GetComponent<Camera>().orthographicSize = webcamDims.y / 2;
    }

    private void InitEstimation()
    {
        Model runtimeModel;

        runtimeModel = ModelLoader.Load(_model);

        heatmapLayer = runtimeModel.outputs[0];
        offsetsLayer = runtimeModel.outputs[1];

        ModelBuilder modelBuilder = new ModelBuilder(runtimeModel);

        modelBuilder.Sigmoid (predictionLayer, heatmapLayer);

        engine = new Engine(_workerType, modelBuilder.model);
    }

    private void InitSkeletons()
    {
        skeletons = new PoseSkeleton[1];

        skeletons[0] = new PoseSkeleton(pointScale, lineWidth);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.graphicsDeviceName == null) useGPU = false;
        Application.targetFrameRate = webcamFps;

        _webCam = new WebCamTexture(webcamDims.x, webcamDims.y, webcamFps);

        _webCam.Play();

        videoDims.y = _webCam.height;

        videoDims.x = _webCam.width;

        videoTexture =
            RenderTexture
                .GetTemporary(videoDims.x,
                videoDims.y,
                24,
                RenderTextureFormat.ARGBHalf);

        InitVideoScreen(videoDims.x, videoDims.y, true);
        InitCamera();

        aspectRatioScale = (float) videoTexture.width / videoTexture.height;
        targetDims.x = (int)(imageDims.y * aspectRatioScale);
        imageDims.x = targetDims.x;

        rTex =
            RenderTexture
                .GetTemporary(imageDims.x,
                imageDims.y,
                24,
                RenderTextureFormat.ARGBHalf);

        InitEstimation();
        InitSkeletons();
    }

    private void ComputeImage(RenderTexture image)
    {
        int numthread = 8;
        int kernel = posenetShader.FindKernel("PreprocessMobileNet");

        RenderTexture result =
            RenderTexture
                .GetTemporary(image.width,
                image.height,
                24,
                RenderTextureFormat.ARGBHalf);

        result.enableRandomWrite = true;

        result.Create();

        posenetShader.SetTexture(kernel, "Result", result);

        posenetShader.SetTexture(kernel, "InpuImage", image);

        posenetShader
            .Dispatch(kernel,
            result.width / numthread,
            result.height / numthread,
            1);

        Graphics.Blit (result, image);

        RenderTexture.ReleaseTemporary (result);
    }

    private void ProcessImage(RenderTexture image)
    {
        if (useGPU)
        {
            ComputeImage (image);
            input = new Tensor(image, channels: 3);
        }
        else
        {
            input = new Tensor(image, channels: 3);

            float[] tensor_array = input.data.Download(input.shape);

            Utils.PreprocessMobileNet (tensor_array);

            input =
                new Tensor(input.shape.depth,
                    input.shape.height,
                    input.shape.width,
                    input.shape.channels,
                    tensor_array);
        }
    }

    private void ProcessOutput(IWorker engine)
    {
        Tensor heatmap = engine.PeekOutput(predictionLayer);
        Tensor offset = engine.PeekOutput(offsetsLayer);

        int stride = (imageDims.y - 1) / (heatmap.shape.height - 1);
        stride -= (stride % 8);

        poses = new Utils.Keypoint[1][];

        poses[0] = Utils.DecodeSinglePose(heatmap, offset, stride);

        heatmap.Dispose();
        offset.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        Graphics.Blit (_webCam, videoTexture);

        imageDims.x = Mathf.Max(imageDims.x, 64);
        imageDims.y = Mathf.Max(imageDims.y, 64);

        if (imageDims.x != targetDims.x)
        {
            aspectRatioScale = (float) videoTexture.height / videoTexture.width;
            targetDims.y = (int)(imageDims.x * aspectRatioScale);
            imageDims.y = targetDims.y;
            targetDims.x = imageDims.x;
        }
        if (imageDims.y != targetDims.y)
        {
            aspectRatioScale = (float) videoTexture.width / videoTexture.height;
            targetDims.x = (int)(imageDims.y * aspectRatioScale);
            imageDims.x = targetDims.x;
            targetDims.y = imageDims.y;
        }

        if (imageDims.x != rTex.width || imageDims.y != rTex.height)
        {
            RenderTexture.ReleaseTemporary (rTex);

            rTex =
                RenderTexture
                    .GetTemporary(imageDims.x, imageDims.y, 24, rTex.format);
        }

        Graphics.Blit (videoTexture, rTex);

        ProcessImage (rTex);
        if (engine.workerType != _workerType)
        {
            engine.worker.Dispose();
            InitEstimation();
        }
        engine.worker.Execute (input);
        input.Dispose();
        ProcessOutput(engine.worker);

        if (skeletons.Length != 1)
        {
            foreach (PoseSkeleton skeleton in skeletons)
            {
                skeleton.Cleanup();
            }
            InitSkeletons();
        }

        int minDimension = Mathf.Min(videoTexture.width, videoTexture.height);

        // The value used to scale the key point locations up to the source resolution
        float scale =
            (float) minDimension / Mathf.Min(imageDims.x, imageDims.y);

        // Update the pose skeletons
        for (int i = 0; i < skeletons.Length; i++)
        {
            UnityEngine.Debug.Log(poses.Length);
            if (i <= poses.Length - 1)
            {
                skeletons[i].ToggleSkeleton(true);

                // Update the positions for the key point GameObjects
                skeletons[i]
                    .UpdateKeyPointPositions(poses[i],
                    scale,
                    videoTexture,
                    true,
                    minConfidence);
                skeletons[i].UpdateLines();
            }
            else
            {
                skeletons[i].ToggleSkeleton(false);
            }
        }
    }

    private void OnDisable()
    {
        // Release the resources allocated for the inference engine
        engine.worker.Dispose();
    }
}
