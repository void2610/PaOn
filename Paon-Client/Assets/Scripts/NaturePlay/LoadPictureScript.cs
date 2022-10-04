using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Paon.NNaturePlay
{
    public class LoadPictureScript : MonoBehaviour
    {
        private GameObject[] pictures = new GameObject[5];

        private string path;

        ///<summary>
        ///新しい5枚の写真を読み込んで表示するメソッド
        ///</summary>
        /// <returns>void</returns>
        void LoadPicture()
        {
            //ファイル名
            string[] files =
                Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);
            Array.Sort (files);
            int n = 0;
            if (files.Length < 5)
            {
                n = files.Length;
            }
            else
            {
                n = 5;
            }
            for (int i = 0; i < n; i++)
            {
                if (files[i] != null)
                {
                    //Debug.Log(files[i]);
                    byte[] bytes = File.ReadAllBytes(files[i]);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage (bytes);
                    pictures[i].GetComponent<Renderer>().material.mainTexture =
                        texture;
                }
            }
        }

        void Start()
        {
            path = Application.dataPath + "/Resources/NaturePlay";
            pictures[0] = GameObject.Find("Picture1");
            pictures[1] = GameObject.Find("Picture2");
            pictures[2] = GameObject.Find("Picture3");
            pictures[3] = GameObject.Find("Picture4");
            pictures[4] = GameObject.Find("Picture5");
            LoadPicture();
        }
    }
}
