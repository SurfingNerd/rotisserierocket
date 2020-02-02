using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelManager : Singleton<LevelManager>
{
    public Image WinScreen;
    public Image LoseScreen;
    public Transform PlayScreen;

    public float maxTimeProgress = 1f;
    public float timeProgress = 0;

    public float levelDurationSeconds = 120;

    public float maxOxygen = 1f; //Obsolete comment: 1141kg = 1m3 uncompressed oxygen
    public float OxygenLevel;
    public Severity OxygenDepletionSeverity;
    public int NumLeaks = 0;
    public float OxygenDepletionPerLeak = 0.1f;

    private float elapsedTimeSeconds = 0;
    private float startTime = 0;
    
    public RocketStatus currentRocketStatus;

    public bool ShouldGameRun = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        OxygenLevel = maxOxygen;
        currentRocketStatus = new RocketStatus();

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTimeSeconds = Time.time - startTime;

        timeProgress = elapsedTimeSeconds / levelDurationSeconds;

        ConsumeOxygen(NumLeaks * OxygenDepletionPerLeak);
        EvaluateOxygenDepletionSeverity();
        CheckLoseConditions();
    }

    void EvaluateOxygenDepletionSeverity()
    {
        if (OxygenLevel < 0.2f) OxygenDepletionSeverity = Severity.High;
        else if (OxygenLevel < 0.4f) OxygenDepletionSeverity = Severity.Medium;
        else if (OxygenLevel < 0.6f) OxygenDepletionSeverity = Severity.Low;
        else if (OxygenLevel <= 0f) OxygenDepletionSeverity = Severity.Empty;
    }

    void CheckLoseConditions()
    {
        if(timeProgress >= maxTimeProgress || OxygenDepletionSeverity == Severity.Empty)
            StartCoroutine(GameOver());
    }

    public void StartGame()
    {
        PlayScreen.DOScaleY(0f, 1f);
        ShouldGameRun = true;
    }

    IEnumerator GameOver()
    {
        ShouldGameRun = false;
        LoseScreen.DOFade(1f, 2f);
        yield return new WaitForSecondsRealtime(6f);
        //PlayScreen.DOScaleY(1f, 1f);
    }

    public void ConsumeOxygen(float depletionValue)
    {
        OxygenLevel -= depletionValue;
    }
}

public enum Severity
{
    None,
    Low,
    Medium,
    High,
    Empty
}
