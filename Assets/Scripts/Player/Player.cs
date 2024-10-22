using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
	public static Player Instance;

	public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
	public event EventHandler OnPlayerPickUpSomething;
	public class OnSelectedCounterChangedEventArgs : EventArgs
	{
		public BaseCounter selectedCounter;
	}


	[SerializeField] private float speed = 7;
	private GameInput gameInput;
	[SerializeField] private LayerMask countersLayerMask;

	private BaseCounter selectedCounter;
	private KitchenObject kitchenObject;
	[SerializeField] private Transform kitchenObjectHoldPoint;
	public bool IsWalking { get; private set; }
	private void Awake()
	{
		Instance = this;
		gameInput = GameInput.Instance;
	}
	// Start is called before the first frame update
	void Start()
	{
		gameInput.OnInteractAction += GameInput_OnInteractAction;
		gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
	}

	private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
	{
		if (!GameManager.Instance.IsGamePlaying()) return;
		if (selectedCounter != null)
		{
			selectedCounter.InteractAlternate(this);
		}
	}

	private void GameInput_OnInteractAction(object sender, System.EventArgs e)
	{
		if (!GameManager.Instance.IsGamePlaying()) return;
		if (selectedCounter != null)
		{
			selectedCounter.Interact(this);
		}
	}

	// Update is called once per frame
	void Update()
	{
		HandleMovement();
		HandleInteractions();
	}
	private void HandleMovement()
	{
		Vector3 moveDir = gameInput.GetMovementVectorNormalized();
		float moveDistance = speed * Time.deltaTime;
		float playerHeight = 2;
		float playerRadius = 0.7f;
		bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
		if (!canMove)
		{
			// try move along x
			Vector3 moveDirX = Vector3.right * moveDir.x;
			canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
			if (canMove)
			{
				transform.Translate(moveDirX * moveDistance, Space.World);
			}
			else
			{
				// try move along z
				Vector3 moveDirZ = Vector3.forward * moveDir.z;
				canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
				if (canMove)
				{
					transform.Translate(moveDirZ * moveDistance, Space.World);
				}
			}
		}
		else
		{
			//transform.position += input.normalized * speed * Time.deltaTime; ~
			transform.Translate(moveDir * moveDistance, Space.World);
		}
		IsWalking = (moveDir != Vector3.zero);
		transform.forward = Vector3.Slerp(transform.forward, moveDir.normalized, Time.deltaTime * speed);
	}
	private void HandleInteractions()
	{
		float interactDistance = 2;
		if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, interactDistance, countersLayerMask))
		{
			if (raycastHit.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
			{
				if (baseCounter != selectedCounter)
				{
					SetSelectedCounter(baseCounter);
				}
			}
			else
			{
				SetSelectedCounter(null);
			}
		}
		else
		{
			SetSelectedCounter(null);
		}
	}
	private void SetSelectedCounter(BaseCounter baseCounter)
	{
		if (!GameManager.Instance.IsGamePlaying()) return;
		this.selectedCounter = baseCounter;
		OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = this.selectedCounter });
	}

	public Transform GetKitchenObjectFollowTransform()
	{
		return kitchenObjectHoldPoint;
	}
	public void SetKitchenObject(KitchenObject kitchenObject)
	{
		this.kitchenObject = kitchenObject;
		if (kitchenObject != null) {
			OnPlayerPickUpSomething?.Invoke(this, EventArgs.Empty);
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
