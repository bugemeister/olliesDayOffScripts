using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LSLevelEntry : MonoBehaviour
{
    public string levelName, levelToCheck, displayName;
    private bool canLoadLevel, levelUnlocked;
    public GameObject mapPointActive, mapPointInactive;
    private bool levelLoading;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(levelToCheck + "_unlocked") == 1 || levelToCheck == "")
        {
            mapPointActive.SetActive(true);
            mapPointInactive.SetActive(false);
            levelUnlocked = true;
        }
        else
        {
            mapPointActive.SetActive(false);
            mapPointInactive.SetActive(true);
            levelUnlocked = false;
        }

        if (PlayerPrefs.GetString("CurrentLevel") == levelName)
        {
            PlayerController.instance.controller.enabled = false;
            PlayerController.instance.transform.position = transform.position;
            PlayerController.instance.controller.enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && canLoadLevel && levelUnlocked && !levelLoading && !UIManager.instance.fadeFromBlack)
        {
            StartCoroutine(LevelEndCo());
            levelLoading = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canLoadLevel = true;
            LSUIManager.instance.nameplate.SetActive(true);
            LSUIManager.instance.levelNameText.text = displayName;

            if(PlayerPrefs.HasKey(levelName + "_coins"))
            {
                LSUIManager.instance.coinText.text = PlayerPrefs.GetInt(levelName + "_coins").ToString();
            }
            else
            {
                LSUIManager.instance.coinText.text = "N/A";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            canLoadLevel = false;
            LSUIManager.instance.nameplate.SetActive(false);
        }
    }

    public IEnumerator LevelEndCo()
    {
        PlayerPrefs.SetString("CurrentLevel", levelName);

        PlayerController.instance.anim.SetBool("IsJumping", false);
        PlayerController.instance.levelEnded = true;
        UIManager.instance.fadeToBlack = true;

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(levelName);
    }
}
