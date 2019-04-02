using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationTriggers : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NarrationTrigger"))
        {
            switch (other.GetComponent<NarrationTriggerID>().id)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
    }
}
