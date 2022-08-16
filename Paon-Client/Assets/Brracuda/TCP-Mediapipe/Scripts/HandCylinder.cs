using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCylinder : MonoBehaviour
{
    GameObject[] HandPoint;

    [SerializeField, Range(0, 10)]
    float Ratio;

    [SerializeField, Range(0, 10000)]
    float XYRatio;

    List<int[]>
        landmark_line_ids =
            new List<int[]> {
                new int[] { 0, 1 },
                new int[] { 1, 5 },
                new int[] { 5, 9 },
                new int[] { 9, 13 },
                new int[] { 13, 17 },
                new int[] { 17, 0 },
                new int[] { 1, 2 },
                new int[] { 2, 3 },
                new int[] { 3, 4 },
                new int[] { 5, 6 },
                new int[] { 6, 7 },
                new int[] { 7, 8 },
                new int[] { 9, 10 },
                new int[] { 10, 11 },
                new int[] { 11, 12 },
                new int[] { 13, 14 },
                new int[] { 14, 15 },
                new int[] { 15, 16 },
                new int[] { 17, 18 },
                new int[] { 18, 19 },
                new int[] { 19, 20 }
            };

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
        foreach (int[] line in landmark_line_ids)
        {
            HandsPoint Point = MovePoints[line[0]].Point;
            Vector3 Point1 =
                new Vector3(Point.x / XYRatio,
                    -Point.y / XYRatio,
                    Point.z * Ratio);
            Point = MovePoints[line[1]].Point;
            Vector3 Point2 =
                new Vector3(Point.x / XYRatio,
                    -Point.y / XYRatio,
                    Point.z * Ratio);
            Vector3 Point2ToPoint1 = (Point1 - Point2);
            float Len = Point2ToPoint1.magnitude / 2;

            HandPoint[index].transform.position =
                Point1 + ((Point2 - Point1) * 0.5f);
            HandPoint[index].transform.localScale =
                new Vector3(0.01f, Len, 0.01f);
            HandPoint[index].transform.up = (Point2ToPoint1.normalized);
            index++;
        }
    }
}
