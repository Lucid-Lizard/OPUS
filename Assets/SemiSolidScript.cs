using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SemiSolidScript : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetKey(KeyCode.Space) || Mathf.Abs(Player.GetComponent<Rigidbody2D>().velocity.y) >= .25f)
        {
            GetComponent<TilemapCollider2D>().enabled = false;
        } else
        {
            GetComponent<TilemapCollider2D>().enabled = true;
        }
    }
}
