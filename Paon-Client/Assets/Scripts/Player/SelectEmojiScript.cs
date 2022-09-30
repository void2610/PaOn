using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEmojiScript : MonoBehaviour
{
    public Material e1;

    public Material e2;

    public Material e3;

    public Material e4;

    public Material e5;

    GameObject Commorose;

    GameObject Now;

    int select = 1;

    void Start()
    {
        Commorose = GameObject.Find("Commorose");
        Now = GameObject.Find("NowEmoji");
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
            if (select == 1)
            {
                e1.color = new Color32(255, 255, 255, 255);
            }
            else if (select == 2)
            {
                e2.color = new Color32(255, 255, 255, 255);
            }
            else if (select == 3)
            {
                e3.color = new Color32(255, 255, 255, 255);
            }
            else if (select == 4)
            {
                e4.color = new Color32(255, 255, 255, 255);
            }
            else if (select == 5)
            {
                e5.color = new Color32(255, 255, 255, 255);
            }
        }
        else
        {
            e1.color = new Color32(255, 255, 255, 0);
            e2.color = new Color32(255, 255, 255, 0);
            e3.color = new Color32(255, 255, 255, 0);
            e4.color = new Color32(255, 255, 255, 0);
            e5.color = new Color32(255, 255, 255, 0);
            Commorose.transform.position = this.gameObject.transform.position;
        }
        if (Resources.Load<Sprite>("Picture/Emoji" + select) != null)
        {
            Now.GetComponent<Image>().sprite =
                Resources.Load<Sprite>("Picture/Emoji" + select);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (other.gameObject.name == "Emoji1")
            {
                select = 1;
            }
            else if (other.gameObject.name == "Emoji2")
            {
                select = 2;
            }
            else if (other.gameObject.name == "Emoji3")
            {
                select = 3;
            }
            else if (other.gameObject.name == "Emoji4")
            {
                select = 4;
            }
            else if (other.gameObject.name == "Emoji5")
            {
                select = 5;
            }
            else
            {
                select = 0;
            }
        }
    }
}
