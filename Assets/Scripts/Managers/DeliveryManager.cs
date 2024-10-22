using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
	public event EventHandler OnRecipeSpawn;
	public event EventHandler OnRecipeRemove;
	public event EventHandler OnDeliverSuccess_Visual;
	public event EventHandler<OnDeliverEventArgs> OnDeliverFailed;
	public event EventHandler<OnDeliverEventArgs> OnDeliverSuccess_SFX;

	public class OnDeliverEventArgs : EventArgs
	{
		public Vector3 position;
	}
	public static DeliveryManager Instance { get; private set; }

	[SerializeField] private RecipeListSO recipeListSO;
	private List<RecipeSO> waitingRecipeSOList;
	private List<float> waitingRecipeSOTimerNormalizedList;

	private float spawnRecipeTimer = 3;
	private float spawnRecipeCycle = 15;
	private int waitingRecipeMax = 4;
	public int DeliveredRecipeAmount { get; private set; } = 0;
	private int failToDeliverAllowMax = 5;
	public int FailToDeliverAllowMax { get { return failToDeliverAllowMax; } }
	private int failToDeliverAllow = 5;
	public int FailToDeliverAllow { get { return failToDeliverAllow; } }
	private void Awake()
	{
		Instance = this;
		waitingRecipeSOList = new List<RecipeSO>();
		waitingRecipeSOTimerNormalizedList = new List<float>();
	}
	private void Update()
	{
		if (!GameManager.Instance.IsGamePlaying()) return;
		spawnRecipeTimer -= Time.deltaTime;
		if (spawnRecipeTimer <= 0)
		{
			spawnRecipeTimer = spawnRecipeCycle;
			if (waitingRecipeSOList.Count < waitingRecipeMax)
			{
				RecipeSO waitingRecipeSO = recipeListSO.RecipeSOList[UnityEngine.Random.Range(0, recipeListSO.RecipeSOList.Count)];
				AddWaitingRecipeSO(waitingRecipeSO);
				OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
			}
		}

		for (int i = 0; i < waitingRecipeSOList.Count; i++)
		{
			waitingRecipeSOTimerNormalizedList[i] -= (Time.deltaTime / waitingRecipeSOList[i].WaitTimeMax);
			if (waitingRecipeSOTimerNormalizedList[i] <= 0)
			{
				RemoveWaitingRecipeAt(i);
				failToDeliverAllow--;
				OnRecipeRemove?.Invoke(this, EventArgs.Empty);
			}
		}

	}
	public void DeliverRecipe(PlateKitchenObject plateKitchenObject, Vector3 deliverPosition)
	{

		for (int i = 0; i < waitingRecipeSOList.Count; i++)
		{
			RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
			if (waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOsList().Count)
			{
				// Loop all the recipe
				bool plateContentMatchesRecipe = true;

				foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList)
				{
					plateContentMatchesRecipe = (plateContentMatchesRecipe && plateKitchenObject.GetKitchenObjectSOsList().Contains(recipeKitchenObjectSO));
				}
				if (plateContentMatchesRecipe)
				{
					RemoveWaitingRecipeAt(i);
					DeliveredRecipeAmount++;
					OnDeliverSuccess_Visual?.Invoke(this, EventArgs.Empty);
					OnDeliverSuccess_SFX?.Invoke(this, new OnDeliverEventArgs { position = deliverPosition });
					return;
				}
			}
		}
		OnDeliverFailed?.Invoke(this, new OnDeliverEventArgs { position = deliverPosition });
	}
	public List<RecipeSO> GetWaitingRecipeSOList() { return waitingRecipeSOList; }
	public List<float> GetWaitingRecipeSOTimerNormalizedList() { return waitingRecipeSOTimerNormalizedList; }
	private void AddWaitingRecipeSO(RecipeSO waitingRecipeSO)
	{
		waitingRecipeSOList.Add(waitingRecipeSO);
		waitingRecipeSOTimerNormalizedList.Add(waitingRecipeSO.WaitTimeMax / waitingRecipeSO.WaitTimeMax);
	}
	private void RemoveWaitingRecipeAt(int index)
	{
		waitingRecipeSOList.RemoveAt(index);
		waitingRecipeSOTimerNormalizedList.RemoveAt(index);
	}
	public RecipeListSO RecipeListSO { get { return recipeListSO; } }
}
