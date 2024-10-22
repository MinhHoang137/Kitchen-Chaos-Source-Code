using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDelivered;
	[SerializeField] private Button restartButton;
	[SerializeField] private Button mainMenuButton;
	private void Start()
	{
		restartButton.onClick.AddListener(() =>
		{
			SceneLoader.Load(SceneLoader.Scene.GameScene);
		});
		mainMenuButton.onClick.AddListener(() => {
			SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
		});
		GameManager.Instance.OnStateChange += GameManager_OnStateChange;
		Hide();
	}

	private void GameManager_OnStateChange(object sender, System.EventArgs e)
	{
		if (GameManager.Instance.IsGameOver())
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
		recipeDelivered.text = DeliveryManager.Instance.DeliveredRecipeAmount.ToString();
    }
    private void Hide()
    { 
        gameObject.SetActive(false);
    }
}
