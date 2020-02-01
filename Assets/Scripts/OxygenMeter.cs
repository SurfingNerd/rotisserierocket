using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OxygenMeter : MonoBehaviour
{
    public Image[] CondensationImgs;
    public Image ReddeningArea;
    public RectTransform Dial;
    public float OxygenLevel => 0.25f;
    
    Severity currentSeverity = Severity.None;

    #region Redness
    float currentMinRed;
    float currentMaxRed;

    float rednessAnimTime = 1.3f;
    bool isReddening = true;
    bool isRednessAnimPlaying = false;
    #endregion

    #region Condensation
    bool shouldCondensate = false;

    float currentMinCondensation;
    float currentMaxCondensation;

    float condensatingAnimTime = 1.5f;
    bool isCondensating = true;
    bool isCondensatingAnimPlaying = false;
    #endregion

    float MaxDialZRotation = -120;
    float DialZRotation = -120;

    float dialJitterDelay = 0.13f;
    float dialJitterFrequency = 0.1f;
    float jitterExtreme = -25f;
    Coroutine dialJitterCoroutine = null;

    void Update()
    {
        //ReddeningArea.fillAmount = OxygenLevel;
        EvaluateSeverity();

        /*
        if (!isRednessAnimPlaying)
            RunRedWarningInterpolation();
        */

        if (!isCondensatingAnimPlaying && shouldCondensate)
            RunCondensationInterpolation();

        SetDialBasePosition();
    }

    void EvaluateSeverity()
    {
        if (OxygenLevel < 0.2f && currentSeverity != Severity.High)
            SetNewAnims(Severity.High);
        else if (OxygenLevel < 0.4f && currentSeverity != Severity.Medium)
            SetNewAnims(Severity.Medium);
        else if (OxygenLevel < 0.6f && currentSeverity != Severity.Low)
            SetNewAnims(Severity.Low);
    }

    void SetNewAnims(Severity newSeverity)
    {
        currentSeverity = newSeverity;

        switch (currentSeverity)
        {
            case Severity.Low:
                currentMinRed = 0.25f;
                currentMaxRed = 0.5f;
                rednessAnimTime = 0.7f;
                break;
            case Severity.Medium:
                currentMinRed = 0.5f;
                currentMaxRed = 0.75f;
                rednessAnimTime = 0.55f;

                shouldCondensate = true;
                currentMinCondensation = 0.2f;
                currentMaxCondensation = 0.6f;
                condensatingAnimTime = 1.5f;
                break;
            case Severity.High:
                currentMinRed = 0.75f;
                currentMaxRed = 1f;
                rednessAnimTime = 0.4f;

                shouldCondensate = true;
                currentMinCondensation = 0.4f;
                currentMaxCondensation = 0.8f;
                condensatingAnimTime = 0.9f;
                break;
            default: break;
        }
    }

    void RunRedWarningInterpolation()
    {
        var newVal = 1f - ((isReddening) ? currentMaxRed : currentMinRed);
        ReddeningArea.DOColor(new Color(1f, newVal, newVal), rednessAnimTime)
            .SetEase(Ease.InSine)
            .OnStart(() => 
            {
                isRednessAnimPlaying = true;
            })
            .onComplete += () => 
            {
                isReddening = !isReddening;
                isRednessAnimPlaying = false;
            };
    }

    void RunCondensationInterpolation()
    {
        // make sure no anim is still playing
        foreach (var img in CondensationImgs)
            img.DOKill();

        var newVal = (isCondensating) ? currentMaxCondensation : currentMinCondensation;
        for (int i = 0; i < 4; i++)
        {
            if(i < 3) CondensationImgs[i].DOColor(new Color(1f, 1f, 1f, newVal), condensatingAnimTime).SetEase(Ease.OutSine);
            else
            {
                CondensationImgs[i].DOColor(new Color(1f, 1f, 1f, newVal), condensatingAnimTime).SetEase(Ease.OutSine)
                .OnStart(() =>
                {
                    isCondensatingAnimPlaying = true;
                    if (!isCondensating) dialJitterCoroutine = StartCoroutine(StartDialJitter());
                    else
                    {
                        if(dialJitterCoroutine != null) StopCoroutine(dialJitterCoroutine);
                        dialJitterCoroutine = null;
                        ResetDialToBase();
                    }
                        
                })
                .onComplete += () =>
                {
                    isCondensating = !isCondensating;
                    isCondensatingAnimPlaying = false;
                };
            }
        }
    }

    void SetDialBasePosition()
    {
        DialZRotation = MaxDialZRotation + (240f * OxygenLevel);
        Dial.localEulerAngles = new Vector3(0f, 0f, DialZRotation);
    }

    IEnumerator StartDialJitter()
    {
        yield return new WaitForSecondsRealtime(dialJitterDelay);
        
        while (true)
        {
            Dial.DOKill();
            float ranVal = Random.Range(jitterExtreme, jitterExtreme * 0.25f);
            Dial.DORotate(new Vector3(0f, 0f, DialZRotation + ranVal), dialJitterFrequency).SetEase(Ease.InExpo);
            yield return new WaitForSecondsRealtime(dialJitterFrequency);
        }
    }

    void ResetDialToBase()
    {
        Dial.localEulerAngles = new Vector3(0f, 0f, DialZRotation);
    }

    enum Severity
    {
        None,
        Low,
        Medium,
        High
    }
}
