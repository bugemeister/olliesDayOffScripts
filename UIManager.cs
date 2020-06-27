using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image deathScreen, blackScreen;
    public float fadeSpeed = 0.1f;
    public bool fadeToDeath, fadeFromDeath;
    public bool fadeToBlack, fadeFromBlack;

    public Text healthText;
    public Image healthImage;

    public Text coinText;
    public Image coinImage;

    public GameObject pauseScreen, optionsScreen;

    public Slider musicVolSlider, sfxVolSlider;

    public string levelSelect, mainMenu;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.GetSFXLevel();
        AudioManager.instance.GetMusicLevel();

        sfxVolSlider.value = AudioManager.instance.sfxLevel;
        musicVolSlider.value = AudioManager.instance.musicLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeToBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 1f)
            {
                fadeToBlack = false;
            } 
        }
   
        if(fadeFromBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 0f)
            {
                fadeFromBlack = false;
            }
        }

        if (fadeToDeath)
        {
            deathScreen.color = new Color(deathScreen.color.r, deathScreen.color.g, deathScreen.color.b, Mathf.MoveTowards(deathScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (deathScreen.color.a == 1f)
            {
                fadeToDeath = false;
            } 
        }

        if(fadeFromDeath)
        {
            deathScreen.color = new Color(deathScreen.color.r, deathScreen.color.g, deathScreen.color.b, Mathf.MoveTowards(deathScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (deathScreen.color.a == 0f)
            {
                fadeFromDeath = false;
            }
        }
    }

    public void Resume()
    {
        GameManager.instance.PauseUnpause();
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }

    public void SetMusicLevel()
    {
        AudioManager.instance.SetMusicLevel();
    }

    public void SetSFXLevel()
    {
        AudioManager.instance.SetSFXLevel();
    }
}

