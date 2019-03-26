using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{

    public List<Enemy> enemies;
    public List<PlayerController> players;

    public List<iTarget> targetableObjects;


    private void Awake()
    {
        UpdateTargetableObjectsList();
    }

    public void RemoveTargetFromList(iTarget target)
    {
        targetableObjects.Remove(target);
    }

    public void UpdateTargetableObjectsList()
    {
        List<iTarget> targetableObjectsTemp = new List<iTarget>();
        var ss = FindObjectsOfType<MonoBehaviour>().OfType<iTarget>();
        foreach (iTarget s in ss)
        {
            targetableObjectsTemp.Add(s);
        }
        targetableObjects = targetableObjectsTemp;
    }
}
