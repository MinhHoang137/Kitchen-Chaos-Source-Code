using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;
	[SerializeField] private TextMeshProUGUI recipeName;
	[SerializeField] private Image waitingRecipeTimerImage;
	private float waitingRecipeSOTimeMax = 1;
	private float waitingRecipeSOTimerNormalized = 1;
	private void Awake()
	{
		iconTemplate.gameObject.SetActive(false);
	}
	private void Update()
	{
		waitingRecipeSOTimerNormalized -= Time.deltaTime/waitingRecipeSOTimeMax;
		waitingRecipeTimerImage.fillAmount = waitingRecipeSOTimerNormalized;
	}
	public void SetRecipeSO(RecipeSO recipeSO, float timeRemainNormalized)
	{
		foreach (Transform child in iconContainer)
		{
			if (child == iconTemplate) continue;
			Destroy(child.gameObject);
		}
		recipeName.text = recipeSO.RecipeName;
		foreach (KitchenObjectSO kitchenObjectSO in recipeSO.KitchenObjectSOList)
		{
			Transform icon = Instantiate(iconTemplate, iconContainer);
			icon.gameObject.SetActive(true);
			icon.GetComponent<Image>().sprite = kitchenObjectSO.GetSprite();
		}
		waitingRecipeSOTimeMax = recipeSO.WaitTimeMax;
		waitingRecipeSOTimerNormalized = timeRemainNormalized;
	}
}
