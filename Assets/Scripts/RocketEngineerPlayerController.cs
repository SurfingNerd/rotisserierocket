using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEngineerPlayerController : MonoBehaviour
{

    public float MinPositionZ = -50;
    public float MaxPositionZ = 50;

    public float CharacterSpeed = 10; 


    float CurrentRotation = 0;


    public GameObject WorldRootToRotate;

    // Start is called before the first frame update
    void Start()
    {
        if (WorldRootToRotate == null)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
