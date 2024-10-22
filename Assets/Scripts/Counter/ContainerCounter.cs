using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter, IInteractable
{
	[SerializeField] private KitchenObjectSO kitchenObjectSO;
	public event EventHandler OnUpdateInteract;
	public event EventHandler OnUpdateInteractAlternate;
	public event EventHandler OnPlayerGrabbedObject;
	private void Start()
	{
		OnUpdateInteractAlternate?.Invoke(this, EventArgs.Empty);
	}
	public override void Interact(Player player)
	{
		if (!player.HasKitchenObject())
		{
			KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
			OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
			OnUpdateInteract?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			Debug.Log("Player already has KitchenObject");
		}
	}
	public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }

	public bool CanInteract(Player player, out string action)
	{
		action = IInteractable.GRAB;
		if (!player.HasKitchenObject())
		{
			action += GetKitchenObjectSO().GetObjectName();
			return true;
		}
		return false;
	}

	public bool CanInteractAlternate(Player player, out string action)
	{
		action = null;
		return false;
	}
}
