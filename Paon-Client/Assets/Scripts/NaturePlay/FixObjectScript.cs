using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNaturePlay
{
    public class FixObjectScript : MonoBehaviour
    {
        public CanvasData canvasData = new CanvasData();

        public bool saving = false;

        ///<summary>
        ///クレヨンの線をJsonファイルで保存するメソッド
        ///</summary>
        /// <returns>void</returns>
        public void SaveLine(
            LineRenderer lr,
            string name,
            int num,
            string lineName
        )
        {
            LineData ld = new LineData();
            ld.SetColor (lineName);
            Vector3[] positions = new Vector3[lr.positionCount];
            lr.GetPositions (positions);
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] =
                    this.gameObject.transform.position - positions[i];
            }
            ld.AddLine (positions);
            ld.Save("Line" + num + "_" + name);
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("HoldableTag"))
            {
                other.gameObject.GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezeAll;
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                other.gameObject.transform.eulerAngles =
                    new Vector3(90,
                        other.gameObject.transform.eulerAngles.y,
                        -90);
            }

            if (saving)
            {
                if (other.gameObject.name != "PlayerBody")
                {
                    if (
                        other.CompareTag("HoldableTag") ||
                        other.CompareTag("LineObjectTag")
                    )
                    {
                        Debug.Log("try to save " + other.gameObject.name);
                        canvasData
                            .AddObject(other.gameObject,
                            new Vector3(this.gameObject.transform.position.x -
                                other.gameObject.transform.position.x,
                                this.gameObject.transform.position.y -
                                other.gameObject.transform.position.y,
                                this.gameObject.transform.position.z -
                                other.gameObject.transform.position.z));
                        GameObject.Destroy(other.gameObject);
                    }
                    else
                    {
                        DateTime dt = DateTime.Now;
                        string name =
                            dt.Year.ToString() +
                            dt.Month.ToString() +
                            dt.Day.ToString() +
                            dt.Hour.ToString() +
                            dt.Minute.ToString() +
                            dt.Second.ToString();

                        //線を保存
                        GameObject[] lines =
                            GameObject.FindGameObjectsWithTag("LineObjectTag");
                        int num = 0;
                        foreach (GameObject line in lines)
                        {
                            SaveLine(line.GetComponent<LineRenderer>(),
                            name,
                            num,
                            line.name);
                            Debug.Log("try to save " + line.name);
                            num++;
                        }

                        //オブジェクトを保存
                        canvasData.Save("Canvas_" + name);
                        canvasData = new CanvasData();
                        saving = false;
                        Debug.Log("save successfull");
                    }
                }
                else
                {
                    Debug.Log("cant save Player");
                    saving = false;
                }
            }
        }
    }
}
