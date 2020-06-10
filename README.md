RtMidiKlak
========

*RtMidiKlak* is another extension for [Klak][Klak], that provides functionality for receiving and sending MIDI messages from physical/virtual MIDI devices.

I made *RtMidiKlak* after [keijiro][keijiro]'s repositories to overcome some troulbes I had with [MidiKlak][MidiKlak], and to provide MIDI output inside [Klak][Klak].
Many thanks to [keijiro][keijiro] for the repositories he made available to us.


System Requirements
-------------------

- Unity 2019.1 or later
- Windows, macOS or Linux
- Only supports 64-bit architecture

*RtMidiKlak* has dependency to the following packages. Please import them before
installing *RtMidiKlak*.

- [Klak][Klak]
- [jp.keijiro.rtmidi][jp.keijiro.rtmidi]


How To Install
--------------

Since this package depends on [jp.keijiro.rtmidi][jp.keijiro.rtmidi] , you need to add it through the [scoped registry] feature to resolve package dependencies. 
Please add the following sections to the manifest file
(Packages/manifest.json).


To the `scopedRegistries` section:

```
{
  "name": "Keijiro",
  "url": "https://registry.npmjs.com",
  "scopes": [ "jp.keijiro" ]
}
```

To the `dependencies` section:

```
"jp.keijiro.rtmidi": "1.0.3"
```

After changes, the manifest file should look like below:

```
{
  "scopedRegistries": [
    {
      "name": "Keijiro",
      "url": "https://registry.npmjs.com",
      "scopes": [ "jp.keijiro" ]
    }
  ],
  "dependencies": {
    "jp.keijiro.rtmidi": "1.0.3",
    ...
```


How To Use It
-------------

*RtMidiKlak* provides four components -- **RtNoteInput** , **RtKnobInput** , **RtNoteOut** and **RtCcOut**.

- **RtNoteInput** - receives MIDI note messages and invokes Unity events with input values (note number and velocity).
- **RtKnobInput** - receives MIDI CC (control change) messages and invokes Unity events with a single float value.
- **RtNoteOut** - sends MIDI note messages out.
- **RtKnobInput** - sends MIDI CC (control change) messages out.


Also see [the troubleshooting topics][Troubleshoot] if you meet any problem
in using MIDI devices.



[keijiro]: https://github.com/keijiro
[Klak]: https://github.com/keijiro/Klak
[MidiJack]: https://github.com/keijiro/MidiJack
[MidiKlak]: https://github.com/keijiro/MidiKlak
[jp.keijiro.rtmidi]: https://github.com/keijiro/jp.keijiro.rtmidi
[scoped registry]: https://docs.unity3d.com/Manual/upm-scoped.html
