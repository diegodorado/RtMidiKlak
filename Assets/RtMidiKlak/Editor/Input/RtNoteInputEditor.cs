using UnityEngine;
using UnityEditor;


    [CanEditMultipleObjects]
    [CustomEditor(typeof(RtNoteInput))]
    public class RtNoteInputEditor : Editor
    {
        SerializedProperty _channel;
        SerializedProperty _noteFilter;
        SerializedProperty _noteName;
        SerializedProperty _lowestNote;
        SerializedProperty _highestNote;

        SerializedProperty _velocityCurve;
        SerializedProperty _offValue;
        SerializedProperty _onValue;
        SerializedProperty _interpolator;

        SerializedProperty _noteOnEvent;
        SerializedProperty _noteOnVelocityEvent;
        SerializedProperty _noteOffEvent;
        SerializedProperty _valueEvent;

        void OnEnable()
        {
            _channel = serializedObject.FindProperty("_channel");
            _noteFilter = serializedObject.FindProperty("_noteFilter");
            _noteName = serializedObject.FindProperty("_noteName");
            _lowestNote = serializedObject.FindProperty("_lowestNote");
            _highestNote = serializedObject.FindProperty("_highestNote");

            _velocityCurve = serializedObject.FindProperty("_velocityCurve");
            _offValue = serializedObject.FindProperty("_offValue");
            _onValue = serializedObject.FindProperty("_onValue");
            _interpolator = serializedObject.FindProperty("_interpolator");

            _noteOnEvent = serializedObject.FindProperty("_noteOnEvent");
            _noteOnVelocityEvent = serializedObject.FindProperty("_noteOnVelocityEvent");
            _noteOffEvent = serializedObject.FindProperty("_noteOffEvent");
            _valueEvent = serializedObject.FindProperty("_valueEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_channel);
            EditorGUILayout.PropertyField(_noteFilter);

            var noteFilter = (RtNoteInput.NoteFilter)_noteFilter.enumValueIndex;

            if (noteFilter == RtNoteInput.NoteFilter.NoteName)
                EditorGUILayout.PropertyField(_noteName);

            if (noteFilter == RtNoteInput.NoteFilter.NoteNumber) {
                EditorGUILayout.PropertyField(_lowestNote);
                EditorGUILayout.PropertyField(_highestNote);
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_velocityCurve);
            EditorGUILayout.PropertyField(_offValue);
            EditorGUILayout.PropertyField(_onValue);
            EditorGUILayout.PropertyField(_interpolator);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_noteOnEvent);
            EditorGUILayout.PropertyField(_noteOnVelocityEvent);
            EditorGUILayout.PropertyField(_noteOffEvent);
            EditorGUILayout.PropertyField(_valueEvent);

            if (EditorApplication.isPlaying &&
                !serializedObject.isEditingMultipleObjects)
            {
                var instance = (RtNoteInput)target;
                instance.debugInput =
                    EditorGUILayout.Toggle("Debug", instance.debugInput);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
