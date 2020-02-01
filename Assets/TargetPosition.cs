using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosition : MonoBehaviour
{
    public GameObject targetObject;

    private Vector3 startPosition = new Vector3(0, 0, 0);
    public Vector3 movementVector = new Vector3(0, 0, .1f);
    private Vector3 currentPositon = new Vector3(0, 0, 0);
    public Vector3 targetStartingPosition = new Vector3(0, 0, 0);

    Dictionary<GameObject, Quaternion> leaks = new Dictionary<GameObject, Quaternion>();

    // Start is called before the first frame update
    void Start()
    {
        targetStartingPosition = targetObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Quaternion rotation in leaks.Values)
        {
            movementVector = Quaternion.Lerp(Quaternion.identity, rotation, Time.deltaTime) * movementVector;
        }

        currentPositon += movementVector;

        gameObject.transform.position = currentPositon;

        gameObject.transform.forward = movementVector;

        Vector3 relativePosition = gameObject.transform.InverseTransformPoint(targetStartingPosition);

        float scalingFactor = targetStartingPosition.magnitude / relativePosition.magnitude;

        relativePosition = relativePosition * scalingFactor;

        targetObject.transform.position = relativePosition;

        targetObject.transform.localScale = new Vector3(scalingFactor, scalingFactor, scalingFactor);
    }

    public void AddLeak(GameObject leakId, Quaternion leakDirection)
    {
        leaks.Add(leakId, leakDirection);
    }

    public void RemoveLeak(GameObject leakId)
    {
        leaks.Remove(leakId);
    }
}
