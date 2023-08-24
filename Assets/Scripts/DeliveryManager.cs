using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManager : MonoBehaviour {

    
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;

    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer = 4f;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    public int recipesDelivered=0;

    public static DeliveryManager instance { get; private set; }

    private void Awake() {
        waitingRecipeSOList = new List<RecipeSO>();
        instance = this;
    }

    private void Update() {
        if (KitchenGameManager.instance.IsGamePlaying()) {
            spawnRecipeTimer -= Time.deltaTime;
            if (spawnRecipeTimer <= 0f) {
                spawnRecipeTimer = spawnRecipeTimerMax;
                if (waitingRecipeSOList.Count < waitingRecipesMax) {
                    RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                    waitingRecipeSOList.Add(waitingRecipeSO);
                    OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }



    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        List<KitchenObjectSO> plateKitchenObjectList = plateKitchenObject.GetPlateKitchenObjectSOList();
        foreach (RecipeSO waitingRecipeSO in waitingRecipeSOList) {
            if (plateKitchenObjectList.Count == waitingRecipeSO.kitchenObjectSOList.Count) {
                bool plateContentsMatchRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    bool ingredientsFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetPlateKitchenObjectSOList()) {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                            ingredientsFound = true; break;
                        };
                    }
                    if (!ingredientsFound) {
                        plateContentsMatchRecipe = false;
                    }
                }
                if (plateContentsMatchRecipe) {
                    waitingRecipeSOList.Remove(waitingRecipeSO);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    recipesDelivered++;
                    return; 
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public int GetNumberOfRecipesDelivered() { return recipesDelivered; }
    public List<RecipeSO> GetWaitingRecipeSOList(){ return waitingRecipeSOList; }
}
