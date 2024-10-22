using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
	[SerializeField] private KitchenObjectSO input;
	[SerializeField] private KitchenObjectSO output;
	[SerializeField] private float maxFryingTimer;
	public KitchenObjectSO GetInput() { return input; }
	public KitchenObjectSO GetOutput() { return output; }
	public float GetMaxFryingTimer() { return maxFryingTimer; }
}
