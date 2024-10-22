using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter, IInteractable
{
	public event EventHandler OnUpdateInteract;
	public event EventHandler OnUpdateInteractAlternate;
	private void Start()
	{
		OnUpdateInteractAlternate?.Invoke(this, EventArgs.Empty);
	}

	public bool CanInteract(Player player, out string action)
	{
		action = "Deliver ";
		if (player.HasKitchenObject())
		{
			if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
			{
				return true;
			}
		}
		return false;
	}

	public bool CanInteractAlternate(Player player, out string action)
	{
		action= null;
		return false;
	}

	public KitchenObjectSO GetKitchenObjectSO()
	{
		return null;
	}

	public override void Interact(Player player)
	{
		if (player.HasKitchenObject())
		{
			if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
				DeliveryManager.Instance.DeliverRecipe(plateKitchenObject, this.transform.position);
				player.GetKitchenObject().DestroySelf();
			}
		}
		OnUpdateInteract?.Invoke(this, EventArgs.Empty);
	}
}
