using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    [SerializeField] private Player player;
	[SerializeField] private AudioClipRefSO audioClipRefSO;
    private AudioSource audioSource;

	private float footstepTimer;
	private float footstepCycle = 0.1f;
	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}
	private void Update()
	{
		if (player.IsWalking)
		{
			footstepTimer -= Time.deltaTime;
			if (footstepTimer < 0)
			{
				footstepTimer = footstepCycle;
				// play footstep
				audioSource.clip = audioClipRefSO.Footstep[Random.Range(0, audioClipRefSO.Footstep.Length)];
				audioSource.Play();
			}
		}
		else
		{
			audioSource.Pause();
		}
		audioSource.volume = SFXManager.Instance.GetVolume();
	}
}
