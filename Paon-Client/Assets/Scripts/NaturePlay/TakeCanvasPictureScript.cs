using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NNaturePlay
{
    public class TakeCanvasPictureScript : MonoBehaviour
    {
        [SerializeField]
        private static readonly string CAPUTURED_PICTURE_SAVE_DIRECTORY = "/";

        [SerializeField]
        private Camera _captureCamera;

        public IEnumerator Capture()
        {
            //_captureCameraに写るものを800*600でPNG画像として保存
            var coroutine =
                StartCoroutine(CaptureFromCamera(512, 512, _captureCamera));
            yield return coroutine;
        }

        /// <summary>
        /// CaptureMain
        /// </summary>
        /// <param name="width">横解像度</param>
        /// <param name="height">縦解像度</param>
        private IEnumerator
        CaptureFromCamera(int width, int height, Camera camera)
        {
            var d_width = camera.targetTexture.width;
            var d_height = camera.targetTexture.height;
            Debug.Log("d_width:" + d_width + " d_height:" + d_height);

            Texture2D tex =
                new Texture2D(width, height, TextureFormat.ARGB32, false);

            // if (camera.targetTexture != null)
            // {
            //     camera.targetTexture.Release();
            // }
            // camera.targetTexture = new RenderTexture(width, height, 24);
            yield return new WaitForEndOfFrame();

            RenderTexture.active = camera.targetTexture;
            tex
                .ReadPixels(new Rect(0,
                    0,
                    camera.targetTexture.width,
                    camera.targetTexture.height),
                0,
                0);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            DateTime dt = DateTime.Now;
            string name =
                dt.Year.ToString() +
                dt.Month.ToString() +
                dt.Day.ToString() +
                dt.Hour.ToString() +
                dt.Minute.ToString() +
                dt.Second.ToString();
            string savePath =
                Application.dataPath +
                "/Resources/NaturePlay/" +
                "Canvas_" +
                name +
                ".png";
            File.WriteAllBytes (savePath, bytes);
            captured = true;

            //Destroy (tex);
            //if (camera.targetTexture != null) camera.targetTexture.Release();
            yield break;
        }

        private bool captured = false;

        void Start()
        {
            _captureCamera = this.gameObject.GetComponent<Camera>();
        }

        void Update()
        {
            if (captured)
            {
                //GameObject.Destroy(this.gameObject);
            }
            else
            {
                StartCoroutine(Capture());
            }
        }
    }
}
