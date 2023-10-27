using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSlotOffset : MonoBehaviour
{
    public GameObject Player;
    public Vector2 HotbarOffset;
    public Camera MC;
    public void Update()
    {
        
        transform.position = MC.transform.position + new Vector3(HotbarOffset.x,HotbarOffset.y,420);
    }
}