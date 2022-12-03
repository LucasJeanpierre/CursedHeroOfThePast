using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsScript : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;

    public void SetMusicVolume(float volume)
    {
        musicMixerGroup.audioMixer.SetFloat("VolumeMusicParam", volume);
    }

    public void SetSFXVolume(float volume)
    {
        soundEffectsMixerGroup.audioMixer.SetFloat("VolumeFXParam", volume);
    }
}
