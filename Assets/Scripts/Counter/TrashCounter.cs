using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCounter : BaseCounter, IInteractable
{
	public static event EventHandler OnAnyTrashed;
	public event EventHandler OnUpdateInteract;
	public event EventHandler OnUpdateInteractAlternate;

	private void Start()
	{
		OnUpdateInteractAlternate?.Invoke(this, EventArgs.Empty);
	}
	public override void Interact(Player player)
	{
		if (player.HasKitchenObject())
		{
			player.GetKitchenObject().DestroySelf();
			OnAnyTrashed?.Invoke(this, EventArgs.Empty);
			OnUpdateInteract?.Invoke(this, EventArgs.Empty);
		}
	}
	new public static void ResetStaticEvent()
	{
		OnAnyTrashed = null;
	}

	public bool CanInteract(Player player, out string action)
	{
		action = "Trash ";
		if (player.HasKitchenObject())
		{
			action += player.GetKitchenObject().GetKitchenObjectSO().GetObjectName();
			return true;
		}
		return false;
	}


	public bool CanInteractAlternate(Player player, out string action)
	{
		action = null;
		return false;
	}

	public KitchenObjectSO GetKitchenObjectSO()
	{
		return null;
	}
}
