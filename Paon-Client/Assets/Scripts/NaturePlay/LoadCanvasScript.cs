using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNaturePlay
{
    public class LoadCanvasScript : MonoBehaviour
    {
        string inputString;

        CanvasData cd = new CanvasData();

        LineData ld = new LineData();

        CanvasData loadCanvasData(string name)
        {
            string jsonString =
                Resources.Load<TextAsset>("NaturePlay/" + name).ToString();
            CanvasData canvasData =
                JsonUtility.FromJson<CanvasData>(jsonString);
            return canvasData;
        }

        LineData loadLineData(string name)
        {
            string jsonString =
                Resources.Load<TextAsset>("NaturePlay/" + name).ToString();
            LineData canvasData = JsonUtility.FromJson<LineData>(jsonString);
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

        void SetLine(LineData ld)
        {
            LineRenderer lr =
                Instantiate(Resources.Load<GameObject>("Props/" + ld.color),
                Vector3.zero,
                Quaternion.identity).GetComponent<LineRenderer>();
            for (int i = 0; i < ld.lines.Length; i++)
            {
                if (ld.lines[i] != null)
                {
                    //LineRendererからPositionsのサイズを取得
                    int NextPositionIndex = lr.positionCount;

                    //LineRendererのPositionsのサイズを増やす
                    lr.positionCount = NextPositionIndex + 1;

                    //LineRendererのPositionsに現在のコントローラーの位置情報を追加
                    lr
                        .SetPosition(NextPositionIndex,
                        this.gameObject.transform.position - ld.lines[i]);
                }
            }
            Debug.Log("loaded " + ld.color);
        }

        void Start()
        {
            cd = loadCanvasData("Canvas_20228231484");
            SetObject (cd);
            ld = loadLineData("Line0_20228231484");
            SetLine (ld);
        }

        void Update()
        {
        }
    }
}
