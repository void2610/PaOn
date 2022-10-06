using UnityEngine;

public class WebCamInput : MonoBehaviour
{
	private int index = 0;

	public WebCamDevice[] device;

	[SerializeField]
	private string webCamName;

	[SerializeField]
	Vector2 webCamResolution = new Vector2(1920, 1080);

	// Provide input image Texture.
	public Texture inputImageTexture
	{
		get
		{
			return inputRT;
		}
	}

	WebCamTexture webCamTexture;

	RenderTexture inputRT;

	void Start()
	{
		webCamTexture = new WebCamTexture(webCamName, (int)webCamResolution.x, (int)webCamResolution.y, 30);
		webCamTexture.Play();

		inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0);
		// DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		if (!webCamTexture.didUpdateThisFrame) return;
		if (Input.GetKey("space")) { SetCamera(index); }
		if (!webCamTexture.isPlaying) webCamTexture.Play();

		var aspect1 = (float)webCamTexture.width / webCamTexture.height;
		var aspect2 = (float)inputRT.width / inputRT.height;
		var aspectGap = aspect2 / aspect1;

		var vMirrored = webCamTexture.videoVerticallyMirrored;
		var scale = new Vector2(aspectGap, vMirrored ? -1 : 1);
		var offset = new Vector2((1 - aspectGap) / 2, vMirrored ? 1 : 0);

		Graphics.Blit(webCamTexture, inputRT, scale, offset);
	}

	// void OnEnable()
	// {
	// 	SetCamera(0);
	// }

	void SetCamera(int idx)
	{
		int length = WebCamTexture.devices.Length;
		if (length == 0) return;
		try
		{
			webCamTexture.Stop();
		}
		catch { }
		idx++;
		if (idx == length) idx = 0;
		webCamTexture = new WebCamTexture(WebCamTexture.devices[idx % length].name, (int)webCamResolution.x, (int)webCamResolution.y);
		webCamTexture.Play();

		Debug.Log(idx + ": " + WebCamTexture.devices[idx % length].name);

		inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0);
	}

	void OnDestroy()
	{
		if (webCamTexture != null) Destroy(webCamTexture);
		if (inputRT != null) Destroy(inputRT);
	}

	public WebCamTexture GetWebcam() => webCamTexture;
}
