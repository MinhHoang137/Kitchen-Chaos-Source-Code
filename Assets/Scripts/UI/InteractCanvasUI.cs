using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractCanvasUI : MonoBehaviour
{
	[SerializeField] private GameObject interactHolder;
	[SerializeField] private RectTransform interactKeyFrame;
	[SerializeField] private TextMeshProUGUI interactKey;
	[SerializeField] private TextMeshProUGUI interactText;

	[SerializeField] private GameObject interactAlternateHolder;
	[SerializeField] private RectTransform interactAlternateKeyFrame;
	[SerializeField] private TextMeshProUGUI interactAlternateKey;
	[SerializeField] private TextMeshProUGUI interactAlternateText;

	[SerializeField] private GameObject interactableGameObject;
	private IInteractable interactable;

	private float initialSize = 0.2f;
	private float sizePerCharacter = 0.1f;
	private Player player;
	void Start()
    {
		if (!interactableGameObject.TryGetComponent<IInteractable>(out interactable))
		{
			Debug.Log("Cant get IInteractable");
			return;
		}
		player = Player.Instance;
		player.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
		interactable.OnUpdateInteract += Interactable_OnUpdateInteract;
		interactable.OnUpdateInteractAlternate += Interactable_OnUpdateInteractAlternate;
        gameObject.SetActive(false);
    }

	private void Interactable_OnUpdateInteractAlternate(object sender, System.EventArgs e)
	{
		UpdateInteractCanvas();
	}

	private void Interactable_OnUpdateInteract(object sender, System.EventArgs e)
	{
		UpdateInteractCanvas();
	}

	private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
	{
		if (interactable == (e.selectedCounter as IInteractable))
		{
			gameObject.SetActive(true);
			UpdateInteractCanvas();
		}
		else
		{
			HideCanvas(gameObject);
		}
	}

	private void ShowCanvas(GameObject canvasGameObject)
	{
		canvasGameObject.SetActive(false);
		canvasGameObject.SetActive(true);
	}
	private void HideCanvas(GameObject canvasGameObject)
	{
		canvasGameObject.SetActive(false);
	}
	private void SetKeyFrameSize(RectTransform frame, TextMeshProUGUI keyText)
	{
		frame.sizeDelta = new Vector2(initialSize + sizePerCharacter * (keyText.text.Length - 1), frame.sizeDelta.y);
	}
	private void UpdateInteractCanvas()
	{
		// Handle interact
		if (interactable == null) { return; }
		string action;
		if (interactable.CanInteract(player, out action))
		{
			interactKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
			interactText.text = action;
			SetKeyFrameSize(interactKeyFrame, interactKey);
			ShowCanvas(interactHolder);
		}
		else
		{
			HideCanvas(interactHolder);
		}
		// Handle InteractAlternate
		if (interactable.CanInteractAlternate(player, out action)) {
			ShowCanvas(interactAlternateHolder);
			interactAlternateKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
			interactAlternateText.text = action;
			SetKeyFrameSize(interactAlternateKeyFrame, interactAlternateKey);
		}
		else
		{
			HideCanvas(interactAlternateHolder);
		}
	}
}
