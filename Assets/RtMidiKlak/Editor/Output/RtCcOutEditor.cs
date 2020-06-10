using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RtCcOut))]
public class RtCcOutEditor : Editor
{
    SerializedProperty _channel;
    SerializedProperty _number;

    void OnEnable()
    {
        _channel = serializedObject.FindProperty("_channel");
        _number = serializedObject.FindProperty("_number");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_channel);
        EditorGUILayout.PropertyField(_number);

        serializedObject.ApplyModifiedProperties();
    }
}
