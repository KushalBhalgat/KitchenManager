using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    private void Awake() {
        mainMenuButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.MainMenuScene); });
        mainMenuButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private void Start() {
        KitchenGameManager.instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (KitchenGameManager.instance.IsGameOver()) { 
            Show();
            recipesDeliveredText.text = DeliveryManager.instance.GetNumberOfRecipesDelivered().ToString();
        }
        else {
            Hide();
        }
    }

    private void Hide() { gameObject.SetActive(false); }
    private void Show() { gameObject.SetActive(true); }
}
