using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
	public event EventHandler<OnIngredientAddEventArgs> OnIngredientAdd;
	public static event EventHandler OnIngredientAdd_SFX;
	public class OnIngredientAddEventArgs : EventArgs
	{
		public KitchenObjectSO kitchenObjectSO;
	}

	[SerializeField] private List<KitchenObjectSO> validKitchenSOsList;
    private List<KitchenObjectSO> kitchenObjectSOsList;
	private void Awake()
	{
		kitchenObjectSOsList = new List<KitchenObjectSO>();
	}
	public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
	{
		if (!validKitchenSOsList.Contains(kitchenObjectSO)) {
			return false;
		}
		if (kitchenObjectSOsList.Contains(kitchenObjectSO))
		{
			// already has this ingerdient
			return false;
		}
		else
		{
			kitchenObjectSOsList.Add(kitchenObjectSO);
			OnIngredientAdd?.Invoke(this, new OnIngredientAddEventArgs
			{
				kitchenObjectSO = kitchenObjectSO
			});
			OnIngredientAdd_SFX?.Invoke(this, EventArgs.Empty);
			return true;
		}
	}
	public bool CanAddIngredient(KitchenObjectSO kitchenObjectSO)
	{
		if (!validKitchenSOsList.Contains(kitchenObjectSO))
		{
			return false;
		}
		if (kitchenObjectSOsList.Contains(kitchenObjectSO))
		{
			// already has this ingerdient
			return false;
		}
		else
		{
			return true;
		}
	}
	public List<KitchenObjectSO> GetKitchenObjectSOsList()
	{
		return kitchenObjectSOsList;
	}
	public static void ResetStaticEvent()
	{
		OnIngredientAdd_SFX = null;
	}
}
