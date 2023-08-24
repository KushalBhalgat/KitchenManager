using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter,IHasProgress {

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnStateChangedEventArgs :EventArgs{ public State state; }

    public enum State { Idle,Frying,Fried,Burned};


    private float Timer;
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private State currentState;


    private void Start() {
        currentState = State.Idle;    
        fryingTimer = 0f;
        burningTimer= 0f;
    }

    private void Update() {
        if (HasKitchenObject()) {
            kitchenObject=GetKitchenObject();
            switch (currentState) {
                case State.Idle:
                    fryingTimer = 0f;
                    burningTimer = 0f;
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        KitchenObjectSO outputKitchenObjectSO = GetFryingOutputWithInput(kitchenObject.GetKitchenObjectSO());
                        kitchenObject.DestroySelf();
                        kitchenObject = KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
                        currentState = State.Fried;
                        fryingTimer = 0f;
                        burningTimer= 0f;
                        burningRecipeSO = GetBurningRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
                    }
                    break;
                case State.Fried:
                    burningTimer+= Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = burningTimer / burningRecipeSO.burningTimerMax });

                    if (burningTimer > burningRecipeSO.burningTimerMax) {
                        KitchenObjectSO outputKitchenObjectSO = burningRecipeSO.output;
                        kitchenObject.DestroySelf();
                        kitchenObject=KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
                        burningTimer = 0f;
                        currentState = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
                    }
                    break;
                case State.Burned:
                    fryingTimer = 0f;burningTimer = 0f;
                    break;
            }
        }
    }

    public override void Interact(Player player) {
        
        if (!HasKitchenObject() && player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
        else if (HasKitchenObject() && !player.HasKitchenObject()) {
            GetKitchenObject().SetKitchenObjectParent(player);
            currentState = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });

        }
        else if (HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                GetKitchenObject().DestroySelf();
                currentState = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
            }
            //plateKitchenObject.OnIngredientAdded?.Invoke(this, new plateKitchenObject.OnIngredientAddedEventArgs{kitchenObjectSO= GetKitchenObject().GetKitchenObjectSO() });
        }
    }

    public override void InteractAlternate(Player player) {
        if (GetKitchenObject() == null) { return; }
        fryingTimer = 0f;
        KitchenObject kitchenObject = GetKitchenObject();
        if (!HasRecipeWithInput(kitchenObject.GetKitchenObjectSO())) { return; }
        if (HasKitchenObject() && HasRecipeWithInput(kitchenObject.GetKitchenObjectSO())) {
            fryingRecipeSO=GetFryingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
            currentState = State.Frying;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
            fryingTimer = 0;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
        }

    }
    private KitchenObjectSO GetFryingOutputWithInput(KitchenObjectSO kitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSO(kitchenObjectSO);
        if (HasRecipeWithInput(kitchenObjectSO)) {
            return fryingRecipeSO.output;
        }
        return null;
    }
    private FryingRecipeSO GetFryingRecipeSO(KitchenObjectSO kitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == kitchenObjectSO) { return fryingRecipeSO; }
        }
        return null;
    }
    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO) {
        return GetFryingRecipeSO(kitchenObjectSO)!=null;
    }
    
    private BurningRecipeSO GetBurningRecipeSO(KitchenObjectSO kitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == kitchenObjectSO) { return burningRecipeSO; }
        }
        return null;
    }
    
}
