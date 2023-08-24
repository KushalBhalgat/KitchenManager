using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent{

    public static event EventHandler OnAnyObjectPlacedHere;

    [SerializeField]private Transform counterTopPoint;
    protected KitchenObject kitchenObject;
    public static void ResetStaticData() {
        OnAnyObjectPlacedHere = null;
    }

    public virtual void Interact(Player player){Debug.LogError("BaseCounter.Interact()");}
    public virtual void InteractAlternate(Player player){}
    
    
    public Transform GetKitchenObjectFollowTransform() { return counterTopPoint; }
    public void SetKitchenObject(KitchenObject kitchenObject) { 
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) { 
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject() { return this.kitchenObject; }
    public void ClearKitchenObject() { this.kitchenObject = null; }
    public bool HasKitchenObject() { return kitchenObject != null; }
}
