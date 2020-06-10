using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RtMidi.LowLevel;

class RtMidiKlakWindow : EditorWindow
{
    #region Custom Editor Window Code

    [MenuItem("Window/RtMidi Klak")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<RtMidiKlakWindow>("RtMidiKlak");
    }

    void OnGUI()
    {
        var temp = "Last MIDI message:";
        temp += "\n" + RtMidiDriver.Instance.LastMessage;
        EditorGUILayout.HelpBox(temp, MessageType.None);
    }

    #endregion



    #region Update And Repaint
    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }
    #endregion
}

