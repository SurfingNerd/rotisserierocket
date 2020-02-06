using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SmokeSpawner : MonoBehaviour
{
    public GameObject SmokePrefab;
    public Transform SmokeParent;
    public Vector3 SpawnOffset;

    public float StartDelay;
    public float TimeBetweenSpawns;
    public float SmokeVelocity;
    public float FadeDuration;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnSmoke());
    }

    IEnumerator SpawnSmoke()
    {
        GameObject go = null;

        // delay
        yield return new WaitForSecondsRealtime(StartDelay);

        while (true)
        {
            go = Instantiate(SmokePrefab, transform.position + SpawnOffset, Quaternion.identity, SmokeParent);
            go.transform.SetAsFirstSibling();

            var rb = go.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.left * SmokeVelocity;

            var img = go.GetComponent<Image>();
            img.DOFade(0f, FadeDuration).SetEase(Ease.OutSine);
            go.transform.DOScale(0.75f, FadeDuration).SetEase(Ease.OutSine);

            yield return new WaitForSecondsRealtime(TimeBetweenSpawns);

            img.DOKill();
            go.transform.DOKill();
            Destroy(go);
        }
    }
}
