using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
	[SerializeField] private Button closeButton;

	[SerializeField] private RectTransform moveUpFrame;
	[SerializeField] private TextMeshProUGUI moveUpKey;
	[SerializeField] private RectTransform moveLeftFrame;
	[SerializeField] private TextMeshProUGUI moveLeftKey;
	[SerializeField] private RectTransform moveDownFrame;
	[SerializeField] private TextMeshProUGUI moveDownKey;
	[SerializeField] private RectTransform moveRightFrame;
	[SerializeField] private TextMeshProUGUI moveRightKey;
	[SerializeField] private RectTransform interactFrame;
	[SerializeField] private TextMeshProUGUI interactKey;
	[SerializeField] private RectTransform interactAltFrame;
	[SerializeField] private TextMeshProUGUI interactAltKey;
	[SerializeField] private RectTransform pauseFrame;
	[SerializeField] private TextMeshProUGUI pauseKey;
	private float firstCharSize = 30;
	private float sizePerChar = 12;
	private void Start()
	{
		closeButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});
		UpdateKeys();
		gameObject.SetActive(false);
	}
	private void UpdateKeys()
	{
		moveUpKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
		SetKeyFrameSize(moveUpFrame, moveUpKey);

		moveLeftKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
		SetKeyFrameSize(moveLeftFrame, moveLeftKey);

		moveDownKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
		SetKeyFrameSize(moveDownFrame, moveDownKey);

		moveRightKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
		SetKeyFrameSize(moveRightFrame, moveRightKey);

		interactKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
		SetKeyFrameSize(interactFrame, interactKey);

		interactAltKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
		SetKeyFrameSize(interactAltFrame, interactAltKey);

		pauseKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
		SetKeyFrameSize(pauseFrame, pauseKey);

	}
	private void SetKeyFrameSize(RectTransform frame, TextMeshProUGUI key)
	{
		frame.sizeDelta = new Vector2(firstCharSize + sizePerChar * (key.text.Length - 1), frame.sizeDelta.y);
	}
}