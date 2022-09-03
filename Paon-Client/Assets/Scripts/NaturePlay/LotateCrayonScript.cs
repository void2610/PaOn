using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotateCrayonScript : MonoBehaviour
{
    private GameObject nearCanvas;

    GameObject serchTag(GameObject nowObj, string tagName)
    {
        float tmpDis = 0;
        float nearDis = 0;
        GameObject targetObj = null;
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
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

    void Start()
    {
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
