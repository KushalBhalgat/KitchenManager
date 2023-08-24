using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button backButton;
    [SerializeField] private Button keyBindButton;
    public event EventHandler<OnOptionsUpdatedEventArgs> OnOptionsUpdated;
    public event EventHandler OnKeyBindSelected;
    public static OptionsMenuUI Instance;
    public class OnOptionsUpdatedEventArgs:EventArgs{
        public float musicVolume;
        public float sfxVolume;
    }

    private void Awake() {
        Instance = this;
        backButton.onClick.AddListener(() => { 
            Hide();
            //SoundManager.Instance.UpdateSoundLevels(sfxSlider.value);
            //MusicManager.instance.UpdateMusicLevels(musicSlider.value);
            OnOptionsUpdated?.Invoke(this, new OnOptionsUpdatedEventArgs { musicVolume = musicSlider.value, sfxVolume = sfxSlider.value });
            GamePausedUI.instance.Show();
        });
        keyBindButton.onClick.AddListener(() => {
            Hide();
            OnKeyBindSelected?.Invoke(this, EventArgs.Empty);
        });
    }
    void Start()
    {
        GamePausedUI.instance.OnClickOptionsInPauseMenu += Instance_OnClickOptionsInPauseMenu;   
        Hide();
    }

    private void Instance_OnClickOptionsInPauseMenu(object sender, System.EventArgs e) {
        sfxSlider.value = SoundManager.Instance.sfxVolume;
        musicSlider.value = MusicManager.instance.GetMusicVolume();
        Show();
    }

    public void Show() {
        gameObject.SetActive(true);
    }
    public void Hide() {
        gameObject.SetActive(false);
    }
}
