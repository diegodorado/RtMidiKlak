using UnityEngine;
using System.Reflection;
using Klak.Wiring;
using System;

[AddComponentMenu("Klak/Wiring/Output/RtMidi/RtCcOut")]
public class RtCcOut : NodeBase
{
    #region Editable properties
    [SerializeField]
    byte _channel = 0;
    [SerializeField]
    byte _number = 0;
    #endregion

    #region Node I/O
    int _lastVal = 0;

    [Inlet]
    public float input
    {
        set
        {
            if (!enabled) return;
            int val = (int) Mathf.Clamp01(value) * 127;
            if (val != _lastVal) {
                RtMidiDriver.SendControlChange(_channel, _number, val);
                _lastVal = val;
            }
        }
    }

    #endregion

}
