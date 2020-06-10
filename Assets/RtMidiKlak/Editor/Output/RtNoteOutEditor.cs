using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RtNoteOut))]
public class RtNoteOutEditor : Editor
{
    SerializedProperty _channel;
    SerializedProperty _note;
    SerializedProperty _velocity;
    SerializedProperty _duration;

    void OnEnable()
    {
        _channel = serializedObject.FindProperty("_channel");
        _note = serializedObject.FindProperty("_note");
        _velocity = serializedObject.FindProperty("_velocity");
        _duration = serializedObject.FindProperty("_duration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_channel);
        EditorGUILayout.PropertyField(_note);
        EditorGUILayout.PropertyField(_velocity);
        EditorGUILayout.PropertyField(_duration);

        serializedObject.ApplyModifiedProperties();
    }
}
