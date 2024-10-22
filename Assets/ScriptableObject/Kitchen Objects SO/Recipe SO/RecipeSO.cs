using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private List<KitchenObjectSO> kitchenObjectSOList;
    [SerializeField] private string recipeName;
    [SerializeField] private float waitTimeMax;
    public List<KitchenObjectSO> KitchenObjectSOList
    {
        get { return kitchenObjectSOList; }
    }
    public string RecipeName { get { return recipeName; } }
    public float WaitTimeMax { get { return waitTimeMax; } }
}
