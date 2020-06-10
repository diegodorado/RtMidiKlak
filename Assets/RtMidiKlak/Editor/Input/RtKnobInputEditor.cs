using UnityEngine;
using UnityEditor;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(RtKnobInput))]
    public class RtKnobInputEditor : Editor
    {
        SerializedProperty _channel;
        SerializedProperty _knobNumber;
        SerializedProperty _responseCurve;
        SerializedProperty _interpolator;
        SerializedProperty _onEvent;
        SerializedProperty _offEvent;
        SerializedProperty _valueEvent;

        void OnEnable()
        {
            _channel = serializedObject.FindProperty("_channel");
            _knobNumber = serializedObject.FindProperty("_knobNumber");
            _responseCurve = serializedObject.FindProperty("_responseCurve");
            _interpolator = serializedObject.FindProperty("_interpolator");
            _onEvent = serializedObject.FindProperty("_onEvent");
            _offEvent = serializedObject.FindProperty("_offEvent");
            _valueEvent = serializedObject.FindProperty("_valueEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_channel);
            EditorGUILayout.PropertyField(_knobNumber);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_responseCurve);
            EditorGUILayout.PropertyField(_interpolator);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_onEvent);
            EditorGUILayout.PropertyField(_offEvent);
            EditorGUILayout.PropertyField(_valueEvent);

            if (EditorApplication.isPlaying &&
                !serializedObject.isEditingMultipleObjects)
            {
                var instance = (RtKnobInput)target;
                instance.debugInput =
                    EditorGUILayout.Slider("Debug", instance.debugInput, 0, 1);
                EditorUtility.SetDirty(target); // request repaint
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
