using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Paon.NInput;
public class LeftHandMove : MonoBehaviour
{
    GameObject hand;
    public GameObject handProvider;
    LeftHandMoveProvider inputProvider;
    GameObject player;
    void Start()
    {
        hand = this.gameObject;
        inputProvider = handProvider.GetComponent<LeftHandMoveProvider>();

    }
    void Update()
    {
        Vector2 coords = inputProvider.GetPosition();
        hand.transform.localPosition = new Vector3((coords.x - 520) * -0.01f, (coords.y - 300) * -0.01f, hand.transform.localPosition.z);
        if (inputProvider.GetInput() == "up")
        {
            hand.transform.Translate(Vector3.up * 0.01f);
        }
        else if (inputProvider.GetInput() == "down")
        {
            hand.transform.Translate(Vector3.down * 0.01f);
        }
        else if (inputProvider.GetInput() == "left")
        {
            hand.transform.Translate(Vector3.left * 0.01f);
        }
        else if (inputProvider.GetInput() == "right")
        {
            hand.transform.Translate(Vector3.right * 0.01f);
        }
        //Debug.Log(inputProvider.GetInput());
    }
}
