using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{

    public List<Enemy> enemies;
    public List<PlayerController> players;
    public Ball activeBall;

    private List<iTarget> allTargetableObjects;
    private List<iTarget> targetableAllies;
    private List<iTarget> targetableEnemies;
    private List<iTarget> targetableProps;


    private void Awake()
    {
        UpdateTargetableObjectsList();
        activeBall = FindObjectOfType<Ball>();
    }

    public void RemoveTargetFromList(iTarget target)
    {
        allTargetableObjects.Remove(target);
        targetableAllies.Remove(target);
        targetableEnemies.Remove(target);
        targetableProps.Remove(target);
    }

    public void UpdateTargetableObjectsList()
    {
        List<iTarget> targetableObjectsTemp = new List<iTarget>();
        List<iTarget> tempTargetableAllies = new List<iTarget>();
        List<iTarget> tempTargetableEnemies = new List<iTarget>();
        List<iTarget> tempTargetableProps = new List<iTarget>();

        var ss = FindObjectsOfType<MonoBehaviour>().OfType<iTarget>();
        foreach (iTarget s in ss)
        {
            targetableObjectsTemp.Add(s);
            MonoBehaviour targetScript = s as MonoBehaviour;
            if (s.GetType() == typeof(PlayerController))
            {
                tempTargetableAllies.Add(s);
            } else if (s.GetType().IsSubclassOf(typeof(Enemy)))
            {
                tempTargetableEnemies.Add(s);
            } else
            {
                tempTargetableProps.Add(s);
            }
        }
        allTargetableObjects = targetableObjectsTemp;
        targetableAllies = tempTargetableAllies;
        targetableEnemies = tempTargetableEnemies;
        targetableProps = tempTargetableProps;
    }

    public List<iTarget> GetAllTargetableObjects()
    {
        return allTargetableObjects;
    }

    public List<iTarget> GetTargetableAllies()
    {
        return targetableAllies;
    }

    public List<iTarget> GetTargetableEnemies()
    {
        return targetableEnemies;
    }

    public List<iTarget> GetTargetableProps()
    {
        return targetableProps;
    }
}
