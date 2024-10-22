using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
	public static SFXManager Instance { get; private set; }

	private const string SFX_VOLUME = "SfxVolume";
	[SerializeField] private AudioClipRefSO audioClipRefSO;
	private float sfxVolume = 1;
	private void Awake()
	{
		Instance = this;
		sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME, 1);
	}
	private void Start()
	{
		DeliveryManager.Instance.OnDeliverSuccess_SFX += DeliveryManager_OnDeliverSuccess_SFX;
		DeliveryManager.Instance.OnDeliverFailed += DeliveryManager_OnDeliverFailed;
		CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
		Player.Instance.OnPlayerPickUpSomething += Player_OnPlayerPickUpSomething;
		BaseCounter.OnAnyObjectPlaced += BaseCounter_OnAnyObjectPlaced;
		TrashCounter.OnAnyTrashed += TrashCounter_OnAnyTrashed;
		PlateKitchenObject.OnIngredientAdd_SFX += PlateKitchenObject_OnIngredientAdd_SFX;
	}

	private void PlateKitchenObject_OnIngredientAdd_SFX(object sender, System.EventArgs e)
	{
		PlateKitchenObject plateKitchenObject = sender as PlateKitchenObject;
		PlaySound(audioClipRefSO.ObjectPickUp, plateKitchenObject.transform.position, sfxVolume);
	}

	private void TrashCounter_OnAnyTrashed(object sender, System.EventArgs e)
	{
		TrashCounter trashCounter = (TrashCounter)sender;
		PlaySound(audioClipRefSO.Trash, trashCounter.transform.position,sfxVolume);
	}

	private void BaseCounter_OnAnyObjectPlaced(object sender, System.EventArgs e)
	{
		BaseCounter baseCounter = sender as BaseCounter;
		PlaySound(audioClipRefSO.ObjectDrop, baseCounter.transform.position,sfxVolume);
	}

	private void Player_OnPlayerPickUpSomething(object sender, System.EventArgs e)
	{
		PlaySound(audioClipRefSO.ObjectPickUp, Player.Instance.transform.position,sfxVolume);
	}

	private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
	{
		CuttingCounter cuttingCounter = (CuttingCounter)sender;
		PlaySound(audioClipRefSO.Chop, cuttingCounter.transform.position, sfxVolume);
	}

	private void DeliveryManager_OnDeliverFailed(object sender, DeliveryManager.OnDeliverEventArgs e)
	{
		PlaySound(audioClipRefSO.DeliveryFail, e.position,sfxVolume);
	}

	private void DeliveryManager_OnDeliverSuccess_SFX(object sender, DeliveryManager.OnDeliverEventArgs e)
	{
		PlaySound(audioClipRefSO.DeliverySuccess, e.position, sfxVolume);
	}

	private void PlaySound(AudioClip clip, Vector3 position, float volume = 1)
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
    private void PlaySound(AudioClip[] clipArray,  Vector3 position, float volume = 1)
    {
        PlaySound(clipArray[Random.Range(0, clipArray.Length)], position, volume);
    }
	public void SetVolume(float volumeNormalized)
	{
		sfxVolume = volumeNormalized;
		PlayerPrefs.SetFloat(SFX_VOLUME, sfxVolume);
	}
	public float GetVolume() { return sfxVolume; }
	public void PlayWarningSound(Vector3 position)
	{
		PlaySound(audioClipRefSO.Warning, position, sfxVolume);
	}
}
