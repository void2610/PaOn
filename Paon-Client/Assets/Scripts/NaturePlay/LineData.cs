using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Paon.NNaturePlay
{
    [Serializable]
    public class LineData
    {
        public Vector3[] lines = new Vector3[500];

        public string color = "BlackLine";

        public void AddLine(Vector3[] positions)
        {
            lines = positions;
        }

        public void SetColor(string name)
        {
            if (name.Contains("Black"))
            {
                color = "BlackLine";
            }
            else if (name.Contains("Blue"))
            {
                color = "BlueLine";
            }
        }

        public void Save(string name)
        {
            string jsonString = JsonUtility.ToJson(this);

            Debug.Log (jsonString);
            string path =
                Application.dataPath +
                "/Resources/NaturePlay/" +
                name +
                ".json";
            StreamWriter writer = new StreamWriter(path, false); //初めに指定したデータの保存先を開く
            writer.WriteLine (jsonString); //JSONデータを書き込み
            writer.Flush(); //バッファをクリアする
            writer.Close(); //ファイルをクローズする
        }
    }
}
