using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationPlayerController : MonoBehaviour
{
    public Animator myAnim;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (myAnim.GetFloat("Blend") > .25f)
            {
                myAnim.SetFloat("Blend", 0);
            }
            else
            {
                myAnim.SetFloat("Blend", .5f);
            }
        }
    }
}
