using UnityEngine;

public class RtMidiStateUpdater : MonoBehaviour
{
    public delegate void Callback();

    public static void CreateGameObject(Callback callback)
    {
        var go = new GameObject("Rt MIDI Updater");

        GameObject.DontDestroyOnLoad(go);
        go.hideFlags = HideFlags.HideInHierarchy;

        var updater = go.AddComponent<RtMidiStateUpdater>();
        updater._callback = callback;
    }

    Callback _callback;

    void Update()
    {
        _callback();
    }
}

