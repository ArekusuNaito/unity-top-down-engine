using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaster : MonoBehaviour {

	public AudioSource musicSource;
	public AudioSource sfxSource;
	

	public static  void playSFX(AudioClip sfxClip)
	{
		if(sfxClip!=null)instance.sfxSource.PlayOneShot(sfxClip);
	}

	public static  void playMusic(AudioClip musicClip)
	{
		if(musicClip!=null)
		{
			instance.musicSource.clip = musicClip;
			instance.musicSource.Play();
		}
		
	}



	public static SoundMaster instance = null;
	void createSingleton()
	{
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	void Awake () 
	{
		createSingleton();
		
	}

}