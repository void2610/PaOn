using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using UnityEngine;

public class LeftHandMove : MonoBehaviour
{
    GameObject hand;

    GameObject handProvider;

    LeftHandInputProvider inputProvider;

    GameObject player;

    public bool canMove = true;

    Vector2 coords;

    void Start()
    {
        hand = this.gameObject;
        inputProvider =
            GameObject
                .Find("LeftHandInputProvider")
                .GetComponent<LeftHandInputProvider>();
    }

    void Update()
    {
        coords = inputProvider.GetPosition();
        if (canMove)
        {
            /*hand.transform.localPosition =
                new Vector3((coords.x - 520) * -0.01f,
                    (coords.y - 300) * -0.01f,
                    hand.transform.localPosition.z);*/
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
        }
    }
}
