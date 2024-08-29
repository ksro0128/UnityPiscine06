using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject settingPanel;
	[SerializeField] private GameObject rankPanel;
	[SerializeField] private GameObject scrollViewContent;
	[SerializeField] private GameObject rankPrefab;
	[SerializeField] private Slider bgmSlider;
	[SerializeField] private Slider sfxSlider;


	void Start()
	{
		settingPanel.SetActive(false);
		rankPanel.SetActive(false);
		bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0);
		sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0);
		LoadRankData();
	}

    public void StartButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Stage1");
	}

	public void SettingButton()
	{
		if (settingPanel.activeSelf)
		{
			settingPanel.SetActive(false);
		}
		else
		{
			settingPanel.SetActive(true);
		}	
	}

	public void RankButton()
	{
		if (rankPanel.activeSelf)
		{
			rankPanel.SetActive(false);
		}
		else
		{
			rankPanel.SetActive(true);
		}
	}

	public void ExitButton()
	{
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

	void LoadRankData()
	{
		string allData = PlayerPrefs.GetString("SaveData");
        if (!string.IsNullOrEmpty(allData))
        {
            string[] records = allData.Split('\n');
            List<SaveData> rankingList = new List<SaveData>();

            foreach (string record in records)
            {
                SaveData data = JsonUtility.FromJson<SaveData>(record);
                rankingList.Add(data);
            }

            rankingList.Sort((a, b) => a.time.CompareTo(b.time));

            foreach (SaveData data in rankingList)
            {
                GameObject rankItem = Instantiate(rankPrefab, scrollViewContent.transform);
                rankItem.transform.Find("PlayerName").GetComponent<TMPro.TMP_Text>().text = data.nickName;
                rankItem.transform.Find("Time").GetComponent<TMPro.TMP_Text>().text = data.timeString;
            }
        }
	}
}
