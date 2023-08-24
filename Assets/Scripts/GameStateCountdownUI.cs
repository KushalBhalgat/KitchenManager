using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;
    
    private void Start() {
        KitchenGameManager.instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }
    
    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (KitchenGameManager.instance.IsGameOnCountdown() ) {Show();}
        else {Hide();}
    }

    private void Update() {
        if (KitchenGameManager.instance.IsGameOnCountdown()) {
            countDownText.text = (KitchenGameManager.instance.GetCountdownToStartTimer()+1).ToString();
        }
    }

    private void Hide() {gameObject.SetActive(false); }
    private void Show() { gameObject.SetActive(true); }
}
