using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosition : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject destinationPlanet;
    public GameObject skySphere;

    private Vector3 targetStartingPosition = new Vector3(0, 0, 0);
    private Vector3 movementVector = new Vector3(0, 0, 0);
    private Vector3 currentPositon = new Vector3(0, 0, 0);

    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        targetStartingPosition = destinationPlanet.transform.position;
        levelManager = LevelManager.Inst;

        SetMovement(new Vector3(0, 0,0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        foreach (RocketLeak leak in levelManager.currentRocketStatus.rocketLeaks)
        {
            Quaternion rotation = leak.transform.rotation;
            movementVector = Quaternion.Lerp(Quaternion.identity, rotation, Time.deltaTime) * movementVector;
        }

        //movementVector = movementVector * 0.1f;

        currentPositon += movementVector;

        gameObject.transform.position = currentPositon;

        gameObject.transform.forward = movementVector;

        Vector3 relativePosition = gameObject.transform.InverseTransformPoint(targetStartingPosition);

        float scalingFactor = targetStartingPosition.magnitude / relativePosition.magnitude;

        relativePosition = relativePosition * scalingFactor;

        destinationPlanet.transform.position = relativePosition;

        destinationPlanet.transform.localScale = new Vector3(scalingFactor, scalingFactor, scalingFactor / 10);

        destinationPlanet.transform.LookAt(mainCamera.transform);

        skySphere.transform.LookAt(destinationPlanet.transform.position);
    }

    public void SetMovement(Vector3 movement)
    {
        movementVector = movement;
    }
}
