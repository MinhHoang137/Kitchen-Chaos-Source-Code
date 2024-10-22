using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
	[SerializeField] private GameObject hasProgressGameObject;
	[SerializeField] private Image barImage;
	private IHasProgress hasProgress;
	private void Start()
	{
		if (!hasProgressGameObject.TryGetComponent<IHasProgress>(out hasProgress))
		{
			Debug.LogError(hasProgressGameObject.name + " doesn't have IHasProgress component!");
			return;
		}
		hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
		barImage.fillAmount = 0;
		Hide();
	}

	private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEvenArgs e)
	{
		barImage.fillAmount = e.progressNormalized;
		if (0 < barImage.fillAmount && barImage.fillAmount < 1 )
		{
			Show();
		}
		else
		{
			Hide();
		}
	}
	private void Show()
	{
		gameObject.SetActive(true);
	}
	private void Hide()
	{
		gameObject.SetActive(false);	
	}
}
