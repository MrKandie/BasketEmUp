using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField]
    PlayerController pc;

    public void HandoffEnd()
    {
        print("endHandoff");
        pc.doingHandoff = false;
        pc.handoffTarget = null;
    }
}
