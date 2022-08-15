using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCamera : MonoBehaviour
{
    Vector3 StartPos;

    [SerializeField]
    GameObject Target;

    void Start()
    {
        StartPos = Target.transform.position - this.transform.position;
    }

    void Update()
    {
        this.transform.position = Target.transform.position - StartPos;
    }
}
