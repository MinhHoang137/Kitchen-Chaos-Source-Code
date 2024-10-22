using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject tutorialUI;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneLoader.Load(SceneLoader.Scene.GameScene);
        });
        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
		});
        howToPlayButton.onClick.AddListener(() => {
            tutorialUI.SetActive(true);
        });
        Time.timeScale = 1.0f;
    }
}
