using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MultiTileHandler : MonoBehaviour
{
    public GameObject multiGO;
    public Vector2[] storedPositions;
    public ItemClass Item;
    
    public Tilemap tileMap;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool doBreak = false;
            foreach (Vector2 pos in storedPositions)
            {
                if (!GameManager.Instance.tileEditManager.worldTiles.ContainsKey(pos))
                {
                    doBreak = true;
                }
            }
            Debug.Log(doBreak);

            if (doBreak)
            {
                foreach (Vector2 pos in storedPositions)
                {
                    if (GameManager.Instance.tileEditManager.worldTiles.ContainsKey(pos))
                    {
                        GameManager.Instance.tileEditManager.worldTiles.Remove(pos);
                        tileMap.SetTile(new Vector3Int((int)pos.x, (int)pos.y, 0), null);
                    }
                }

                Destroy(multiGO);

            }
        }
    }
}
