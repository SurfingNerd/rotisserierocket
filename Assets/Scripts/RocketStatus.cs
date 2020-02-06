using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketStatus
{
    public List<RocketLeak> rocketLeaks = new List<RocketLeak>();


    public void AddLeak(Vector3 position, GameObject rocketHolePrefab, Transform parent, float size )
    {
        GameObject go  = (GameObject)GameObject.Instantiate(rocketHolePrefab, position, Quaternion.identity);
        //go.transform.SetPositionAndRotation(position, correctRotation);
        RocketLeak rocketLeak = go.GetComponent<RocketLeak>();
        rocketLeak.size = 1.0f;

        go.transform.SetParent(parent, true);
        //go.transform.LookAt(new Vector3(0, 0, position.z));
        //Quaternion.LookRotation()
        go.transform.rotation = Quaternion.LookRotation(position - new Vector3(0, 0, position.z), Vector3.forward);
        //RocketHole hole = new RocketHole(1.0f, position, go);
        rocketLeaks.Add(rocketLeak);

        var rootGOs = parent.gameObject.scene.GetRootGameObjects();
        foreach (var item in rootGOs)
            if (item.name == "LevelManager")
            {
                item.GetComponent<LevelManager>().NumLeaks = rocketLeaks.Count;
                Debug.Log("Num leaks: " + rocketLeaks.Count);
            }

                
    }
}

// public class RocketHole
// {
//     public RocketHole(float size_, Vector3 position_, GameObject gameObject_)
//     {
//         size = size_;
//         position = position_;
//     }
// }
