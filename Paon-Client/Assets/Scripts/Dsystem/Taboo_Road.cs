using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dsystem
{
  public class Taboo_Road : Taboo
  {
    public override void Start()
    {
      base.Start();
      key = "Taboo_Road";
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
