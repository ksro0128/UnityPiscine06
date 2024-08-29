using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
	private bool isFirstPlay = true;
	private bool isPlaying = false;


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
		// PlayerPrefs.DeleteAll();
	}

	public bool IsFirstPlay()
	{
		if (isFirstPlay)
		{
			isFirstPlay = false;
			return true;
		}
		return isFirstPlay;
	}

	public bool IsPlaying()
	{
		return isPlaying;
	}

	public void SetPlaying(bool isPlaying)
	{
		this.isPlaying = isPlaying;
	}

}
