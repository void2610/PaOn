using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    MoveInputProvider inputProvider;

    public GameObject player;

    public GameObject es;

    public bool canMove = true;

    void Start()
    {
        inputProvider = es.GetComponent<MoveInputProvider>();
        player = this.gameObject;
    }

    void Update()
    {
        if (canMove)
        {
            if (inputProvider.GetInput() == "space")
            {
                player
                    .GetComponent<Rigidbody>()
                    .AddForce(Vector3.up * 0.25f, ForceMode.Impulse);
            }
            else if (inputProvider.GetInput() == "up")
            {
                player.transform.Translate(Vector3.forward * 0.1f);
            }
            else if (inputProvider.GetInput() == "down")
            {
                player.transform.Translate(Vector3.back * 0.03f);
            }
            else if (inputProvider.GetInput() == "left")
            {
                player.transform.Rotate(0, -0.3f, 0);
            }
            else if (inputProvider.GetInput() == "right")
            {
                player.transform.Rotate(0, 0.3f, 0);
            }
            //Debug.Log(inputProvider.GetInput());
        }
    }
}
