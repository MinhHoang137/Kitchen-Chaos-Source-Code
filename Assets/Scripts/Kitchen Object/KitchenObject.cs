using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        this.kitchenObjectParent?.ClearKitchenObject();
        this.kitchenObjectParent = kitchenObjectParent;
		kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }
    public IKitchenObjectParent GetKitchenObjectParent() { 
        return kitchenObjectParent; 
    }
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
		Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.GetPrefab());
		KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
	}
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
	{
		if (this is not PlateKitchenObject)
		{
			plateKitchenObject = null;
			return false;
		}
		plateKitchenObject = this as PlateKitchenObject;
		return true;
	}
}
