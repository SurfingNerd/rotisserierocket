using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public Image WinScreen;
    public Image LoseScreen;


    [HideInInspector] public float maxOxygen = 1f; //Obsolete comment: 1141kg = 1m3 uncompressed oxygen
    [HideInInspector] public float OxygenLevel;
    [HideInInspector] public Severity OxygenDepletionSeverity;
    [HideInInspector] public int NumLeaks => currentRocketStatus.rocketLeaks.Count;
    
    public float OxygenDepletionPerLeak = 0.000001f;

    //calculated in the Physics
    [HideInInspector] public float TotalDistance;

    [HideInInspector] public float DistanceMissing;

    //calculated in the Physics
    [HideInInspector] public float DistanceCovered => (TotalDistance - DistanceMissing);

    [HideInInspector] public float NormalizedDistanceCovered;

    private float elapsedTimeSeconds = 0;
    private float startTime = 0;
    
    public RocketStatus currentRocketStatus;

    // How Close do i need to get to the planet to actually Win the game ?
    public float WinCoditionMinimumDistance = 120;

    [HideInInspector] public bool ShouldGameRun = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        OxygenLevel = maxOxygen;
        currentRocketStatus = new RocketStatus();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Restart"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }


        elapsedTimeSeconds = Time.time - startTime;

        ConsumeOxygen(NumLeaks * OxygenDepletionPerLeak * Time.deltaTime);
        EvaluateOxygenDepletionSeverity();
        CheckLoseConditions();
        CheckWinConditions();
    }

    void EvaluateOxygenDepletionSeverity()
    {
        if (OxygenLevel <= 0f) OxygenDepletionSeverity = Severity.Empty;
        else if (OxygenLevel < 0.2f) OxygenDepletionSeverity = Severity.High;
        else if (OxygenLevel < 0.4f) OxygenDepletionSeverity = Severity.Medium;
        else if (OxygenLevel < 0.6f) OxygenDepletionSeverity = Severity.Low;
    }

    void CheckLoseConditions()
    {
        //Debug.Log("OxygenLevel: " + this.OxygenLevel.ToString("0.000") + " Serverity: " + OxygenDepletionSeverity);
        if(OxygenDepletionSeverity == Severity.Empty)
        {
            //Debug.LogError("Game Over");
            StartCoroutine(GameOver());
        }
    }

    void CheckWinConditions()
    {
        //Debug.Log("OxygenLevel: " + this.OxygenLevel.ToString("0.000") + " Serverity: " + OxygenDepletionSeverity);
        if(DistanceMissing <= WinCoditionMinimumDistance)
        {
            //Debug.LogError("Game Over");
            StartCoroutine(WinGame());
        }        
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator WinGame()
    {
        ShouldGameRun = false;
        WinScreen.DOFade(1f, 2f);
        yield return new WaitForSecondsRealtime(6f);
    }

    IEnumerator GameOver()
    {
        ShouldGameRun = false;
        LoseScreen.DOFade(1f, 2f);
        yield return new WaitForSecondsRealtime(6f);
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
