using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNaturePlay
{
    public class LotateCrayonScript : MonoBehaviour
    {
        private GameObject nearCanvas;

        ///<summary>
        ///対象オブジェクトから一番近い指定タグオブジェクトを探すメソッド
        ///</summary>
        /// <returns>最近のオブジェクト</returns>
        /// <param name="nowObj">対象のオブジェクト</param>
        /// <param name="tagName">探したいタグ名</param>
        GameObject serchTag(GameObject nowObj, string tagName)
        {
            float tmpDis = 0;
            float nearDis = 0;
            GameObject targetObj = null;
            foreach (GameObject
                obs
                in
                GameObject.FindGameObjectsWithTag(tagName)
            )
            {
                tmpDis =
                    Vector3
                        .Distance(obs.transform.position,
                        nowObj.transform.position);
                if (nearDis == 0 || nearDis > tmpDis)
                {
                    nearDis = tmpDis;
                    targetObj = obs;
                }
            }
            return targetObj;
        }

        void Update()
        {
            nearCanvas = serchTag(this.gameObject, "CanvasTag");
            this.gameObject.transform.eulerAngles =
                new Vector3(nearCanvas.transform.eulerAngles.x,
                    nearCanvas.transform.eulerAngles.y - 90,
                    nearCanvas.transform.eulerAngles.z);
        }
    }
}
