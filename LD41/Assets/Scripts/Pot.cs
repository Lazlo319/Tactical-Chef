﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pot : MonoBehaviour
{

    [SerializeField] private float temperature = 20f;
    [SerializeField] private float boilingTemp = 100f;//temperature the pasta cooks
    [SerializeField] private float coolingTemp = 0.5f;//every so often, how much heat does the pot lose
    public GameObject emptyingPot;

    private List<GameObject> drips;//all the holes shot in the pot
    private int pastaAdded = 0, tomatosAdded = 0, onionsAdded = 0;
    private float timer;
    private float maxTimer = 3f;//3 seconds
    private float health = 100;
    private bool broken = false;
    private bool knockedOver = false;
    private GameController gc;
    private AudioSource audio;

    private bool timerStart = false;
    private float finalTime = 2;

    public float Temperature
    {
        get
        {
            return temperature;
        }

    }

    public int PastaAdded
    {
        get
        {
            return pastaAdded;
        }

        set
        {
            pastaAdded = value;
        }
    }

    public int TomatosAdded
    {
        get
        {
            return tomatosAdded;
        }

        set
        {
            tomatosAdded = value;
        }
    }

    public int OnionsAdded
    {
        get
        {
            return onionsAdded;
        }

        set
        {
            onionsAdded = value;
        }
    }

    public float BoilingTemp
    {
        get
        {
            return boilingTemp;
        }

        set
        {
            boilingTemp = value;
        }
    }

    public bool Broken
    {
        get
        {
            return broken;
        }

        set
        {
            broken = value;
        }
    }

    public bool KnockedOver
    {
        get
        {
            return knockedOver;
        }

        set
        {
            knockedOver = value;
        }
    }


    // Use this for initialization
    void Start()
    {
        gc = FindObjectOfType<GameController>();
        audio = GetComponent<AudioSource>();
        timer = maxTimer;
    }

    public void heatUp(float damage)
    {
        temperature += damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStart)
        {
            if (finalTime > 0)
            {
                finalTime -= Time.deltaTime;
            } else
            {
                knockedOver = true;
            }
        }
        if (!audio.isPlaying && temperature >= boilingTemp)
        {
            audio.Play();
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (temperature > 0)
                temperature -= coolingTemp;
            else temperature = 0;
            timer = maxTimer;
        }

        if (health < 0 && !broken) broken = true;
        
    }
    public void KnockOver()
    {
        if (!knockedOver)
            GetComponent<Animation>().Play();
        timerStart = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Fire"))
        {
            if (temperature <= 120)
                heatUp(1f);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("Bullet") && gc.CurrentObjective == 3)
        {
            Debug.Log("Hit!");
            other.gameObject.GetComponent<AudioSource>().Play();
            health -= 10;
            Destroy(other.gameObject);
        }
    }
}