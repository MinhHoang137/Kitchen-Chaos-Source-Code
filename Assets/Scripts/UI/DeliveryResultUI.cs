using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI deliveryResult;
    [SerializeField] private Color successfulColor;
    [SerializeField] private Color failureColor;
    [SerializeField] private Image resultIcon;
    [SerializeField] private Sprite successfulSprite;
    [SerializeField] private Sprite failureSprite;
	private void Start()
	{
		DeliveryManager.Instance.OnDeliverSuccess_Visual += DeliveryManager_OnDeliverSuccess_Visual;
		DeliveryManager.Instance.OnDeliverFailed += DeliveryManager_OnDeliverFailed;
        Hide();
	}

	private void DeliveryManager_OnDeliverFailed(object sender, DeliveryManager.OnDeliverEventArgs e)
	{
        deliveryResult.text = "Delivery\nFailed";
        background.color = failureColor;
        resultIcon.sprite = failureSprite;
        Show();
	}

	private void DeliveryManager_OnDeliverSuccess_Visual(object sender, System.EventArgs e)
	{
        deliveryResult.text = "Deliver\nSuccess";
        background.color = successfulColor;
        resultIcon.sprite = successfulSprite;
        Show();
	}

	public void Show()
    {
        gameObject.SetActive(true);
        background.gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false); 
        background.gameObject.SetActive(false);
    }
}
