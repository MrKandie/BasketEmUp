﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObjectCheck : MonoBehaviour
{
    public ActivableObject[] objectsToActivate;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var truc in objectsToActivate)
        {
            truc.activationChecker = this;
        }
    }

    public bool CheckIfAllObjectsActivated()
    {
        int activatedObjects=0;
        foreach(var objects in objectsToActivate)
        {
            if (objects.activated)
            {
                activatedObjects++;
            }
        }

        if (activatedObjects == objectsToActivate.Length)
        {
            print("Tout les objets ont étés activés");
            foreach (var objects in objectsToActivate)
            {
                objects.FinalActivate();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ActivateObjectDebug(int index)
    {
        objectsToActivate[index].Activate();
    }
}