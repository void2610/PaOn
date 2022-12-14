using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Dsystem
{
  public class AlertScript : MonoBehaviour
  {

    private Image img;
    public bool isAlerted = false;
    void Start()
    {
      img = GameObject.Find("RedImage").GetComponent<Image>();
      img.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
      if (isAlerted)
      {
        this.img.color = new Color(0.5f, 0f, 0f, 0.5f);
      }
    }
  }
}
