using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
	public static GameInput Instance { get; private set; }

	private const string INPUT_BINDING = "InputBinding";

	public event EventHandler OnInteractAction;
	public event EventHandler OnInteractAlternateAction;
	public event EventHandler OnPause;
	private PlayerInputActions playerInputActions;

	private InputAction inputAction;
	private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

	public enum Binding
	{
		MoveUp,
		MoveDown,
		MoveLeft,
		MoveRight,
		Interact,
		InteractAlternate,
		Pause
	}

	private void Awake()
	{
		Instance = this;
		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();
		playerInputActions.Player.Pause.performed += Pause_performed;
		playerInputActions.Player.Interact.performed += Interact_performed;
		playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
		playerInputActions.Player.CancelRebind.performed += CancelRebind_performed;
		//PlayerPrefs.SetString(INPUT_BINDING, null);
		if (PlayerPrefs.HasKey(INPUT_BINDING))
		{
			playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(INPUT_BINDING));
		}
	}

	private void CancelRebind_performed(InputAction.CallbackContext obj)
	{
		// Cancel the ongoing rebinding operation if one is in progress
		if (rebindingOperation == null)
		{
			return;
		}
		rebindingOperation.Cancel();
	}

	private void OnDestroy()
	{
		playerInputActions.Player.Interact.performed -= Interact_performed;
		playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
		playerInputActions.Player.Pause.performed -= Pause_performed;
		playerInputActions.Dispose();
	}

	private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		OnPause?.Invoke(this, EventArgs.Empty);
	}

	private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
	}

	private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		OnInteractAction?.Invoke(this, EventArgs.Empty);
	}

	public Vector3 GetMovementVectorNormalized()
	{
		Vector3 inputVector = playerInputActions.Player.Move.ReadValue<Vector3>();
		//Debug.Log(inputVector);
		return inputVector.normalized;
	}
	public string GetBindingText(Binding binding)
	{
		switch (binding)
		{
			case Binding.MoveUp:
				return playerInputActions.Player.Move.bindings[5].ToDisplayString();
			case Binding.MoveDown:
				return playerInputActions.Player.Move.bindings[6].ToDisplayString();
			case Binding.MoveLeft:
				return playerInputActions.Player.Move.bindings[3].ToDisplayString();
			case Binding.MoveRight:
				return playerInputActions.Player.Move.bindings[4].ToDisplayString();
			case Binding.Interact:
				return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
			case Binding.InteractAlternate:
				return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
			case Binding.Pause:
				return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
			default:
				return "";
		}
	}
	public void Rebind(Binding binding, Action onCompleteOrCancel)
	{
		playerInputActions.Disable();
		inputAction = playerInputActions.Player.Move;
		int bindingIndex = 0;
		switch (binding)
		{
			case Binding.MoveUp:
				inputAction = playerInputActions.Player.Move;
				bindingIndex = 5;
				break;
			case Binding.MoveDown:
				inputAction = playerInputActions.Player.Move;
				bindingIndex = 6;
				break;
			case Binding.MoveLeft:
				inputAction = playerInputActions.Player.Move;
				bindingIndex = 3;
				break;
			case Binding.MoveRight:
				inputAction = playerInputActions.Player.Move;
				bindingIndex = 4;
				break;
			case Binding.Interact:
				inputAction = playerInputActions.Player.Interact;
				bindingIndex = 0;
				break;
			case Binding.InteractAlternate:
				inputAction = playerInputActions.Player.InteractAlternate;
				bindingIndex = 0;
				break;
			case Binding.Pause:
				inputAction = playerInputActions.Player.Pause;
				bindingIndex = 0;
				break;
		}
		rebindingOperation = inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete((callback) =>
		{
			callback.Dispose();
			playerInputActions.Enable();
			onCompleteOrCancel();
			PlayerPrefs.SetString(INPUT_BINDING, playerInputActions.SaveBindingOverridesAsJson());
		}).OnCancel(callback =>
		{
			rebindingOperation.Dispose();
			rebindingOperation = null;

			// Re-enable player input actions after canceling
			playerInputActions.Enable();
			onCompleteOrCancel();
		}).Start();
	}
}
