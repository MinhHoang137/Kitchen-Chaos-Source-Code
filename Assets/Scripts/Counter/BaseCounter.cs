using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
	public static event EventHandler OnAnyObjectPlaced;

	[SerializeField] private Transform counterTopPoint;

	private KitchenObject kitchenObject;
	public static void ResetStaticEvent()
	{
		OnAnyObjectPlaced = null;
	}
	public virtual void Interact(Player player)
	{
		Debug.Log("BaseCounter.Interact()");
	}
	public virtual void InteractAlternate(Player player)
	{
		Debug.Log("BaseCounter.InteractAlternate()");
	}
	public Transform GetKitchenObjectFollowTransform()
	{
		return counterTopPoint;
	}
	public void SetKitchenObject(KitchenObject kitchenObject)
	{
		this.kitchenObject = kitchenObject;
		if (kitchenObject != null) {
			OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
		}
	}
	public KitchenObject GetKitchenObject() { return kitchenObject; }
	public bool HasKitchenObject()
	{
		return kitchenObject != null;
	}
	public void ClearKitchenObject()
	{
		kitchenObject = null;
	}
}
