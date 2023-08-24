using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }


    private List<KitchenObjectSO> plateKitchenObjectSOList;
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSO;    

    private void Awake() {
        plateKitchenObjectSOList = new List<KitchenObjectSO>();
        

    }

    public void AddKitchenObjectOntoPlate(KitchenObjectSO kitchenObjectSO) {
        if (IsValidKitchenObject(kitchenObjectSO)) {  
            plateKitchenObjectSOList.Add(kitchenObjectSO);
        }

    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {
        if (plateKitchenObjectSOList.Contains(kitchenObjectSO) || !IsValidKitchenObject(kitchenObjectSO)) { return false; }
        plateKitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenObjectSO = kitchenObjectSO }); ;
        return true;
    }

    public bool IsValidKitchenObject(KitchenObjectSO kitchenObjectSO){ 
        foreach(KitchenObjectSO listKitchenObjectSO in validKitchenObjectSO) {
            if (listKitchenObjectSO == kitchenObjectSO) {
                return true;
            }
        }
        return false;
    }

    public List<KitchenObjectSO> GetPlateKitchenObjectSOList() {return plateKitchenObjectSOList;}
    
    
    
    /*
    private void Update() {
        
        if (GetPlateKitchenObjectSOListCount() == 1 && spawnedIngredientsGameObjectList.Count == 0) {
            spawnedIngredientsGameObjectList.Add(Instantiate(plateKitchenObjectSOList[0].prefab, plateTopPoint).gameObject);
        }
        if (GetPlateKitchenObjectSOListCount() == 0 && spawnedIngredientsGameObjectList.Count==1) {
            GameObject onPlateIngredientToBeDropped = spawnedIngredientsGameObjectList[0];
            spawnedIngredientsGameObjectList.Remove(onPlateIngredientToBeDropped);
            Destroy(onPlateIngredientToBeDropped);
        }
        
    }
        */
}
