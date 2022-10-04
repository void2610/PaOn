using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNaturePlay
{
    public class DrawLineScript : MonoBehaviour
    {
        public GameObject LineObjectPrefab;

        private GameObject CurrentLineObject = null;

        private Vector3 tmp = Vector3.zero;

        public bool drawing = false;

        private bool dtmp = false;

        void Start()
        {
            this.GetComponent<AudioSource>().Stop();
        }

        void Update()
        {
            Vector3 pointer = this.gameObject.transform.position;

            if (!dtmp && drawing)
            {
                this.GetComponent<AudioSource>().Play();
            }
            if (dtmp && !drawing)
            {
                this.GetComponent<AudioSource>().Stop();
            }

            if (drawing && Time.frameCount % 3 == 0)
            {
                if (Mathf.Abs(Vector3.Distance(tmp, pointer)) > 0.0f)
                {
                    Debug.Log("drawing");
                    if (CurrentLineObject == null)
                    {
                        CurrentLineObject =
                            Instantiate(LineObjectPrefab,
                            new Vector3(0, 0, 0),
                            Quaternion.identity);
                    }

                    LineRenderer render =
                        CurrentLineObject.GetComponent<LineRenderer>();
                    int NextPositionIndex = render.positionCount;
                    render.positionCount = NextPositionIndex + 1;
                    render.SetPosition (NextPositionIndex, pointer);
                }
                else if (!drawing)
                {
                    if (CurrentLineObject != null)
                    {
                        CurrentLineObject = null;
                    }
                }
            }
            tmp = pointer;
            dtmp = drawing;
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "CanvasTag")
            {
                drawing = true;
            }
            else
            {
                drawing = false;
            }
        }

        void OnTriggerExit(Collider other)
        {
            drawing = false;
        }
    }
}
