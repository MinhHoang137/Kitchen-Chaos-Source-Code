using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject kitchenGameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameOjectsList;
	private void OnEnable()
	{
		if (plateKitchenObject != null)
		{
			plateKitchenObject.OnIngredientAdd += PlateKitchenObject_OnIngredientAdd;
		}
	}
	private void Start()
	{
		foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSOGameOjectsList)
		{
			kitchenObjectSO_GameObject.kitchenGameObject.SetActive(false);
		}
    }

	private void PlateKitchenObject_OnIngredientAdd(object sender, PlateKitchenObject.OnIngredientAddEventArgs e)
	{
		foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSOGameOjectsList)
		{
            if (kitchenObjectSO_GameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSO_GameObject.kitchenGameObject.SetActive(true);
                return;
            }
		}
	}
}
