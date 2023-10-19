using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public Camera PlayerCamera;
    public GameObject Player;
    public GameObject PlayerHead;
    void Update()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Input.GetAxis("Horizontal") * -4);
        Player.transform.rotation = rotation;
    }
}
