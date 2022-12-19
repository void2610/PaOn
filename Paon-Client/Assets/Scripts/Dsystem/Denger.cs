using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dsystem{
    public class Denger : MonoBehaviour
    {
        public string name = "No Name";
        public string description = "No description";

        private AlertManageScript ams;

        private float sTime;
        private float coolDown = 10.0f;
        private bool canActivate = true;
        public virtual void Start(){
            ams = GameObject.Find("AlertManager").GetComponent<AlertManageScript>();
        }

        public virtual void Update(){
            if(Time.time - sTime > coolDown){
                canActivate = true;
            }
        }
        public void ShowAlert(){
            ams.nowName = name;
            ams.nowDescription = description;
            ams.isAlerted = true;
        }
        public virtual void Activation(){
            if(canActivate){
                canActivate = false;
                sTime = Time.time;
                ShowAlert();
            }
        }
    }
}