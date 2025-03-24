using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCam : MonoBehaviour
{
    public GameObject vCam2;

        private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Cam Trigger":
                print("upper");
                vCam2.SetActive(true);
            break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Cam Trigger":
                print("down");
                vCam2.SetActive(false);
            break;
        }
    }

}
