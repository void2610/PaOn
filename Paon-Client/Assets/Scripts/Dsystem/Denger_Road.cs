using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dsystem
{
  public class Denger_Road : Denger
  {
    public override void Start()
    {
      base.Start();
      key = "Denger_Road";
      name = "どうろにでるな";
      description = "しぬぞ";
    }

    public override void Update()
    {
      base.Update();

    }
    void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.name == "PlayerBody")
      {
        Activation();
      }
    }
  }
}
