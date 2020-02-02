using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhysicsNew : MonoBehaviour
{
    private LevelManager levelManager;

    public float normalizedDistanceToTarget = 0.0f;

    private float initialZ;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.Inst;
        initialZ = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {        
        //Vector3 sidewardVector = new Vector3(0, 0, 0);
        float x = 0;
        float y = 0;

        float energyScaling = 1.0f;

        foreach(RocketLeak leak in levelManager.currentRocketStatus.rocketLeaks)
        {
            x -= leak.transform.position.x * energyScaling;
            y -= leak.transform.position.y * energyScaling;
        }

        Vector3 forwardVector = new Vector3(x, y, 1.0f);

        // we drag the earth closer.
        gameObject.transform.position -= (forwardVector * Time.deltaTime);

        normalizedDistanceToTarget = 1 - gameObject.transform.position.z / initialZ;

        //Debug.Log("Appying Force: " + forwardVector + " New Earth Position: "  + gameObject.transform.position + " " + normalizedDistance);
    }
}
