using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter{

    



    [SerializeField]private KitchenObjectSO kitchenObjectSO;
    

    public override void Interact(Player player) {
        if (!HasKitchenObject() && player.HasKitchenObject()) {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
        else if (HasKitchenObject() && !player.HasKitchenObject()) {
            this.GetKitchenObject().SetKitchenObjectParent(player);
        }
        else if (HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                GetKitchenObject().DestroySelf();
            }
            //plateKitchenObject.OnIngredientAdded?.Invoke(this, new plateKitchenObject.OnIngredientAddedEventArgs{kitchenObjectSO= GetKitchenObject().GetKitchenObjectSO() });
            
        }
        else if(HasKitchenObject() && GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObjectOnCounter)){
            if (plateKitchenObjectOnCounter.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                player.GetKitchenObject().DestroySelf();
            }
            //Ayo

            /*
            PlateKitchenObject plateKitchenObject = (PlateKitchenObject)GetKitchenObject();
            if (plateKitchenObject.IsValidKitchenObject(player.GetKitchenObject().GetKitchenObjectSO())) {
                KitchenObjectSO inHandKitchenObjectSO = player.GetKitchenObject().GetKitchenObjectSO();
                player.GetKitchenObject().DestroySelf();
                GetKitchenObject().SetKitchenObjectParent(player);
                if(plateKitchenObject.GetPlateKitchenObjectSOListCount() < 1) { 
                    plateKitchenObject.AddKitchenObjectOntoPlate(inHandKitchenObjectSO);
                }
            }
            */
        }
    }


    public override void InteractAlternate(Player player) {
        
    }
}
