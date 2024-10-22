using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{
    [SerializeField] private KitchenObjectSO input;
    [SerializeField] private KitchenObjectSO output;
    [SerializeField] private float maxBurningTimer;
    public KitchenObjectSO GetInput() {  return input; }
    public KitchenObjectSO GetOutput() { return output; }
    public float MaxBurningTimer { get { return maxBurningTimer; } }
}
