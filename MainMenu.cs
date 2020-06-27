using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel, levelSelect;
    public GameObject continueButton;
    public string[] levelList;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Continue"))
        {
            continueButton.SetActive(true);
        } else
        {
            ResetProgress();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        ResetProgress();
        SceneManager.LoadScene(firstLevel);

        PlayerPrefs.SetInt("Continue", 0);
        PlayerPrefs.SetString("CurrentLevel", firstLevel);
        Time.timeScale = 1f;
    }

    public void Continue()
    {
        PlayerPrefs.SetString("CurrentLevel", firstLevel);
        SceneManager.LoadScene(levelSelect);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        //for (int i = 0; i < levelList.Length; i++)
        //{
         //   PlayerPrefs.SetInt(levelList[i] + "_unlocked", 0);
        //}
    }
}
