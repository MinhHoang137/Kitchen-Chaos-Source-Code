using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticEvent : MonoBehaviour
{
	private void Awake()
	{
		BaseCounter.ResetStaticEvent();
		CuttingCounter.ResetStaticEvent();
		TrashCounter.ResetStaticEvent();
		PlateKitchenObject.ResetStaticEvent();
	}
}
