using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent{

    public static Player Instance { get; private set; }


    public event EventHandler OnPickedSomething;

    [SerializeField] private float playerSpeed = 7f;
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private GameInput gameInput; 
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private bool canMove;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    
    private bool isWalking;
    private Vector2 inputVector;
    private KitchenObject kitchenObject;
    private BaseCounter selectedCounter;



    
    private float playerHeight=2f;
    private float playerRadius= 0.7f;

    private Vector3 lastInteractDirection;

    //private float playerSize = 0.7f;
    public event EventHandler<OnSelectedCounterEventArgs>OnSelectedCounterChanged;


    private void Awake() {
        if (Instance != null) { Debug.LogError("More than one player instance"); }
        Instance = this;
    }
    public class OnSelectedCounterEventArgs: EventArgs { 
        public BaseCounter selectedCounter;
    };

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if (selectedCounter != null && KitchenGameManager.instance.IsGamePlaying()) { selectedCounter.InteractAlternate(this);}
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (selectedCounter != null && KitchenGameManager.instance.IsGamePlaying()) { selectedCounter.Interact(this);}
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }


    private void HandleInteractions() {

        inputVector = gameInput.GetMovementVectorNormalized();
        float interactDistance = 2f;
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero) { lastInteractDirection = moveDir; }
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, counterLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if (baseCounter != selectedCounter) { selectedCounter = baseCounter; 
                    //Debug.Log(selectedCounter);
                    SetSelectedCounter(selectedCounter);
                }
            }
            else { selectedCounter = null; SetSelectedCounter(selectedCounter); }
        }
        else { selectedCounter = null; SetSelectedCounter(selectedCounter); }

    }

    public bool IsWalking(){return isWalking;}


    private void SetSelectedCounter(BaseCounter selectedCounter) {
         OnSelectedCounterChanged?.Invoke(this,new OnSelectedCounterEventArgs{selectedCounter=selectedCounter});
    }

    private void HandleMovement() {
        float moveDistance = playerSpeed * Time.deltaTime;
        inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        isWalking = moveDir != Vector3.zero;

        canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove) {
            //Attempt to move on X axis
            Vector3 moveDirX = Vector3.right * moveDir.x;
            canMove = moveDir.x!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove) { moveDir = moveDirX; }
            else {
                //Else attempt to move on Z axis
                Vector3 moveDirZ = Vector3.forward * moveDir.z;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) { moveDir = moveDirZ; }
            }
        }

        if (canMove) { transform.position += moveDir * moveDistance; }
        if (moveDir != Vector3.zero){ 
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }

    public Transform GetKitchenObjectFollowTransform() { return kitchenObjectHoldPoint; }
    public void SetKitchenObject(KitchenObject kitchenObject) { 
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) { 
            OnPickedSomething?.Invoke(this, EventArgs.Empty); 
        }
    }
    public KitchenObject GetKitchenObject() { return this.kitchenObject; }
    public void ClearKitchenObject() { this.kitchenObject = null; }
    public bool HasKitchenObject() { return kitchenObject != null; }
}
