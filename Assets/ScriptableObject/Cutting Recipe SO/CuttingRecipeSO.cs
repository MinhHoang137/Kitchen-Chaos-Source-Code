using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO input;
    [SerializeField] private KitchenObjectSO output;
    [SerializeField] private int maxCuttingProgress;
    public KitchenObjectSO GetInput() { return input; }
    public KitchenObjectSO GetOutput() { return output; }
    public int MaxCuttingProgress { get { return maxCuttingProgress; } }
}
