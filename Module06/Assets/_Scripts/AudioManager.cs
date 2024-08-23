using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	[SerializeField] private AudioMixer	audioMixer;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 0));
		SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0));
	}

	public void SetSFXVolume(float volume)
	{
		if (volume <= 0.0001f)
			audioMixer.SetFloat("SFX", -80);
		else
			audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
	}

	public void SetBGMVolume(float volume)
	{
		if (volume <= 0.0001f)
			audioMixer.SetFloat("BGM", -80);
		else
			audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
	}
}
