﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject entrance, theBoss;
    public static BossActivator instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            entrance.SetActive(false);
            theBoss.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
