using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject settingPanel;
	[SerializeField] private Slider bgmSlider;
	[SerializeField] private Slider sfxSlider;


	void Start()
	{
		settingPanel.SetActive(false);
		bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0);
		sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0);
	}

    public void StartButton()
	{
		Debug.Log("Start Button Clicked");
	}

	public void SettingButton()
	{
		Debug.Log("Setting Button Clicked");
		if (settingPanel.activeSelf)
		{
			settingPanel.SetActive(false);
		}
		else
		{
			settingPanel.SetActive(true);
		}	
	}

	public void ExitButton()
	{
		Debug.Log("Exit Button Clicked");
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	public void BGMVolumeChange()
	{
		PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
		AudioManager.instance.SetBGMVolume(bgmSlider.value);
	}

	public void SFXVolumeChange()
	{
		PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
		AudioManager.instance.SetSFXVolume(sfxSlider.value);
	}
}
