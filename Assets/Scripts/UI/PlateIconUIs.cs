using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUIs : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
	[SerializeField] private Transform iconTemplate;
	private void Start()
	{
		plateKitchenObject.OnIngredientAdd += PlateKitchenObject_OnIngredientAdd;
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(false);
		}
	}

	private void PlateKitchenObject_OnIngredientAdd(object sender, PlateKitchenObject.OnIngredientAddEventArgs e)
	{
		UpdateVisual();
	}
	private void UpdateVisual()
	{
        foreach (Transform child in transform)
        {
			if (child == iconTemplate) continue; 
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOsList())
        {
			Transform iconTransform = Instantiate(iconTemplate, transform);
			iconTransform.gameObject.SetActive(true);
			iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
