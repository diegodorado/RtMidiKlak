using UnityEditor;
using UnityEngine;

public class MidiSettings : ScriptableObject
{
    public const string k_settingsPath = "Assets/RtMidiSettings.asset";
    [SerializeField]
    public string MidiInFilter;

    [SerializeField]
    public string MidiOutFilter;

    public static MidiSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<MidiSettings>(k_settingsPath);
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<MidiSettings>();
            AssetDatabase.CreateAsset(settings, k_settingsPath);
            AssetDatabase.SaveAssets();
        }
        return settings;
    }
}