using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject particlesGameObject;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        if(e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying) {ShowVisuals();} 
        else { HideVisuals();}
    }

    private void HideVisuals() {
        particlesGameObject.SetActive(false);
        stoveOnGameObject.SetActive(false);
    }
    private void ShowVisuals() {
        particlesGameObject.SetActive(true);
        stoveOnGameObject.SetActive(true);
    }
}
