using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketStatus
{
    public List<RocketHole> rocketHoles = new List<RocketHole>();
}

public class RocketHole
{
    public float size;

    public Vector3 position;

    public GameObject gameObject;
    
    public RocketHole(float size_, Vector3 position_, GameObject gameObject_)
    {
        size = size_;
        position = position_;
    }
}
