using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CuttingCounter : BaseCounter, IHasProgress, IInteractable
{
	public event EventHandler<IHasProgress.OnProgressChangedEvenArgs> OnProgressChanged;
	public event EventHandler OnCut;

	public static event EventHandler OnAnyCut;
	public event EventHandler OnUpdateInteract;
	public event EventHandler OnUpdateInteractAlternate;

	[SerializeField] private CuttingRecipeSO[] cuttingKitchenObjectSOArray;
	private int cuttingProgress;
	new public static void ResetStaticEvent()
	{
		OnAnyCut = null;
	}
	public override void Interact(Player player)
	{
		if (HasKitchenObject())
		{
			// There is Kitchen Object
			if (player.HasKitchenObject())
			{
				if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
				{
					//Player is holding a plate
					if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
					{
						// give ingredient to player
						GetKitchenObject().DestroySelf();
					}
				}
			}
			else
			{
				// Give KitchenObject to player
				GetKitchenObject().SetKitchenObjectParent(player);
				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
				{
					progressNormalized = 0
				});
			}
		}
		else
		{
			// There is no Kitchen Object
			if (player.HasKitchenObject())
			{
				if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
				{
					// put kitchen object on the counter, if player.KitchenObject is cuttable
					player.GetKitchenObject().SetKitchenObjectParent(this);
					CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOwithInput(GetKitchenObject().GetKitchenObjectSO());
					cuttingProgress = 0;
					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
					{
						progressNormalized = (float)cuttingProgress / cuttingRecipeSO.MaxCuttingProgress
					});
				}
			}
			else
			{
				// Do nothing
			}
		}
		OnUpdateInteract?.Invoke(this, EventArgs.Empty);
	}
	public override void InteractAlternate(Player player)
	{
		if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
		{
			if (!player.HasKitchenObject())
			{
				CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOwithInput(GetKitchenObject().GetKitchenObjectSO());
				cuttingProgress++;
				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
				{
					progressNormalized = (float)cuttingProgress / cuttingRecipeSO.MaxCuttingProgress
				});
				OnCut?.Invoke(this, EventArgs.Empty);
				OnAnyCut?.Invoke(this, EventArgs.Empty);
				if (cuttingProgress >= cuttingRecipeSO.MaxCuttingProgress)
				{
					KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
					GetKitchenObject().DestroySelf();
					KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
					OnUpdateInteractAlternate?.Invoke(this, EventArgs.Empty);
				}
			}
		}
	}
	private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOwithInput(inputKitchenObjectSO);
		return cuttingRecipeSO != null;
	}
	private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
	{
		CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOwithInput(inputKitchenObjectSO);
		if (cuttingRecipeSO != null)
		{
			return cuttingRecipeSO.GetOutput();
		}
		return null;
	}
	private CuttingRecipeSO GetCuttingRecipeSOwithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		foreach (var cuttingKitchenObjectSO in cuttingKitchenObjectSOArray)
		{
			if (inputKitchenObjectSO == cuttingKitchenObjectSO.GetInput())
			{
				return cuttingKitchenObjectSO;
			}
		}
		return null;
	}

	public bool CanInteract(Player player, out string action)
	{
		action = "";
		if (HasKitchenObject())
		{
			// There is Kitchen Object
			if (player.HasKitchenObject())
			{
				if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
				{
					//Player is holding a plate
					if (plateKitchenObject.CanAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
					{
						// give ingredient to player
						action = IInteractable.GRAB + GetKitchenObjectSO().GetObjectName();
						return true;
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
			// There is no Kitchen Object
			if (player.HasKitchenObject())
			{
				if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
				{
					// put kitchen object on the counter, if player.KitchenObject is cuttable
					action = IInteractable.PLACE + player.GetKitchenObject().GetKitchenObjectSO().GetObjectName();
					return true;
				}
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
		action = "Cut ";
		if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
		{
			if (!player.HasKitchenObject())
			{
				action += GetKitchenObjectSO().GetObjectName();
				return true;
			}
		}
		return false;
	}

	public KitchenObjectSO GetKitchenObjectSO()
	{
		if (GetKitchenObject() != null)
		{
			return GetKitchenObject().GetKitchenObjectSO();
		}
		return null;
	}
}
