using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress, IInteractable
{
	public event EventHandler<OnStateChangeEventArgs> OnStateChange;
	public event EventHandler<IHasProgress.OnProgressChangedEvenArgs> OnProgressChanged;
	public event EventHandler OnUpdateInteract;
	public event EventHandler OnUpdateInteractAlternate;

	public class OnStateChangeEventArgs : EventArgs
	{
		public State state;
	}
	public enum State
	{
		Idle,
		Frying,
		Fried,
		Burned
	}
	private State state;
	[SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
	private FryingRecipeSO fryingRecipeSO;
	[SerializeField] BurningRecipeSO[] burningRecipeSOArray;
	private BurningRecipeSO burningRecipeSO;
    private float fryingTimer;
	private float burningTimer;
	private void Start()
	{
		state = State.Idle;
		OnStateChange?.Invoke(this, new OnStateChangeEventArgs
		{
			state = this.state
		});
		OnUpdateInteractAlternate?.Invoke(this, EventArgs.Empty);
	}
	private void Update()
	{
		if (HasKitchenObject())
		{
			switch (state)
			{
				case State.Idle:
					break;
				case State.Frying:
					fryingTimer += Time.deltaTime;
					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
					{
						progressNormalized = fryingTimer / fryingRecipeSO.GetMaxFryingTimer()
					});
					if (fryingTimer > fryingRecipeSO.GetMaxFryingTimer())
					{
						GetKitchenObject().DestroySelf();
						KitchenObject.SpawnKitchenObject(fryingRecipeSO.GetOutput(), this);
						state = State.Fried;
						burningTimer = 0;
						burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
						OnStateChange?.Invoke(this, new OnStateChangeEventArgs
						{
							state = this.state
						});
						OnUpdateInteract?.Invoke(this, EventArgs.Empty);
					}
					break;
				case State.Fried:
					burningTimer += Time.deltaTime;
					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
					{
						progressNormalized = burningTimer / burningRecipeSO.MaxBurningTimer
					});
					if (burningTimer > burningRecipeSO.MaxBurningTimer)
					{
						GetKitchenObject().DestroySelf();
						KitchenObject.SpawnKitchenObject(burningRecipeSO.GetOutput(), this);
						state = State.Burned;
						OnStateChange?.Invoke(this, new OnStateChangeEventArgs
						{
							state = this.state
						});
						OnUpdateInteract?.Invoke(this, EventArgs.Empty);
					}
					break;
				case State.Burned:
					OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEvenArgs{
						progressNormalized = 0
					});
					break;
			}
		}
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
						SetState_Idle();
					}
				}
			}
			else
			{
				// Give KitchenObject to player
				GetKitchenObject().SetKitchenObjectParent(player);
				SetState_Idle();
			}
		}
		else
		{
			// There is no Kitchen Object
			if (player.HasKitchenObject())
			{
				if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
				{
					// put kitchen object on the counter, if player.KitchenObject can be fryed
					player.GetKitchenObject().SetKitchenObjectParent(this);
					fryingRecipeSO = GetFryingRecipeSOwithInput(GetKitchenObject().GetKitchenObjectSO());
					fryingTimer = 0;
					state = State.Frying;
					OnStateChange?.Invoke(this, new OnStateChangeEventArgs
					{
						state = this.state
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
	private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOwithInput(inputKitchenObjectSO);
		return fryingRecipeSO != null;
	}
	private FryingRecipeSO GetFryingRecipeSOwithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		foreach (var fryingRecipeSO in fryingRecipeSOArray)
		{
			if (inputKitchenObjectSO == fryingRecipeSO.GetInput())
			{
				return fryingRecipeSO;
			}
		}
		return null;
	}
	private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (inputKitchenObjectSO == burningRecipeSO.GetInput())
			{
				return burningRecipeSO;
			}
        }
		return null;
    }
	private void SetState_Idle()
	{
		state = State.Idle;
		OnStateChange?.Invoke(this, new OnStateChangeEventArgs
		{
			state = this.state
		});
		OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
		{
			progressNormalized = 0
		});
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
					// put kitchen object on the counter, if player.KitchenObject can be fryed
					action = "Fry " + player.GetKitchenObject().GetKitchenObjectSO().GetObjectName();
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
		action = null;
		return false;
	}

	public KitchenObjectSO GetKitchenObjectSO()
	{
		return GetKitchenObject().GetKitchenObjectSO();
	}
}
