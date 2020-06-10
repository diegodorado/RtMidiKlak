using UnityEngine;
using System.Reflection;
using Klak.Wiring;
using System.Collections;

[AddComponentMenu("Klak/Wiring/Output/RtMidi/RtNoteOut")]
public class RtNoteOut : NodeBase
{
    #region Editable properties
    [SerializeField]
    byte _channel = 0;
    [SerializeField]
    byte _note = 60;
    [SerializeField]
    byte _velocity = 100;
    [SerializeField]
    float _duration = 0.2f;
    #endregion

    #region Node I/O

    [Inlet]
    public void Bang()
    {
        RtMidiDriver.SendNoteOn(_channel, _note, _velocity);
        StartCoroutine(waitNoteOff());
    }

    private IEnumerator waitNoteOff()
    {
        yield return new WaitForSeconds(_duration);
        RtMidiDriver.SendNoteOff(_channel, _note);
    }

    #endregion
}
