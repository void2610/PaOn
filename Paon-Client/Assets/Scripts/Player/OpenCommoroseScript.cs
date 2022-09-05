using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCommoroseScript : MonoBehaviour
{
    GameObject Player;

    public GameObject Hand;

    public Material e1;

    public Material e2;

    public Material e3;

    public Material e4;

    public Material e5;

    void Start()
    {
        Player = GameObject.Find("PlayerBody");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            e1.color = new Color32(255, 255, 255, 110);
            e2.color = new Color32(255, 255, 255, 110);
            e3.color = new Color32(255, 255, 255, 110);
            e4.color = new Color32(255, 255, 255, 110);
            e5.color = new Color32(255, 255, 255, 110);
        }
        else
        {
            e1.color = new Color32(255, 255, 255, 0);
            e2.color = new Color32(255, 255, 255, 0);
            e3.color = new Color32(255, 255, 255, 0);
            e4.color = new Color32(255, 255, 255, 0);
            e5.color = new Color32(255, 255, 255, 0);
            this.gameObject.transform.position = this.Hand.transform.position;
        }
    }
}
