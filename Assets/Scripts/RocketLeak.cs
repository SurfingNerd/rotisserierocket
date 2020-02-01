using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLeak : MonoBehaviour
{
    public float size;

    //public Vector3 position;

    //public GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateHighlight()
    {
        Debug.Log("Todo: Activate leak highlight.");
    }

    public void DeactivateHighlight()
    {
        Debug.Log("Todo: Deactivate leak highlight.");
    }

    #region Triggers for Leaks

     //When the Primitive collides with the walls, it will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogWarning("Entered Collider " + other.name);

        if (other.CompareTag("Player"))
        {
            RocketEngineerPlayerController playerController = other.GetComponent<RocketEngineerPlayerController>();

            playerController.NotifyLeakInRegion(this);
        }
    }

    //When the Primitive exits the collision, it will change Color
    private void OnTriggerExit(Collider other)
    {
        //Debug.LogWarning("Exited Collider " + other.name);
        
        if (other.CompareTag("Player"))
        {
            RocketEngineerPlayerController playerController = other.GetComponent<RocketEngineerPlayerController>();

            playerController.NotifyLeakExited(this);
        }
    }

    #endregion


}
