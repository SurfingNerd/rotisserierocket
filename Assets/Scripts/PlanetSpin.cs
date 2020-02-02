using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpin : MonoBehaviour
{
    public Vector3 angularVelocity = new Vector3(1, 0, 0);
    
    float angle;
    
    public Quaternion myRotation = new Quaternion();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(angularVelocity * Time.deltaTime);
    }
}
