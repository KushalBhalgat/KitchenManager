using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Player playerScript;
    public const string IS_WALKING = "IsWalking";

    private void Start() {
        animator= GetComponent<Animator>();
        animator.SetBool(IS_WALKING,playerScript.IsWalking());  
        
    }
    private void Update() {
        animator.SetBool(IS_WALKING, playerScript.IsWalking());
    }
}
