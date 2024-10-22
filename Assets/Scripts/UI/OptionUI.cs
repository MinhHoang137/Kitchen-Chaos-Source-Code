using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeNumber;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeNumber;
    [SerializeField] private Button closeButton;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
	[SerializeField] private Button moveLeftButton;
	[SerializeField] private Button moveRightButton;
	[SerializeField] private Button interactButton;
	[SerializeField] private Button interactAlternateButton;
	//[SerializeField] private Button pauseButton;

	[SerializeField] private TextMeshProUGUI moveUpButtonText;
	[SerializeField] private TextMeshProUGUI moveDownButtonText;
	[SerializeField] private TextMeshProUGUI moveLeftButtonText;
	[SerializeField] private TextMeshProUGUI moveRightButtonText;
	[SerializeField] private TextMeshProUGUI interactButtonText;
	[SerializeField] private TextMeshProUGUI interactAlternateButtonText;
	//[SerializeField] private TextMeshProUGUI pauseButtonText;

    [SerializeField] private GameObject keyRebindingUI;

    private Action onCloseButtonAction;
	private void Awake()
	{
        keyRebindingUI.SetActive(false);
		Hide();
	}
	private void Start()
	{
        sfxSlider.onValueChanged.AddListener(delegate { SetSfxVolume(); });
        musicSlider.onValueChanged.AddListener(delegate {SetMusicVolume(); });
        closeButton.onClick.AddListener(() =>
        {
            onCloseButtonAction();
            Hide();
        });

        moveUpButton.onClick.AddListener(() => {
            OnStartRebinding(moveUpButton);
            Rebind(GameInput.Binding.MoveUp, moveUpButton);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            OnStartRebinding(moveDownButton);
            Rebind(GameInput.Binding.MoveDown, moveDownButton);
        });
		moveLeftButton.onClick.AddListener(() =>
		{
			OnStartRebinding(moveLeftButton);
			Rebind(GameInput.Binding.MoveLeft, moveLeftButton);
		});
        moveRightButton.onClick.AddListener(() => {
            OnStartRebinding(moveRightButton);
            Rebind(GameInput.Binding.MoveRight, moveRightButton);
        });
		interactButton.onClick.AddListener(() => {
			OnStartRebinding(interactButton);
			Rebind(GameInput.Binding.Interact, interactButton);
		});
		interactAlternateButton.onClick.AddListener(() => {
			OnStartRebinding(interactAlternateButton);
			Rebind(GameInput.Binding.InteractAlternate, interactAlternateButton);
		});
		//pauseButton.onClick.AddListener(() => {
		//	OnStartRebinding(pauseButton);
		//	Rebind(GameInput.Binding.Pause, pauseButton);
		//});

		UpdateVisual();
	}
	private void OnEnable()
	{
		UpdateVisual();
	}
	public void SetSfxVolume()
    {
        SFXManager.Instance.SetVolume(sfxSlider.value / sfxSlider.maxValue);
        UpdateVisual();
    }
    public void SetMusicVolume()
    {
        MusicManager.Instance.SetVolume(musicSlider.value / musicSlider.maxValue);
        UpdateVisual();
    }
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
	private void UpdateVisual()
	{
        sfxSlider.value = Mathf.Round(SFXManager.Instance.GetVolume() * sfxSlider.maxValue);
		sfxVolumeNumber.text = Mathf.Round(SFXManager.Instance.GetVolume() * sfxSlider.maxValue).ToString();

        musicSlider.value = Mathf.Round(MusicManager.Instance.GetVolume() * musicSlider.maxValue);
		musicVolumeNumber.text = Mathf.Round(MusicManager.Instance.GetVolume() * musicSlider.maxValue).ToString();

        moveUpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        SetButtonSize(moveUpButton, moveUpButtonText);

        moveDownButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        SetButtonSize(moveDownButton, moveDownButtonText);

        moveLeftButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        SetButtonSize(moveLeftButton, moveLeftButtonText);

        moveRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        SetButtonSize (moveRightButton, moveRightButtonText);

        interactButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        SetButtonSize(interactButton, interactButtonText);

        interactAlternateButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        SetButtonSize(interactAlternateButton, interactAlternateButtonText);

        //pauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        //SetButtonSize(pauseButton, pauseButtonText);
	}
    private void OnStartRebinding(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.color = Color.green;
        keyRebindingUI.SetActive(true);
    }
    private void OnCompleteOrCancelRebinding(Button button) {
        button.GetComponent<Image>().color = Color.gray;
        keyRebindingUI.SetActive(false);
    }
    private void SetButtonSize(Button button, TextMeshProUGUI buttonText)
    {
		button.GetComponent<RectTransform>().sizeDelta = new Vector2(80 + 30 * (buttonText.text.Length - 1), button.GetComponent<RectTransform>().sizeDelta.y);
	}
    private void Rebind(GameInput.Binding binding, Button button)
    {
		keyRebindingUI.SetActive(true);
        GameInput.Instance.Rebind(binding, () =>
        {
            OnCompleteOrCancelRebinding(button);
            UpdateVisual();
        });
    }
}
