using UnityEngine;
using Klak.Math;
using Klak.Wiring;
using RtMidi.LowLevel;

[AddComponentMenu("Klak/Wiring/Input/RtMIDI/Rt Note Input")]
public class RtNoteInput : NodeBase
{
    #region Editable properties

    public enum NoteFilter
    {
        Off, NoteName, NoteNumber
    }

    public enum NoteName
    {
        C, CSharp, D, DSharp, E, F, FSharp, G, GSharp, A, ASharp, B
    }

    [SerializeField]
    MidiChannel _channel = MidiChannel.All;

    [SerializeField]
    NoteFilter _noteFilter = NoteFilter.Off;

    [SerializeField]
    NoteName _noteName;

    [SerializeField]
    int _lowestNote = 60; // C4

    [SerializeField]
    int _highestNote = 60; // C4

    [SerializeField]
    AnimationCurve _velocityCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField]
    float _offValue = 0.0f;

    [SerializeField]
    float _onValue = 1.0f;

    [SerializeField]
    FloatInterpolator.Config _interpolator = new FloatInterpolator.Config(
        FloatInterpolator.Config.InterpolationType.DampedSpring, 30
    );

    #endregion

    #region Node I/O

    [SerializeField, Outlet]
    VoidEvent _noteOnEvent = new VoidEvent();

    [SerializeField, Outlet]
    FloatEvent _noteOnVelocityEvent = new FloatEvent();

    [SerializeField, Outlet]
    VoidEvent _noteOffEvent = new VoidEvent();

    [SerializeField, Outlet]
    FloatEvent _valueEvent = new FloatEvent();

    #endregion

    #region Private members

    FloatInterpolator _floatValue;

    bool CompareNoteToName(int number, NoteName name)
    {
        return (number % 12) == (int)name;
    }

    bool FilterNote(MidiChannel channel, int note)
    {
        if (_channel != MidiChannel.All && channel != _channel) return false;
        if (_noteFilter == NoteFilter.Off) return true;
        if (_noteFilter == NoteFilter.NoteName)
            return CompareNoteToName(note, _noteName);
        else // NoteFilter.Number
            return _lowestNote <= note && note <= _highestNote;
    }

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        if (!FilterNote(channel, note)) return;

        velocity = _velocityCurve.Evaluate(velocity);

        _noteOnEvent.Invoke();
        _noteOnVelocityEvent.Invoke(velocity);

        _floatValue.targetValue = _onValue * velocity;
    }

    void NoteOff(MidiChannel channel, int note)
    {
        if (!FilterNote(channel, note)) return;

        _noteOffEvent.Invoke();

        _floatValue.targetValue = _offValue;
    }

    #endregion

    #region MonoBehaviour functions

    void OnEnable()
    {
        RtMidiDriver.noteOnDelegate += NoteOn;
        RtMidiDriver.noteOffDelegate += NoteOff;
    }

    void OnDisable()
    {
        RtMidiDriver.noteOnDelegate -= NoteOn;
        RtMidiDriver.noteOffDelegate -= NoteOff;
    }

    void Start()
    {
        _floatValue = new FloatInterpolator(_offValue, _interpolator);
    }

    void Update()
    {
        _valueEvent.Invoke(_floatValue.Step());
    }

    #endregion

#if UNITY_EDITOR

    #region Editor Interface

    bool _debugInput;

    int debugNote
    {
        get
        {
            if (_noteFilter == NoteFilter.NoteName)
                return (int)_noteName + 60; // C4
            else
                return _lowestNote;
        }
    }

    public bool debugInput
    {
        get { return _debugInput; }
        set
        {
            if (!_debugInput)
                if (value) NoteOn(_channel, debugNote, 1);
                else
                if (!value) NoteOff(_channel, debugNote);
            _debugInput = value;
        }
    }

    #endregion

#endif
}

