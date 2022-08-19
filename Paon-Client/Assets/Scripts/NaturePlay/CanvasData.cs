using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NatuePlay
{
    public class CanvasData
    {
        public ObjectData[] objects;

        void AddObject(GameObject obj)
        {
        }
    }

    public class ObjectData
    {
        public int type;

        public Vector3 localPosition;

        public Quaternion localRotation;

        public Vector3 localScale;

        public ObjectData(
            int type,
            Vector3 localPosition,
            Quaternion localRotation,
            Vector3 localScale
        )
        {
            this.type = type;
            this.localPosition = localPosition;
            this.localRotation = localRotation;
            this.localScale = localScale;
        }
    }
}
