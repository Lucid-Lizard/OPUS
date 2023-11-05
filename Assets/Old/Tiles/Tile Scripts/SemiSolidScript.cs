using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SemiSolidScript : MonoBehaviour
{
    public GameObject Player;
    public Vector2 storedPos;

    public GameObject Handler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.position.y <= transform.position.y + .7f) {
            GetComponent<BoxCollider2D>().enabled = false;
        } else
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }

        if(Input.GetKey(KeyCode.S))
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }

        if (!GameManager.Instance.tileEditManager.worldTiles.ContainsKey(storedPos))
        {
            Destroy(Handler);
        }
    }
}
