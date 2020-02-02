using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosition : MonoBehaviour
{
    public LevelManager LvlMgr;

    public Camera mainCamera;
    public GameObject destinationPlanet;
    public GameObject skySphere;
    public float normalizedDistanceToTarget;

    private Vector3 targetStartingPosition = new Vector3(0, 0, 0);
    private Vector3 movementVector = new Vector3(0, 0, 0);
    private Vector3 currentPositon = new Vector3(0, 0, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        targetStartingPosition = destinationPlanet.transform.position;

        SetMovement(new Vector3(0, 0, 0.05f));
    }

    // Update is called once per frame
    void Update()
    {
        return;
        foreach (RocketLeak leak in levelManager.currentRocketStatus.rocketLeaks)
        {
            //Debug.DrawLine(leak.transform.position, leak.transform.position  );
            Quaternion rotation = leak.transform.rotation;

            Vector3 origin = leak.transform.position;
            

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

        normalizedDistanceToTarget = Vector3.Distance(targetStartingPosition, currentPositon) / Vector3.Distance(targetStartingPosition, new Vector3(0,0,0));
    }

    public void SetMovement(Vector3 movement)
    {
        movementVector = movement;
    }
}
