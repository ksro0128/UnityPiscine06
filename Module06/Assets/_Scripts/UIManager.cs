using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] sirenImage;
    [SerializeField] private TMPro.TMP_Text keyText;
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private GameObject caughtPanel;
    [SerializeField] private GameObject manualPanel;
    [SerializeField] private GameObject[] manualPages;
    private int pageIndex = 0;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMPro.TMP_Text viewText;
    public static UIManager instance;
    private int sirenIndex = 0;
    private int keys = 0;
    private AudioSource ClearSound;
    private AudioSource CaughtSound;
    private bool isCaught = false;
    private bool isClear = false;

    [SerializeField] private GameObject savePanel;
    [SerializeField] private TMPro.TMP_InputField intputNickName;
    [SerializeField] private TMPro.TMP_Text saveTime;
    [SerializeField] private Button saveButton;
    [SerializeField] private GameObject noKeyText;
    private bool isSave = false;



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        foreach (GameObject image in sirenImage)
        {
            image.SetActive(false);
        }
        keyText.text = keys + " / 3";
        ClearSound = clearPanel.GetComponent<AudioSource>();
        CaughtSound = caughtPanel.GetComponent<AudioSource>();
        foreach (GameObject page in manualPages)
        {
            page.SetActive(false);
        }
        manualPanel.SetActive(false);
        clearPanel.SetActive(false);
        caughtPanel.SetActive(false);
        savePanel.SetActive(false);
        noKeyText.SetActive(false);
        if (GameManager.instance != null)
        {
            if (GameManager.instance.IsFirstPlay())
                ShowManual();
            else
            {
                Timer.instance.StartTimer();
                GameManager.instance.SetPlaying(true);
            }
        }
        else
            ShowManual();
    }

    void ShowManual()
    {
        manualPanel.SetActive(true);
        manualPages[pageIndex].SetActive(true);
    }

    public void PreviousPage()
    {
        if (pageIndex == 0)
            return;
        if (pageIndex == manualPages.Length - 1)
        {
            nextButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Next";
        }
        manualPages[pageIndex].SetActive(false);
        pageIndex--;
        manualPages[pageIndex].SetActive(true);
    }

    public void NextPage()
    {
        manualPages[pageIndex].SetActive(false);
        pageIndex++;

        if (pageIndex == manualPages.Length - 1)
            nextButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Start";

        if (pageIndex == manualPages.Length)
        {
            manualPanel.SetActive(false);
            if (GameManager.instance != null)
                GameManager.instance.SetPlaying(true);
            Timer.instance.StartTimer();
            return;
        }
        manualPages[pageIndex].SetActive(true);
    }

    public void ShowSirenImage()
    {
        sirenImage[sirenIndex].SetActive(true);
        sirenIndex++;
    }

    public void HideSirenImage()
    {
        sirenImage[sirenIndex - 1].SetActive(false);
        sirenIndex--;
    }

    public void AddKeyText()
    {
        keys++;
        keyText.text = keys + " / 3";
    }

    public void ShowClearPanel()
    {
        if (isClear)
            return;
        clearPanel.SetActive(true);
        ClearSound.Play();
        isClear = true;
    }

    public void ShowCaughtPanel()
    {
        if (isCaught)
            return;
        caughtPanel.SetActive(true);
        CaughtSound.Play();
        isCaught = true;
    }

    public void SetViewText(string text)
    {
        viewText.text = text;
    }
    

    public void ShowSavePanel()
    {
        saveTime.text = Timer.instance.GetTimeString();
        savePanel.SetActive(true);
    }

    public void SaveNickName()
    {
        if (isSave)
            return;
        if (intputNickName.text == "")
            return;
        isSave = true;
        saveButton.GetComponentInChildren<TMPro.TMP_Text>().text = "저장완료";
        saveButton.interactable = false;
        string nickName = intputNickName.text;
        string timeString = Timer.instance.GetTimeString();
        float time = Timer.instance.GetTime();
        string saveData = JsonUtility.ToJson(new SaveData(nickName, timeString, time));
        string allData = PlayerPrefs.GetString("SaveData");
        if (allData == "")
            allData = saveData;
        else
            allData += "\n" + saveData;
        PlayerPrefs.SetString("SaveData", allData);
    }

    public void GoMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ShowNoKeyText()
    {
        noKeyText.SetActive(true);
        StartCoroutine(HideNoKeyText());
    }

    IEnumerator HideNoKeyText()
    {
        yield return new WaitForSeconds(2);
        noKeyText.SetActive(false);
    }

}
