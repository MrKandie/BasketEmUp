using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField]
    PlayerController pc;

    public void HandoffEnd()
    {
        pc.doingHandoff = false;
        pc.handoffTarget = null;
    }
}
