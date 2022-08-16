using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSphere : MonoBehaviour
{
    GameObject[] HandPoint;

    [SerializeField, Range(0, 10)]
    float Ratio;

    [SerializeField, Range(0, 10000)]
    float XYRatio;

    void Start()
    {
        HandPoint = new GameObject[this.transform.childCount];

        for (int i = 0; i < this.transform.childCount; i++)
        {
            HandPoint[i] = this.transform.GetChild(i).gameObject;
        }
    }

    public void MovePoint(Hands[] MovePoints)
    {
        int index = 0;
        foreach (Hands MovePoint in MovePoints)
        {
            HandsPoint Point = MovePoint.Point;
            HandPoint[index].transform.position =
                new Vector3(Point.x / XYRatio,
                    -Point.y / XYRatio,
                    Point.z * Ratio);
            index++;
        }
    }
}
