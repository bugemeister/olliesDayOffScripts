using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;
    public Animator anim;
    public GameObject exitZone;
    public float waitForExit;

    public enum BossPhase { intro, phase1, phase2, phase3, end};
    public BossPhase currentPhase = BossPhase.intro;

    public int bossMusic, bossDeath, bossDeathShout, bossHit;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       
    }

    private void OnEnable()
    {
        AudioManager.instance.PlayMusic(bossMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isRespawning)
        {
            currentPhase = BossPhase.intro;
            anim.SetBool("Phase1", false);
            anim.SetBool("Phase2", false);
            anim.SetBool("Phase3", false);

            AudioManager.instance.PlayMusic(AudioManager.instance.levelMusic);

            gameObject.SetActive(false);

            BossActivator.instance.gameObject.SetActive(true);
            BossActivator.instance.entrance.SetActive(true);

            GameManager.instance.isRespawning = false;
        }


    }

    public void DamageBoss()
    {
        AudioManager.instance.PlaySFX(bossHit);
        currentPhase++;

        if (currentPhase != BossPhase.end)
        {
            anim.SetTrigger("Hurt");
        }

        switch (currentPhase)
        {
            case BossPhase.phase1:
                anim.SetBool("Phase1", true);
                break;
            case BossPhase.phase2:
                anim.SetBool("Phase2", true);
                anim.SetBool("Phase1", false);
                break;
            case BossPhase.phase3:
                anim.SetBool("Phase3", true);
                anim.SetBool("Phase2", false);
                break;
            case BossPhase.end:
                anim.SetTrigger("End");
                StartCoroutine(EndBossCo());
                break;
        }
    }

    IEnumerator EndBossCo()
    {
        AudioManager.instance.PlaySFX(bossDeath);
        AudioManager.instance.PlaySFX(bossDeathShout);
        yield return new WaitForSeconds(waitForExit);
        Destroy(BossActivator.instance.gameObject.GetComponent<BoxCollider>());
        exitZone.SetActive(true);
        AudioManager.instance.PlayMusic(AudioManager.instance.levelMusic);
    }
}
