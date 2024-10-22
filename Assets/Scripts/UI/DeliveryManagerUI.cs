using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;
	[SerializeField] private TextMeshProUGUI failToDeliver;
	private void Awake()
	{
		recipeTemplate.gameObject.SetActive(false);
	}
	private void Start()
	{
		DeliveryManager.Instance.OnRecipeSpawn += DeliveryManager_OnRecipeSpawn;
		DeliveryManager.Instance.OnDeliverSuccess_Visual += DeliveryManager_OnDeliverSuccessfully;
		DeliveryManager.Instance.OnRecipeRemove += DeliveryManager_OnRecipeRemove;
	}

	private void DeliveryManager_OnRecipeRemove(object sender, System.EventArgs e)
	{
		UpdateVisual();
		failToDeliver.text = "Failed Allow: " + DeliveryManager.Instance.FailToDeliverAllow.ToString();
	}

	private void DeliveryManager_OnDeliverSuccessfully(object sender, System.EventArgs e)
	{
		UpdateVisual();
	}

	private void DeliveryManager_OnRecipeSpawn(object sender, System.EventArgs e)
	{
		UpdateVisual();
	}

	private void UpdateVisual()
	{
		foreach (Transform child in container)
		{
			if (child == recipeTemplate) continue;
			Destroy(child.gameObject);
		}
		List<RecipeSO> waitingRecipeSO = DeliveryManager.Instance.GetWaitingRecipeSOList();
		List<float> waitingTimerNormalized = DeliveryManager.Instance.GetWaitingRecipeSOTimerNormalizedList();
		for (int i = 0; i < waitingRecipeSO.Count; i++)
		{
			Transform recipeIconTransfrom = Instantiate(recipeTemplate, container);
			recipeIconTransfrom.gameObject.SetActive(true);
			recipeIconTransfrom.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(waitingRecipeSO[i], waitingTimerNormalized[i]);
		}
	}
}
