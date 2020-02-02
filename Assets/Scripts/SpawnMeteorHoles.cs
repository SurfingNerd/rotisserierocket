﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeteorHoles : MonoBehaviour
{

    public GameObject RocketHolePrefab;

    public float SpawnRate = 5;


    private float m_lastSpawn;

    private RocketStatus m_currentRocketStatus;
    // Start is called before the first frame update
    void Start()
    {
        m_currentRocketStatus = LevelManager.Inst.currentRocketStatus;

    }

    // Update is called once per frame
    void Update()
    {
        m_lastSpawn += Time.deltaTime;
        if (m_lastSpawn > SpawnRate)
        {
            m_lastSpawn = 0;

            Vector3 position = new Vector3(-2.98f, 18.26f, 88.43f);

            Quaternion correctRotation = new  Quaternion();
            LevelManager.Inst.currentRocketStatus.AddLeak(position, correctRotation, RocketHolePrefab, transform, 1.0f);

            
        }
    }
}


