using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter: BaseCounter, IInteractable
{
	public event EventHandler OnUpdateInteract;

	public event EventHandler OnUpdateInteractAlternate;
	private void Start()
	{
		OnUpdateInteractAlternate?.Invoke(this, EventArgs.Empty);
	}

	public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // give ingredient to player
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // player is not holding a plate, but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // counter is holding the plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
							// give ingredient to counter
							player.GetKitchenObject().DestroySelf();
						}
                    }
                }
            }
            else
            {
                // Give KitchenObject to player
                GetKitchenObject().SetKitchenObjectParent(player);
			}
        }
        else
        {
			if (player.HasKitchenObject())
			{
				// put kitchen object on the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
			}
			else
			{
				// Do nothing
			}
		}
		OnUpdateInteract?.Invoke(this, EventArgs.Empty);
    }
	

	public bool CanInteract(Player player, out string action)
	{
		action = "";
		if (HasKitchenObject())
		{
			if (player.HasKitchenObject())
			{
				// Player is carrying something
				if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
				{
					//Player is holding a plate
					action = IInteractable.GRAB + GetKitchenObjectSO().GetObjectName();
					return true;
				}
				else
				{
					// player is not holding a plate, but something else
					if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
					{
						// counter is holding the plate
						if (plateKitchenObject.CanAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
						{
							// give ingredient to counter
							action = IInteractable.PLACE + player.GetKitchenObject().GetKitchenObjectSO().GetObjectName();
							return true;
						}
					}
				}
			}
			else
			{
				// Give KitchenObject to player
				action = IInteractable.GRAB + GetKitchenObjectSO().GetObjectName();
				return true;
			}
		}
		else
		{
			if (player.HasKitchenObject())
			{
				// put kitchen object on the counter
				action = IInteractable.PLACE + player.GetKitchenObject().GetKitchenObjectSO().GetObjectName();
				return true;
			}
			else
			{
				// Do nothing
			}
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
		return GetKitchenObject().GetKitchenObjectSO();
	}
}
