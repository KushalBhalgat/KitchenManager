using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{


    public static event EventHandler OnAnyCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCuttingInitiated;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    
    public int cuttingProgress;

    private void Start() {
        cuttingProgress = 0;
    }

    new public static void ResetStaticData() {
        OnAnyCut = null;
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject() && player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            if (!HasRecipeWithInput(kitchenObject.GetKitchenObjectSO())) { return; }
            cuttingProgress = 0;
            CuttingRecipeSO cuttingRecipeSO=GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = (float)cuttingProgress/ cuttingRecipeSO.cuttingProgressMax });

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
    }

    public override void InteractAlternate(Player player) {
        if (GetKitchenObject() == null) {return;}
        KitchenObject kitchenObject = GetKitchenObject();
        if (!HasRecipeWithInput(kitchenObject.GetKitchenObjectSO())) { return; }
        if (HasKitchenObject() && HasRecipeWithInput(kitchenObject.GetKitchenObjectSO())) {
            OnCuttingInitiated?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            cuttingProgress++;

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = (float)cuttingProgress/ cuttingRecipeSO.cuttingProgressMax });

            //Debug.Log(cuttingProgress);
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax){
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(kitchenObject.GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        return GetCuttingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        KitchenObjectSO outputKitchenObjectSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO).output;
        if (outputKitchenObjectSO != null) { return outputKitchenObjectSO; }
        else { Debug.LogError("GetOutputForInput() cant find input KitchenObjectSO in the array"); 
            return null; 
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) { return cuttingRecipeSO; }
        }
        //Debug.LogError("GetCuttingRecipeSOWithInput() cant find input KitchenObjectSO in the array");
        return null;
    }
}
