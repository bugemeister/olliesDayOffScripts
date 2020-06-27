using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 respawnPosition;

    public GameObject deathEffect;

    public int currentCoins;

    public bool isDead = false;

    public int levelEndMusic = 8;

    public string levelToLoad;

    public bool isRespawning;

    public Quaternion rotation;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        respawnPosition = PlayerController.instance.transform.position;
        AddCoins(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && !isDead)
        {
            PauseUnpause();
        }
        
    }

    public void Respawn()
    {
        float camDisableTime = 0f;
        camDisableTime += Time.deltaTime;

        AudioManager.instance.PlaySFX(6);
        StartCoroutine(RespawnCo());

        HealthManager.instance.PlayerKilled();
    }

    public IEnumerator RespawnCo()
    {
        Instantiate(deathEffect, PlayerController.instance.transform.position + new Vector3(0f, 0f, 0f), PlayerController.instance.transform.rotation);

        PlayerController.instance.wallJumpText.enabled = false;
        PlayerController.instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        CameraController.instance.theCMBrain.enabled = false;

        UIManager.instance.fadeToDeath = true;

        yield return new WaitForSeconds(2f);

        isRespawning = true;

        HealthManager.instance.ResetHealth();

        UIManager.instance.fadeFromDeath = true;

        CameraController.instance.theCMBrain.enabled = true;
        PlayerController.instance.transform.position = respawnPosition;
        PlayerController.instance.gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint + new Vector3(0f, 1f, 0f);
    }

    public void AddCoins(int coinsToAdd)
    {
        currentCoins += coinsToAdd;
        UIManager.instance.coinText.text = "" + currentCoins;
    }

    public void PauseUnpause()
    {
        if(UIManager.instance.pauseScreen.activeInHierarchy)
        {
            UIManager.instance.pauseScreen.SetActive(false);
            Time.timeScale = 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            UIManager.instance.pauseScreen.SetActive(true);
            UIManager.instance.CloseOptions();
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public IEnumerator LevelEndCo()
    {
        rotation = PlayerController.instance.transform.rotation;
       

        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);
        AudioManager.instance.PlayMusic(levelEndMusic);
        PlayerController.instance.levelEnded = true;
        yield return new WaitForSeconds(3.5f);
        UIManager.instance.fadeToBlack = true;
        yield return new WaitForSeconds(2f);
        Debug.Log("Level Ended");

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_coins"))
        {
            if (currentCoins > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_coins"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_coins", currentCoins);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_coins", currentCoins);
        }

        SceneManager.LoadScene(levelToLoad);
    }
}
