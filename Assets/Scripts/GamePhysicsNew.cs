using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhysicsNew : MonoBehaviour
{
    private LevelManager levelManager;

    public float normalizedDistanceToTarget = 0.0f;

    private float initialZ;

    public float winDistance = 120;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        initialZ = gameObject.transform.position.z;
        levelManager.TotalDistance = initialZ;
        levelManager.DistanceMissing = levelManager.TotalDistance;
    }

    // Update is called once per frame
    void Update()
    {        
        //Vector3 sidewardVector = new Vector3(0, 0, 0);
        float x = 0;
        float y = 0;

        float energyScaling = 100.0f;

        foreach(RocketLeak leak in levelManager.currentRocketStatus.rocketLeaks)
        {
            x -= leak.transform.position.x * energyScaling;
            y -= leak.transform.position.y * energyScaling;
        }

        Vector3 forwardVector = new Vector3(x, y, 100.0f);

        // we drag the earth closer.
        gameObject.transform.position -= (forwardVector * Time.deltaTime);

        normalizedDistanceToTarget = 1 - gameObject.transform.position.z / initialZ;


        levelManager.NormalizedDistanceCovered = normalizedDistanceToTarget;

        levelManager.DistanceMissing = gameObject.transform.position.magnitude;

        Debug.Log("Distance: " + gameObject.transform.position.magnitude);

        // if (gameObject.transform.position.magnitude < winDistance)
        // {
        //     //UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("WinScene");
        //     UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("WinScene");
        // }


        // if (gameObject.transform.position.z < -1000)
        // {
        //     //UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("GameOverScene");
        //     UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameOverScene");
            
        // }

        //Debug.Log("Appying Force: " + forwardVector + " New Earth Position: "  + gameObject.transform.position + " " + normalizedDistance);
    }
}
