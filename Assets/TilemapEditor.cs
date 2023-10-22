using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEditor : MonoBehaviour
{

    public GameObject TG;
    public Tilemap tileMap;
    public Tilemap semiSolidMap;
    public Tilemap treeMap;
    public Tilemap wallMap;
    private Tilemap SelectedLayer;
    public Camera cam;
    public TileClass Selected = null;
    public Vector3Int MousePos;
    public GameObject Cursor;
    public Sprite CursorTexture;
    public IDictionary<Vector2, TileClass> worldTiles ;
    public IDictionary<Vector2, TileClass> worldWalls ;
    public TileClass[] SemiSolids;
    public void Start()
    {
        worldTiles = TG.GetComponent<TerrainGeneration>().worldTiles;
        worldWalls = TG.GetComponent<TerrainGeneration>().worldWalls;
    }
    private void Update()
    {
        MousePos = new Vector3Int(Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).y), 0);
        Cursor.transform.position = MousePos + (Vector3.one / 2);
        if(Input.GetMouseButtonDown(0))
        {
            if(worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                if (worldTiles[new Vector2(MousePos.x, MousePos.y)].Semisolid)
                {
                    SelectedLayer = semiSolidMap;
                } else if (worldTiles[new Vector2(MousePos.x, MousePos.y)].Tree)
                {
                    SelectedLayer = treeMap;
                } else
                {
                    SelectedLayer = tileMap;
                }
            } else
            {
                SelectedLayer = wallMap;
            }
        }
        if(Input.GetMouseButton(0))
        {
            if(Selected != null)
            {
                if (!Selected.Wall)
                {
                    if (!worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
                    {


                        if (Selected.Semisolid == true)
                        {
                            worldTiles.Add(new Vector2(MousePos.x, MousePos.y), Selected);
                            semiSolidMap.SetTile(MousePos, Selected.ruleTile);

                        }
                        
                        else
                        {
                            worldTiles.Add(new Vector2(MousePos.x, MousePos.y), Selected);
                            tileMap.SetTile(MousePos, Selected.ruleTile);
                        }


                    }
                } else
                {
                    if (!worldWalls.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
                    {
                        worldWalls.Add(new Vector2(MousePos.x, MousePos.y), Selected);
                        wallMap.SetTile(MousePos, Selected.ruleTile);


                    }
                }
            } else
            {
                if(SelectedLayer != wallMap)
                {
                    worldTiles.Remove(new Vector2(MousePos.x, MousePos.y));
                    SelectedLayer.SetTile(MousePos, null);
                } else
                {
                    worldWalls.Remove(new Vector2(MousePos.x, MousePos.y));
                    wallMap.SetTile(MousePos, null);
                }
                
            }
            
                
        } else if (Input.GetMouseButtonDown(1))
        {
            if (worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                Selected = worldTiles[new Vector2(MousePos.x, MousePos.y)];

                Cursor.GetComponent<SpriteRenderer>().sprite = Selected.tileSprite;
            }
            else if (worldWalls.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                Selected = worldWalls[new Vector2(MousePos.x, MousePos.y)];
                Cursor.GetComponent<SpriteRenderer>().sprite = CursorTexture;
            }else
            {
                Selected = null;
                Cursor.GetComponent<SpriteRenderer>().sprite = CursorTexture;
            }




        }
    }
}