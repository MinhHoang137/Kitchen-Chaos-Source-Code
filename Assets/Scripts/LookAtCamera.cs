using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	private enum Mode
	{
		LookAt,
		LookAtInverted,
		CameraForward,
		CameraForwardInverted
	}
	[SerializeField] private Mode mode;
	private void LateUpdate()
	{
		switch (mode)
		{
			case Mode.LookAt:
				transform.LookAt(Camera.main.transform);
				break;
			case Mode.LookAtInverted:
				Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
				Vector3 lookAtPosition = transform.position + dirFromCamera;
				transform.LookAt(lookAtPosition);
				break;
			case Mode.CameraForward:
				transform.forward = Camera.main.transform.forward;
				break;
			case Mode.CameraForwardInverted:
				transform.forward = -Camera.main.transform.forward;
				break;
		}
	}
}
