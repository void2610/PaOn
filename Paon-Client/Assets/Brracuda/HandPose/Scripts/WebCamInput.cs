using UnityEngine;

public class WebCamInput : MonoBehaviour
{
	private int index = 0;

	public WebCamDevice[] device;

	[SerializeField]
	private string webCamName;

	[SerializeField]
	Vector2 webCamResolution = new Vector2(640, 1080);

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
		webCamTexture = new WebCamTexture(webCamName, (int)webCamResolution.x, (int)webCamResolution.y, 60);
		webCamTexture.Play();

		inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0);
	}

	void Update()
	{
		if (!webCamTexture.isPlaying) webCamTexture.Play();
		if (!webCamTexture.didUpdateThisFrame) return;
		if (Input.GetKey("space")) { SetCamera(); }

		var aspect1 = (float)webCamTexture.width / webCamTexture.height;
		var aspect2 = (float)inputRT.width / inputRT.height;
		var aspectGap = aspect2 / aspect1;

		var vMirrored = webCamTexture.videoVerticallyMirrored;
		var scale = new Vector2(aspectGap, vMirrored ? -1 : 1);
		var offset = new Vector2((1 - aspectGap) / 2, vMirrored ? 1 : 0);

		Graphics.Blit(webCamTexture, inputRT, scale, offset);
	}

	void SetCamera()
	{
		int length = WebCamTexture.devices.Length;
		Debug.Log("length" + length);
		if (length == 0) return;
		try
		{
			webCamTexture.Stop();
		}
		catch { }
		index++;
		if (index == length) index = 0;
		Debug.Log("idx" + index);
		webCamTexture = new WebCamTexture(WebCamTexture.devices[index].name, (int)webCamResolution.x, (int)webCamResolution.y, 60);
		webCamTexture.Play();

		Debug.Log(index + ": " + WebCamTexture.devices[index].name);

		inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0);
	}

	void OnDestroy()
	{
		if (webCamTexture != null)
		{
			webCamTexture.Stop();
			Destroy(webCamTexture);
		}
		if (inputRT != null) Destroy(inputRT);
	}

	public WebCamTexture GetWebcam() => webCamTexture;
}
