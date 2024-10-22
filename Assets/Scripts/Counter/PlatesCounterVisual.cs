using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform plateVisualPrefab;
    private List<Transform> platesList;
    // Start is called before the first frame update
    void Start()
    {
        platesList = new List<Transform>();
		platesCounter.OnPlateSpawn += PlatesCounter_OnPlateSpawn;
		platesCounter.OnPlateRemove += PlatesCounter_OnPlateRemove;
    }
	private void PlatesCounter_OnPlateRemove(object sender, System.EventArgs e)
	{
		Transform plate = platesList[platesList.Count - 1];
        platesList.Remove(plate);
        Destroy(plate.gameObject);
	}

	private void PlatesCounter_OnPlateSpawn(object sender, System.EventArgs e)
	{
        Transform plate = Instantiate(plateVisualPrefab, platesCounter.GetKitchenObjectFollowTransform());
        platesList.Add(plate);
        float offsetY = 0.1f;
        plate.localPosition = new Vector3 (0, offsetY * platesList.Count, 0);
	}
}
