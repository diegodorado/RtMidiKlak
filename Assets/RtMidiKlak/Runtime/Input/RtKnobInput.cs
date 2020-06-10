using UnityEngine;
using Klak.Math;
using Klak.Wiring;
using RtMidi.LowLevel;


[AddComponentMenu("Klak/Wiring/Input/RtMIDI/Rt Knob Input")]
public class RtKnobInput : NodeBase
{
    #region Editable properties

    [SerializeField]
    MidiChannel _channel = MidiChannel.All;

    [SerializeField]
    int _knobNumber = 0;

    [SerializeField]
    AnimationCurve _responseCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField]
    FloatInterpolator.Config _interpolator = new FloatInterpolator.Config(
        FloatInterpolator.Config.InterpolationType.DampedSpring, 30
    );

    #endregion

    #region Node I/O

    [SerializeField, Outlet]
    VoidEvent _onEvent = new VoidEvent();

    [SerializeField, Outlet]
    VoidEvent _offEvent = new VoidEvent();

    [SerializeField, Outlet]
    FloatEvent _valueEvent = new FloatEvent();

    #endregion

    #region Private members

    FloatInterpolator _floatValue;
    float _lastInputValue;


    void DoKnobUpdate(float inputValue)
    {
        const float threshold = 0.5f;

        // Update the target value for the interpolator.
        _floatValue.targetValue = _responseCurve.Evaluate(inputValue);

        // Invoke the event in direct mode.
        if (!_interpolator.enabled)
            _valueEvent.Invoke(_floatValue.Step());

        // Detect an on-event and invoke the event.
        if (_lastInputValue < threshold && inputValue >= threshold)
            _onEvent.Invoke();

        // Detect an ooff-event and invoke the event.
        if (inputValue < threshold && _lastInputValue >= threshold)
            _offEvent.Invoke();

        _lastInputValue = inputValue;
    }

    #endregion

    #region MonoBehaviour functions
    void OnKnobUpdate(MidiChannel channel, int knobNumber, float knobValue)
    {
        // Do nothing if the setting doesn't match.
        if (_channel != MidiChannel.All && channel != _channel) return;
        if (_knobNumber != knobNumber) return;
        // Do the actual process.
        DoKnobUpdate(knobValue);
    }

    void OnEnable()
    {
        RtMidiDriver.knobDelegate += OnKnobUpdate;
    }

    void OnDisable()
    {
        RtMidiDriver.knobDelegate -= OnKnobUpdate;
    }

    void Start()
    {
        _lastInputValue = 0f;
        _floatValue = new FloatInterpolator(
            _responseCurve.Evaluate(_lastInputValue), _interpolator
        );
    }

    void Update()
    {
        if (_interpolator.enabled)
            _valueEvent.Invoke(_floatValue.Step());
    }

    #endregion

#if UNITY_EDITOR

    #region Editor interface

    public float debugInput
    {
        get { return _lastInputValue; }
        set { DoKnobUpdate(value); }
    }

    #endregion

#endif
}
