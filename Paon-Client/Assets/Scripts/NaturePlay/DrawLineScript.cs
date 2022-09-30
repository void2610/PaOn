using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNatuePlay
{
    public class DrawLineScript : MonoBehaviour
    {
        //変数を用意
        //SerializeFieldをつけるとInspectorウィンドウからゲームオブジェクトやPrefabを指定できます。
        [SerializeField]
        GameObject LineObjectPrefab;

        //現在描画中のLineObject
        private GameObject CurrentLineObject = null;

        private Vector3 tmp = Vector3.zero;

        public bool drawing = false;

        bool dtmp = false;

        void Start()
        {
            this.GetComponent<AudioSource>().Stop();
        }

        // Update is called once per frame
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
                        //PrefabからLineObjectを生成
                        CurrentLineObject =
                            Instantiate(LineObjectPrefab,
                            new Vector3(0, 0, 0),
                            Quaternion.identity);
                    }

                    //ゲームオブジェクトからLineRendererコンポーネントを取得
                    LineRenderer render =
                        CurrentLineObject.GetComponent<LineRenderer>();

                    //LineRendererからPositionsのサイズを取得
                    int NextPositionIndex = render.positionCount;

                    //LineRendererのPositionsのサイズを増やす
                    render.positionCount = NextPositionIndex + 1;

                    //LineRendererのPositionsに現在のコントローラーの位置情報を追加
                    render.SetPosition (NextPositionIndex, pointer);
                }
                else if (!drawing)
                {
                    if (CurrentLineObject != null)
                    {
                        //現在描画中の線があったらnullにして次の線を描けるようにする。
                        CurrentLineObject = null;
                    }
                }
            }
            tmp = pointer;
            dtmp = drawing;
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("CanvasTag"))
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
