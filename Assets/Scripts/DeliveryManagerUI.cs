using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;
    private void Awake() {
        recipeTemplate.gameObject.SetActive(false);
        
    }
    private void Start() {
        DeliveryManager.instance.OnRecipeSpawned += DeliverManager_OnRecipeSpawned;
        DeliveryManager.instance.OnRecipeCompleted += DeliverManager_OnRecipeCompleted;
        UpdateVisual();
    }

    private void DeliverManager_OnRecipeSpawned(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void DeliverManager_OnRecipeCompleted(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach(Transform child in container) {
            if (child !=recipeTemplate) {
                Destroy(child.gameObject);
            }
        }

        foreach(RecipeSO waitingRecipeSO in DeliveryManager.instance.GetWaitingRecipeSOList()) {
            Transform recipeTransform=Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(waitingRecipeSO);
        };
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
    private void Show() {
        gameObject.SetActive(true);
    }
}
