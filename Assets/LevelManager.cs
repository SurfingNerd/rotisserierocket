using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public float levelProgress = 0;

    public float levelLengthSeconds = 120;
    public float startingOxygenKilos = 1141; //1141kg = 1m3 uncompressed oxygen

    public float oxygenWarningLevelKilos = 200;

    private float elapsedTimeSeconds = 0;
    private float startTime = 0;
    private float currentOxygenKilos;
    private bool outOfOxygen = false;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        currentOxygenKilos = startingOxygenKilos;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTimeSeconds = Time.time - startTime;

        levelProgress = elapsedTimeSeconds / levelLengthSeconds;

        CheckOxygenLevels();

        CheckLoseConditions();
    }

    private void CheckOxygenLevels()
    {
        if(currentOxygenKilos <= 0)
        {
            outOfOxygen = true;
        }
        else if (currentOxygenKilos <= oxygenWarningLevelKilos)
        {

        }
    }

    private void CheckLoseConditions()
    {

    }

    public void ConsumeOxygen(float kilograms)
    {
        currentOxygenKilos -= kilograms;
    }

}
