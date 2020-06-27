using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] bgMusic;
    public AudioSource[] sfx;

    public AudioMixerGroup musicMixer, sfxMixer;

    public int levelMusic = 2;

    private int currentTrack;

    public float musicLevel;
    public float sfxLevel;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic(levelMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            currentTrack++;
            PlayMusic(currentTrack);
        }
    }

    public void PlayMusic(int musicTrackNo)
    {
        for(int i = 0; i < bgMusic.Length; i++)
        {
            bgMusic[i].Stop();
        }

        bgMusic[musicTrackNo].Play();
    }

    public void PlaySFX(int SFXNo)
    {
        sfx[SFXNo].Play();
    }

    public void SetMusicLevel()
    {
        musicMixer.audioMixer.SetFloat("MusicVol", UIManager.instance.musicVolSlider.value);
      
    }

    public void SetSFXLevel()
    {
        sfxMixer.audioMixer.SetFloat("SfxVol", UIManager.instance.sfxVolSlider.value);
    }

    public void GetMusicLevel()
    {
        musicMixer.audioMixer.GetFloat("MusicVol", out musicLevel);

    }

    public void GetSFXLevel()
    {
        sfxMixer.audioMixer.GetFloat("SfxVol", out sfxLevel);
    }
}
