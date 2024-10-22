using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
	[SerializeField] private Button resumeButton;
	[SerializeField] private Button mainMenuButton;
	[SerializeField] private Button optionButton;
	[SerializeField] private OptionUI optionUI;
	private void Start()
	{
		resumeButton.onClick.AddListener(() => {
			GameManager.Instance.TogglePauseGame();
		});
		mainMenuButton.onClick.AddListener(() => { 
			SceneLoader.Load(SceneLoader.Scene.MainMenuScene); 
		});
		optionButton.onClick.AddListener(() => { 
			optionUI.Show(Show); 
			Hide(); 
		});
		GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
		GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
		Hide();
	}

	private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
	{
		optionUI.Hide();
		Hide();
	}

	private void GameManager_OnGamePaused(object sender, System.EventArgs e)
	{
		Show();
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
