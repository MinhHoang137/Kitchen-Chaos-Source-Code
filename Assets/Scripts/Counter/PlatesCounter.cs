using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter, IInteractable
{
    public event EventHandler OnPlateSpawn;
    public event EventHandler OnPlateRemove;
	public event EventHandler OnUpdateInteract;
	public event EventHandler OnUpdateInteractAlternate;

	[SerializeField] KitchenObjectSO plateKitchenOjectSO;

    private float plateSpawnCycle = 4f;
    private float plateSpawnTimer = 2;
    private int plateCount = 0;
    private float plateCountLimit = 4;

	private void Start()
	{
		OnUpdateInteractAlternate?.Invoke(this, EventArgs.Empty);
	}
	// Update is called once per frame
	void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        plateSpawnTimer += Time.deltaTime;
        if (plateSpawnTimer > plateSpawnCycle)
        {
            plateSpawnTimer = 0;
            // Spawn plate
            if (plateCount < plateCountLimit) {
                plateCount++;
                OnPlateSpawn?.Invoke(this, EventArgs.Empty);
            }
            if (plateCount == 1)
            {
                OnUpdateInteract?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (plateCount > 0)
            {
                //Give Plate to player
                plateCount--;
                OnPlateRemove?.Invoke(this, EventArgs.Empty);
                KitchenObject plate = KitchenObject.SpawnKitchenObject(plateKitchenOjectSO, this);
                plate.SetKitchenObjectParent(player);
            }
        }
        OnUpdateInteract?.Invoke(this, EventArgs.Empty );
    }
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return plateKitchenOjectSO;
    }

	public bool CanInteract(Player player, out string action)
	{
        action = IInteractable.GRAB;
		if (!player.HasKitchenObject())
		{
			if (plateCount > 0)
			{
                action += GetKitchenObjectSO().GetObjectName();
                return true;
			}
		}
        return false;
	}

	public bool CanInteractAlternate(Player player, out string action)
	{
		action = null;
        return false;
	}
}
