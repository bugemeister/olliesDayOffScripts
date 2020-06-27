using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject cpOn, cpOff;
    public GameObject checkpointEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!cpOn.activeSelf) //If cp on isnt active, set new checkpoint. This is here so you cant keep re setting a check point.
            {
                AudioManager.instance.PlaySFX(11);
                GameManager.instance.SetSpawnPoint(transform.position);

                Checkpoint[] allCheckPoints = FindObjectsOfType<Checkpoint>();
                for (int i = 0; i < allCheckPoints.Length; i++)
                {
                    allCheckPoints[i].cpOff.SetActive(true);
                    allCheckPoints[i].cpOn.SetActive(false);
                }
                cpOff.SetActive(false);
                cpOn.SetActive(true);
                Instantiate(checkpointEffect, gameObject.transform.position, gameObject.transform.rotation);
            }
        }
    }
}
