using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEditor : MonoBehaviour
{

    public GameObject TG;
    public Tilemap tilemap;
    public Camera cam;
    public TileClass Selected = null;
    public Vector3Int MousePos;
    public GameObject Cursor;
    public Sprite CursorTexture;
    public IDictionary<Vector2, TileClass> worldTiles ;
    public void Start()
    {
        worldTiles = TG.GetComponent<TerrainGeneration>().worldTiles;
    }
    private void Update()
    {
        MousePos = new Vector3Int(Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).y), 0);
        Cursor.transform.position = MousePos + (Vector3.one / 2);
        if(Input.GetMouseButton(0))
        {
            if(Selected != null)
            {
                if (!worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
                {
                    
                    worldTiles.Add(new Vector2(MousePos.x, MousePos.y), Selected);
                    tilemap.SetTile(MousePos, Selected.ruleTile); 
                    
                    
                }
            } else
            {
                worldTiles.Remove(new Vector2(MousePos.x, MousePos.y));
                tilemap.SetTile(MousePos, null);
            }
            
                
        } else if (Input.GetMouseButtonDown(1))
        {
            if (worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                Selected = worldTiles[new Vector2(MousePos.x, MousePos.y)];
                Cursor.GetComponent<SpriteRenderer>().sprite = Selected.tileSprite;
            }
            else
            {
                Selected = null;
                Cursor.GetComponent<SpriteRenderer>().sprite = CursorTexture;
            }
                
                
            
            
        }
    }
}