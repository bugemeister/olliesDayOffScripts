using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue;
    public GameObject coinEffect;
    public int soundToPlay;
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
            AudioManager.instance.PlaySFX(soundToPlay);
            GameManager.instance.AddCoins(coinValue);
            Destroy(gameObject);
            Instantiate(coinEffect, transform.position, transform.rotation);
        }
    }
}
