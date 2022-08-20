using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNaturePlay
{
    public class LoadCanvasScript : MonoBehaviour
    {
        string inputString;

        CanvasData cd = new CanvasData();

        CanvasData loadCanvasData(string name)
        {
            string jsonString =
                Resources.Load<TextAsset>("NaturePlay/" + name).ToString();
            CanvasData canvasData =
                JsonUtility.FromJson<CanvasData>(jsonString);
            return canvasData;
        }

        void SetObject(CanvasData canvasData)
        {
            for (int i = 0; i < 100; i++)
            {
                if (canvasData.objects[i].name != "")
                {
                    Instantiate(Resources
                        .Load<GameObject>("Props/" +
                        canvasData.objects[i].name),
                    new Vector3(this.gameObject.transform.position.x -
                        canvasData.objects[i].localPosition.x,
                        this.gameObject.transform.position.y -
                        canvasData.objects[i].localPosition.y,
                        this.gameObject.transform.position.z -
                        canvasData.objects[i].localPosition.z),
                    Quaternion
                        .Euler(new Vector3(canvasData
                                .objects[i]
                                .localRotation
                                .x,
                            canvasData.objects[i].localRotation.y,
                            canvasData.objects[i].localRotation.z)));
                    Debug.Log("loaded " + canvasData.objects[i].name);
                }
            }
        }

        void Start()
        {
            cd = loadCanvasData("Canvas_202282134512");
            SetObject (cd);
        }

        void Update()
        {
        }
    }
}
