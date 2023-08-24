using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        UpdateList();   
    }


    private void UpdateList() {
        foreach (Transform child in transform) {
            if (child != iconTemplate) { Destroy(child.gameObject); }
        }

        foreach(KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetPlateKitchenObjectSOList()) {
            if (plateKitchenObject.IsValidKitchenObject(kitchenObjectSO)) { 
                Transform iconTransform=Instantiate(iconTemplate, transform);
                iconTransform.gameObject.SetActive(true);
                iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
            }
        }
    }
}
