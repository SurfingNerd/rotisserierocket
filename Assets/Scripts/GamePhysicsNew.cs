using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhysicsNew : MonoBehaviour
{
    private LevelManager levelManager;


    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.Inst;
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
            x += leak.transform.position.x;
            y += leak.transform.position.y;
        }

        Vector3 forwardVector = new Vector3(x, y, 1.0f);

        // we drag the earth closer.
        gameObject.transform.position -= (forwardVector * energyScaling * Time.deltaTime);

        Debug.Log("Appying Force: " + forwardVector + " New Earth Position: "  + gameObject.transform.position);
    }
}
