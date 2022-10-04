using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Paon.NNaturePlay
{
    [Serializable]
    public class CanvasData
    {
        public ObjectData[] objects = new ObjectData[100];

        public int num = 0;

        ///<summary>
        ///引数のオブジェクトの種類と位置をObjectDataに保存するメソッド
        ///</summary>
        /// <returns>void</returns>
        /// <param name="obj">対象のオブジェクト</param>
        /// <param name="position">オブジェクトの座標</param>
        public void AddObject(GameObject obj, Vector3 position)
        {
            //名前を部分検索して親prefab名に変換
            string name = obj.name;
            if (name.Contains("shovel"))
            {
                name = "shovel";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("BlackLine"))
            {
                name = "BlackLine";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("BlueLine"))
            {
                name = "BlueLine";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Bucket"))
            {
                name = "Bucket";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("BlackCrayon"))
            {
                name = "BlackCrayon";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("BlueCrayon"))
            {
                name = "BlueCrayon";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Flower"))
            {
                name = "Flower";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Insect"))
            {
                name = "Insect";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Scoop"))
            {
                name = "Scoop";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Stone1"))
            {
                name = "Stone1";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Stone2"))
            {
                name = "Stone2";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Stone3"))
            {
                name = "Stone3";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Leaf1"))
            {
                name = "Leaf1";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Leaf2"))
            {
                name = "Leaf2";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Leaf3"))
            {
                name = "Leaf3";
                Debug.Log(name + " saved");
            }
            else if (name.Contains("Gold"))
            {
                name = "Gold";
                Debug.Log(name + " saved");
            }
            else
            {
                Debug.Log("Object name not found");
            }

            objects[num] =
                new ObjectData(name,
                    position,
                    obj.transform.eulerAngles,
                    obj.transform.localScale);
            num++;
        }

        ///<summary>
        ///格納したオブジェクトをJsonファイルで保存するメソッド
        ///</summary>
        /// <returns>void</returns>
        /// <param name="name">ファイル名</param>
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

    [Serializable]
    public class ObjectData
    {
        public string name;

        public Vector3 localPosition;

        public Vector3 localRotation;

        public Vector3 localScale;

        public ObjectData(
            string name,
            Vector3 localPosition,
            Vector3 localRotation,
            Vector3 localScale
        )
        {
            this.name = name;
            this.localPosition = localPosition;
            this.localRotation = localRotation;
            this.localScale = localScale;
        }
    }
}
