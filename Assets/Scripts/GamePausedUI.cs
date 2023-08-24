using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;

    public event EventHandler OnClickOptionsInPauseMenu;

    public static GamePausedUI instance { get; private set; }   
    private void Awake() {
        instance = this;
        mainMenuButton.onClick.AddListener(() => { Time.timeScale = 1f; Loader.Load(Loader.Scene.MainMenuScene); });
        resumeButton.onClick.AddListener(() => { Time.timeScale = 1f; Hide(); });
        optionsButton.onClick.AddListener(() => { OnClickOptionsInPauseMenu?.Invoke(this, EventArgs.Empty); Hide(); });
    }


    void Start()
    {
        KitchenGameManager.instance.OnGamePaused += GameManager_OnGamePaused;
        KitchenGameManager.instance.OnGameUnpaused += GameManager_OnGameUnpaused; ;
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e) {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e) {
        Show();
    }

    public void Show() {
        gameObject.SetActive(true);
    }
    public void Hide() {
        gameObject.SetActive(false);
    }
}
