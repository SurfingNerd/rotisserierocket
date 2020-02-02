using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public Animator camAnimator;
    // Start is called before the first frame update
    void Start()
    {
        camAnimator.SetTrigger("StartCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
