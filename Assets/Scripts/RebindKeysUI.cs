using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static OptionsMenuUI;

public class RebindKeysUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    private void Awake() {
        backButton.onClick.AddListener(() => {
            Hide();
            OptionsMenuUI.Instance.Show();
        });
    }
    void Start()
    {
        Hide();
        OptionsMenuUI.Instance.OnKeyBindSelected += Instance_OnKeyBindSelected;
    }

    private void Instance_OnKeyBindSelected(object sender, System.EventArgs e) {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}
