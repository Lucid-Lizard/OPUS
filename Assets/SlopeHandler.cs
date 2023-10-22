using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SlopeHandler : MonoBehaviour
{

    public Vector2 storedPos;
    public IDictionary<Vector2, TileClass> worldTiles;
    public GameObject Handler;
    // Start is called before the first frame update
    void Start()
    {
        Handler.GetComponent<EdgeCollider2D>().points = new Vector2[] {
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            };
    }

    // Update is called once per frame
    void Update()
    {
        if (worldTiles.ContainsKey(storedPos + new Vector2(1, 0)))
        {
            Handler.GetComponent<EdgeCollider2D>().points = new Vector2[] {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,0),
            };
        }

        else if (worldTiles.ContainsKey(storedPos - new Vector2(1, 0)))
        {
            Handler.GetComponent<EdgeCollider2D>().points = new Vector2[] {
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            };


            
        }

        if (!worldTiles.ContainsKey(storedPos))
        {
            Destroy(Handler);
        }
    }
}
