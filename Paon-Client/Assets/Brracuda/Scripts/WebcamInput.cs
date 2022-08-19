using UnityEngine;

public class WebCamInput : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    string device;

    [SerializeField]
    Vector2 webcamResolution = new Vector2(1920, 1080);

    WebCamTexture _webcam;

    RenderTexture input;

    public Texture inputImageTexture
    {
        get
        {
            return input;
        }
    }

    void Start()
    {
        _webcam =
            new WebCamTexture(device,
                (int) webcamResolution.x,
                (int) webcamResolution.y);
        _webcam.Play();

        input =
            new RenderTexture((int) webcamResolution.x,
                (int) webcamResolution.y,
                0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_webcam.didUpdateThisFrame) return;

        var aspect1 = (float) _webcam.width / _webcam.height;
        var aspect2 = (float) input.width / input.height;
        var gap = aspect2 / aspect1;

        var mirrored = _webcam.videoVerticallyMirrored;
        var scale = new Vector2(gap, mirrored ? -1 : 1);
        var offset = new Vector2((1 - gap) / 2, mirrored ? 1 : 0);

        Graphics.Blit (_webcam, input, scale, offset);
    }

    void OnDestroy()
    {
        if (_webcam != null) Destroy(_webcam);
        //if (input != null) Destroy(_input);
    }
}
