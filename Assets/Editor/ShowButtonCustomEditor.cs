using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIShowButton))]
public class ShowButtonCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty showUINameProp = serializedObject.FindProperty("showUIName");
        SerializedProperty isPopUpProp = serializedObject.FindProperty("isPopUp");
        SerializedProperty fieldToPosProp = serializedObject.FindProperty("_popUpTrm");

        EditorGUILayout.PropertyField(showUINameProp);
        EditorGUILayout.PropertyField(isPopUpProp);

        if (isPopUpProp.boolValue)
        {
            EditorGUILayout.PropertyField(fieldToPosProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
