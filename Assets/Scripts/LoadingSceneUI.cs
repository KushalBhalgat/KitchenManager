using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    private float loadingTimer = 0f;
    private float loadingTimerMax = 1.5f;


    private void Update() {
        progressBar.fillAmount = loadingTimer / loadingTimerMax;
        if (loadingTimer < loadingTimerMax) {
            loadingTimer += Time.deltaTime;
        }
    }
}
