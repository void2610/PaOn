using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NaturePlay
{
    public class CanvasData
    {
        public int num = 0;

        public ObjectData[] objects;

        public void AddObject(GameObject obj)
        {
            //名前を部分検索して親prefab名に変換
            string name = obj.name;
            if (name.Contains("shovel"))
            {
                name = "shovel";
            }
            else if (name.Contains("BlackLine"))
            {
                name = "BlackLine";
            }
            else if (name.Contains("BlueLine"))
            {
                name = "BlueLine";
            }
            else if (name.Contains("Bucket"))
            {
                name = "Bucket";
            }
            else if (name.Contains("BlackCrayon"))
            {
                name = "BlackCrayon";
            }
            else if (name.Contains("BlueCrayon"))
            {
                name = "BlueCrayon";
            }
            else if (name.Contains("Flower"))
            {
                name = "Flower";
            }
            else if (name.Contains("Insect"))
            {
                name = "Insect";
            }
            else if (name.Contains("Scoop"))
            {
                name = "Scoop";
            }
            else if (name.Contains("Stone1"))
            {
                name = "Stone1";
            }
            else if (name.Contains("Stone2"))
            {
                name = "Stone2";
            }
            else if (name.Contains("Stone3"))
            {
                name = "Stone3";
            }
            else if (name.Contains("Leaf1"))
            {
                name = "Leaf1";
            }
            else if (name.Contains("Leaf2"))
            {
                name = "Leaf2";
            }
            else if (name.Contains("Leaf3"))
            {
                name = "Leaf3";
            }

            objects[num] =
                new ObjectData(name,
                    obj.transform.position,
                    obj.transform.eulerAngles,
                    obj.transform.localScale);
            num++;
        }

        public void Save()
        {
        }
    }

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
