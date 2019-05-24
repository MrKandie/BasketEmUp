using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ActivableObjectCheck))]
public class ActivableObjectCheckEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ActivableObjectCheck myScript = (ActivableObjectCheck)target;

        for(int i =0; i< myScript.objectsToActivate.Length; i++)
        { 
            if (GUILayout.Button("Validate Object N°" + i))
            {
                myScript.ActivateObjectDebug(i);
            }
        }
    }
}
