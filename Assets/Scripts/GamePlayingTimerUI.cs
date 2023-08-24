using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingTimerUI : MonoBehaviour
{
    [SerializeField]private Image timerImage;
    private void Update() {
        if (KitchenGameManager.instance.IsGamePlaying()) { Show(); }
        else { Hide(); }
        timerImage.fillAmount = KitchenGameManager.instance.GetGamePlayingTimerNormalized();
    }


    private void Show() {
        timerImage.gameObject.SetActive(true);
    }
    private void Hide() {
        timerImage.gameObject.SetActive(false);
    }
}
