using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Coup { Dunk, LightPass, HeavyPass, Dash, Heal }

public class ComboManager : MonoBehaviour
{

    public List<Coup> actualCombo;

    public float comboSpeedModifier;
    public float comboDamageModifier;

    public float timeBeforeResetingCombo = 5;

    private float timeSinceLastUpdate;

    public bool hyperHeavyAttackReady = true;


    private void Awake()
    {
        actualCombo = new List<Coup>();
    }

    private void Update()
    {
        if (actualCombo.Count > 0)
        {
            timeSinceLastUpdate += Time.deltaTime;
            if (timeSinceLastUpdate >= timeBeforeResetingCombo)
            {
                ResetCombo();
            }
        }
    }

    public void AddCoup(Coup coup)
    {
        timeSinceLastUpdate = 0;
        actualCombo.Add(coup);
        if (coup != Coup.LightPass && coup != Coup.HeavyPass)
        {
            ResetCombo();
        } else
        {
            UpdateCombo();
        }
    }

    public void ResetCombo()
    {
        actualCombo.Clear();
        timeSinceLastUpdate = 0;
        UpdateCombo();
    }

    public void UpdateCombo()
    {
        hyperHeavyAttackReady = false;
        if (actualCombo.Count < 1) { return; }
        if (actualCombo.Count > 3) { ResetCombo(); }
        switch (actualCombo[0])
        {
            case Coup.LightPass:
                if (actualCombo.Count < 2) { return; }
                switch (actualCombo[1])
                {
                    case Coup.LightPass:
                        if (actualCombo.Count < 3) { return; }
                        switch (actualCombo[2])
                        {
                            case Coup.HeavyPass:
                                hyperHeavyAttackReady = true;
                                break;
                            default:
                                ResetCombo();
                                break;
                        }
                        break;
                }
                break;
        }
    }
}
