using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	private const string MUSIC_VOLUME = "MusicVolume";
	[SerializeField] private AudioClipRefSO audioClipRefSO;
	private AudioClip backgroundMusic;
	private AudioSource audioSource;
    public static MusicManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}
	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.3f);
		StartCoroutine(PlayBackgroundMusic());
	}
	private AudioClip GetRandomBackgroundMusic()
	{
		AudioClip backgroundMusic = audioClipRefSO.BackgroundMusic[Random.Range(0, audioClipRefSO.BackgroundMusic.Length)];
		return backgroundMusic;
	}
	private IEnumerator PlayBackgroundMusic()
	{
		backgroundMusic = GetRandomBackgroundMusic();
		audioSource.clip = backgroundMusic;
		audioSource.Play();
		yield return new WaitForSecondsRealtime(backgroundMusic.length);
		StartCoroutine(PlayBackgroundMusic());
	}
	public void SetVolume(float volumeNormalized)
	{
		audioSource.volume = volumeNormalized;
		PlayerPrefs.SetFloat(MUSIC_VOLUME, volumeNormalized);
	}
	public float GetVolume()
	{
		return audioSource.volume;
	}
}
