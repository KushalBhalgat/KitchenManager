using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    //SO stands for Scriptable Objects
    public Transform prefab;
    public Sprite sprite;
    public string objectName;

}
