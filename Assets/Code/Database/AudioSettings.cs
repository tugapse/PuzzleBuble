using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "Settings/Audio Settings", order = 0)]
public class AudioSettings : ScriptableObject
{
    public float BGMVolume = 1;
    public float SFXVolume = 1;
}