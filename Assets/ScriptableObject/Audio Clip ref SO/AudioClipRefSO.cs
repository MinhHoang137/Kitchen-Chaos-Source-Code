using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefSO : ScriptableObject
{
    [SerializeField] private AudioClip[] chop;
	[SerializeField] private AudioClip[] deliveryFail;
	[SerializeField] private AudioClip[] deliverySuccess;
	[SerializeField] private AudioClip[] footstep;
	[SerializeField] private AudioClip[] objectDrop;
	[SerializeField] private AudioClip[] objectPickUp;
	[SerializeField] private AudioClip[] stoveSizzle;
	[SerializeField] private AudioClip[] trash;
	[SerializeField] private AudioClip[] warning;
	[SerializeField] private AudioClip[] backgroundMusic;
	
	public AudioClip[] Chop { get { return chop; } }
	public AudioClip[] DeliveryFail { get { return deliveryFail; } }
	public AudioClip[] DeliverySuccess { get { return deliverySuccess; } }
	public AudioClip[] Footstep { get { return footstep; } }
	public AudioClip[] ObjectDrop { get { return objectDrop; } }
	public AudioClip[] ObjectPickUp { get { return objectPickUp; } }
	public AudioClip[] StoveSizzle { get { return stoveSizzle; } }
	public AudioClip[] Trash { get { return trash; } }
	public AudioClip[] Warning { get { return warning; } }
	public AudioClip[] BackgroundMusic { get { return backgroundMusic; } }

}
