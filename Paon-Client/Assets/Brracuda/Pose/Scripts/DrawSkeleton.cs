using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSkeleton : MonoBehaviour
{
    public GameObject[] keypoints;

    private GameObject[] lines;

    private LineRenderer[] _lineRenderer;

    private int[][] joinPairs;

    private float lineWidth = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        int numPairs = keypoints.Length + 1;

        lines = new GameObject[numPairs];

        _lineRenderer = new LineRenderer[numPairs];

        joinPairs = new int[numPairs][];

        InitializeSkeleton();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RenderSkeleton();
    }

    private void InitializeLine(
        int pairIndex,
        int startIndex,
        int endIndex,
        float width,
        Color color
    )
    {
        joinPairs[pairIndex] = new int[] { startIndex, endIndex };

        string name =
            $"{keypoints[startIndex].name}_to_{keypoints[endIndex].name}";
        lines[pairIndex] = new GameObject(name);

        _lineRenderer[pairIndex] =
            lines[pairIndex].AddComponent<LineRenderer>();

        _lineRenderer[pairIndex].material =
            new Material(Shader.Find("Unlit/Color"));

        _lineRenderer[pairIndex].material.color = color;

        _lineRenderer[pairIndex].positionCount = 2;

        _lineRenderer[pairIndex].startWidth = width;

        _lineRenderer[pairIndex].endWidth = width;
    }

    private void InitializeSkeleton()
    {
        //magenta
        //Nose to left eye
        InitializeLine(0, 0, 1, lineWidth, Color.magenta);

        //Nose to right eye
        InitializeLine(1, 0, 2, lineWidth, Color.magenta);

        //Left eye to left ear
        InitializeLine(2, 1, 3, lineWidth, Color.magenta);

        //Right eye to right ear
        InitializeLine(3, 2, 4, lineWidth, Color.magenta);

        //red
        //Left shoulder to right shoulder
        InitializeLine(4, 5, 6, lineWidth, Color.red);

        //Left shoulder to left hip
        InitializeLine(5, 5, 11, lineWidth, Color.red);

        //Right shoulder to right hip
        InitializeLine(6, 6, 12, lineWidth, Color.red);

        //Left shoulder to right hip
        InitializeLine(7, 5, 12, lineWidth, Color.red);

        //Right shoulder to left hip
        InitializeLine(8, 6, 11, lineWidth, Color.red);

        //Left hip to right hip
        InitializeLine(9, 11, 12, lineWidth, Color.red);

        //green
        //Left Arm
        InitializeLine(10, 5, 7, lineWidth, Color.green);
        InitializeLine(11, 7, 9, lineWidth, Color.green);

        //Right Arm
        InitializeLine(12, 6, 8, lineWidth, Color.green);
        InitializeLine(13, 8, 10, lineWidth, Color.green);

        //blue
        //Left Leg
        InitializeLine(14, 11, 13, lineWidth, Color.blue);
        InitializeLine(15, 13, 15, lineWidth, Color.blue);

        //Right Leg
        InitializeLine(16, 12, 14, lineWidth, Color.blue);
        InitializeLine(17, 14, 16, lineWidth, Color.blue);
    }

    private void RenderSkeleton()
    {
        for (int i = 0; i < joinPairs.Length; i++)
        {
            int startpointIndex = joinPairs[i][0];

            int endpointIndex = joinPairs[i][1];

            GameObject startingKeyPoint = keypoints[startpointIndex];
            GameObject endingKeyPoint = keypoints[endpointIndex];

            Vector3 startPos =
                new Vector3(startingKeyPoint.transform.position.x,
                    startingKeyPoint.transform.position.y,
                    startingKeyPoint.transform.position.z);

            Vector3 endPos =
                new Vector3(endingKeyPoint.transform.position.x,
                    endingKeyPoint.transform.position.y,
                    endingKeyPoint.transform.position.z);

            if (
                startingKeyPoint.activeInHierarchy &&
                endingKeyPoint.activeInHierarchy
            )
            {
                _lineRenderer[i].gameObject.SetActive(true);

                _lineRenderer[i].SetPosition(0, startPos);

                _lineRenderer[i].SetPosition(1, endPos);
            }
            else
            {
                _lineRenderer[i].gameObject.SetActive(false);
            }
        }
    }
}
