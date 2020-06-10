using UnityEngine;
using System.Collections.Generic;
using RtMidi.LowLevel;

public class RtMidiDriver
{
    MidiProbe _inProbe;
    List<MidiInPort> _inPorts;
    MidiProbe _outProbe;
    List<MidiOutPort> _outPorts;
    MidiSettings _settings;

    public RtMidiDriver()
    {
        _inPorts = new List<MidiInPort>();
        _inProbe = new MidiProbe(MidiProbe.Mode.In);
        _outPorts = new List<MidiOutPort>();
        _outProbe = new MidiProbe(MidiProbe.Mode.Out);
        _settings = MidiSettings.GetOrCreateSettings();
        RescanInPorts();
        RescanOutPorts();
    }

    ~RtMidiDriver()
    {
        _inProbe?.Dispose();
        DisposeInPorts();
        DisposeOutPorts();
    }

    #region Event Delegates

    public delegate void NoteOnDelegate(MidiChannel channel, int note, float velocity);
    public delegate void NoteOffDelegate(MidiChannel channel, int note);
    public delegate void KnobDelegate(MidiChannel channel, int knobNumber, float knobValue);

    public NoteOnDelegate _noteOnDelegate { get; set; }
    public NoteOffDelegate _noteOffDelegate { get; set; }
    public KnobDelegate _knobDelegate { get; set; }

    #endregion
    // MIDI event delegates
    public static NoteOnDelegate noteOnDelegate
    {
        get { return Instance._noteOnDelegate; }
        set { Instance._noteOnDelegate = value; }
    }

    public static NoteOffDelegate noteOffDelegate
    {
        get { return Instance._noteOffDelegate; }
        set { Instance._noteOffDelegate = value; }
    }

    public static KnobDelegate knobDelegate
    {
        get { return Instance._knobDelegate; }
        set { Instance._knobDelegate = value; }
    }


    #region Editor Support

#if UNITY_EDITOR

    string _lastMessage;
    public string LastMessage
    {
        get
        {
            if (CheckUpdateInterval()) Update();
            return _lastMessage;
        }
    }

    // Update timer
    const float _updateInterval = 1.0f / 30;
    float _lastUpdateTime;

    bool CheckUpdateInterval()
    {
        var current = Time.realtimeSinceStartup;
        if (current - _lastUpdateTime > _updateInterval || current < _lastUpdateTime)
        {
            _lastUpdateTime = current;
            return true;
        }
        return false;
    }

#endif

    #endregion


    void OnNoteOn(byte channel, byte note, byte velocity)
    {
        var vel = 1.0f / 127 * velocity + 1;
        if (_noteOnDelegate != null)
            _noteOnDelegate((MidiChannel)channel, note, vel - 1);
#if UNITY_EDITOR
        _lastMessage = string.Format("[{0}] On {1} ({2})", channel, note, velocity);
#endif
    }

    void OnNoteOff(byte channel, byte note)
    {
        if (_noteOffDelegate != null)
            _noteOffDelegate((MidiChannel)channel, note);
#if UNITY_EDITOR
        _lastMessage = (string.Format("[{0}] Off {1}", channel, note));
#endif
    }

    void OnControlChange(byte channel, byte number, byte value)
    {
        var level = 1.0f / 127 * value;
        if (_knobDelegate != null)
            _knobDelegate((MidiChannel)channel, number, level);
#if UNITY_EDITOR
        _lastMessage = (string.Format("[{0}] CC {1} ({2})", channel, number, value));
#endif
    }

    public static void SendAllOff(int channel)
    {
        foreach (var port in Instance._outPorts) port?.SendAllOff(channel);
    }
    public static void SendNoteOn(int channel, int note, int velocity)
    {
        foreach (var port in Instance._outPorts) port?.SendNoteOn(channel, note, velocity);
    }
    public static void SendNoteOff(int channel, int note)
    {
        foreach (var port in Instance._outPorts) port?.SendNoteOff(channel, note);
    }

    public static void SendControlChange(int channel, int number, int value)
    {
        foreach (var port in Instance._outPorts) port?.SendControlChange(channel, number, value);
    }

    // Scan and open all the available output ports.
    void RescanInPorts()
    {
        DisposeInPorts();
        for (var i = 0; i < _inProbe.PortCount; i++)
        {
            var name = _inProbe.GetPortName(i);
            _inPorts.Add((name != null && name.Contains(_settings.MidiInFilter)) ? new MidiInPort(i)
            {
                OnNoteOn = OnNoteOn,
                OnNoteOff = OnNoteOff,
                OnControlChange = OnControlChange
            } : null);
        }
    }
    void RescanOutPorts()
    {
        DisposeOutPorts();
        for (var i = 0; i < _outProbe.PortCount; i++)
        {
            var name = _outProbe.GetPortName(i);
            _outPorts.Add(( name !=null && name.Contains(_settings.MidiOutFilter)) ? new MidiOutPort(i) : null);

        }
    }

    // Close and release all the opened ports.
    void DisposeInPorts()
    {
        foreach (var p in _inPorts) p?.Dispose();
        _inPorts.Clear();
    }
    void DisposeOutPorts()
    {
        foreach (var p in _outPorts) p?.Dispose();
        _outPorts.Clear();
    }

    void Update()
    {
        // Rescan when the number of ports changed.
        if (_outPorts.Count != _outProbe.PortCount)
            RescanOutPorts();

        // Rescan when the number of ports changed.
        if (_inPorts.Count != _inProbe.PortCount)
            RescanInPorts();

        // Process queued messages in the opened ports.
        foreach (var p in _inPorts) p?.ProcessMessages();
    }

    #region Singleton Class Instance

    static RtMidiDriver _instance;

    public static RtMidiDriver Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RtMidiDriver();
                if (Application.isPlaying)
                    RtMidiStateUpdater.CreateGameObject(
                        new RtMidiStateUpdater.Callback(_instance.Update));
            }
            return _instance;
        }
    }

    #endregion

}
