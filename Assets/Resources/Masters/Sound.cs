using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

	public AudioSource musicSource;
	public AudioSource sfxSource;
	

	public  void playSFX(AudioClip sfxClip)
	{
		if(sfxClip!=null)sfxSource.PlayOneShot(sfxClip);
	}

	public  void playMusic(AudioClip musicClip)
	{
		if(musicClip!=null)
		{
			musicSource.clip = musicClip;
			musicSource.Play();
		}
		
	}


}