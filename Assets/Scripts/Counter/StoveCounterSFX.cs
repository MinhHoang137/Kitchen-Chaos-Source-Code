using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSFX : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}
	private void Start()
	{
		stoveCounter.OnStateChange += StoveCounter_OnStateChange;
	}

	private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e)
	{
		bool playSound = (e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying);
		if (playSound) {
			audioSource.Play();
		}
        else
        {
            audioSource.Pause();
        }
    }
	private void Update()
	{
		audioSource.volume = SFXManager.Instance.GetVolume();
	}
}
