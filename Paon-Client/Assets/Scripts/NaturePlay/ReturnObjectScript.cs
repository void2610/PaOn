using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (
            collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "HoldableTag"
        )
        {
            collision.gameObject.transform.position =
                new Vector3(collision.gameObject.transform.position.x,
                    4,
                    collision.gameObject.transform.position.z);
        }
    }
}
