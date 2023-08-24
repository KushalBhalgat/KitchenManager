using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    
    AudioSource musicAudioSource;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        musicAudioSource=GetComponent<AudioSource>();
        OptionsMenuUI.Instance.OnOptionsUpdated += Instance_OnOptionsUpdated;
    }

    //public void UpdateMusicLevels(float musicVolume) { this.musicAudioSource.volume = musicVolume; }


    private void Instance_OnOptionsUpdated(object sender, OptionsMenuUI.OnOptionsUpdatedEventArgs e) {
        musicAudioSource.volume = e.musicVolume;
    }

    public float GetMusicVolume() {
        return musicAudioSource.volume;
    }
}
