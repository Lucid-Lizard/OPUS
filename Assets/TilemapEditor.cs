using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEditor : MonoBehaviour
{
    public PhysicsMaterial2D SlopeFM;
    public GameObject Player;
    private Tilemap SelectedLayer;
    public Camera cam;
    public TileClass Selected = null;
    public Vector3Int MousePos;
    public GameObject Cursor;
    public Sprite CursorTexture;

    public TileClass[] SemiSolids;
    
    private void Update()
    {
        MousePos = new Vector3Int(Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).y), 0);
        Cursor.transform.position = MousePos + (Vector3.one / 2);
        if(Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                if (GameManager.Instance.tileEditManager.worldTiles[new Vector2(MousePos.x, MousePos.y)].Semisolid)
                {
                    SelectedLayer = GameManager.Instance.tileEditManager.semiSolidMap;
                } else if (GameManager.Instance.tileEditManager.worldTiles[new Vector2(MousePos.x, MousePos.y)].Tree)
                {
                    SelectedLayer = GameManager.Instance.tileEditManager.treeMap;
                } else
                {
                    SelectedLayer = GameManager.Instance.tileEditManager.tileMap;
                }
            } else if (GameManager.Instance.tileEditManager.worldWalls.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                SelectedLayer = GameManager.Instance.tileEditManager.wallMap;
            } else
            {
                if(Selected != null)
                {
                    if (Selected.Wall)
                    {
                        SelectedLayer = GameManager.Instance.tileEditManager.wallMap;
                    }
                    else
                    {
                        if (Selected.Semisolid)
                        {
                            SelectedLayer = GameManager.Instance.tileEditManager.semiSolidMap;
                        }
                        else if (Selected.Tree)
                        {
                            SelectedLayer = GameManager.Instance.tileEditManager.treeMap;
                        }
                        else
                        {
                            SelectedLayer = GameManager.Instance.tileEditManager.tileMap;
                        }
                    }
                } else
                {
                    SelectedLayer = GameManager.Instance.tileEditManager.tileMap;
                }
                
            }
            Debug.Log(SelectedLayer.name);
        }
        if(Input.GetMouseButton(0))
        {
            if(Selected != null)
            {
                if(Selected.multiTile == false)
                {
                    if (!Selected.Wall)
                    {
                        if (!GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
                        {
                            
                            if (Selected.tileName == "Sand")
                            {
                                GameManager.Instance.tileEditManager.PlaceTileRect(Selected, new Vector2(MousePos.x, MousePos.y), new Vector2(5,5), GameManager.Instance.tileEditManager.tileMap, false);
                            }
                            if (Selected.Semisolid == true)
                            {
                                GameManager.Instance.tileEditManager.PlaceTile(Selected, MousePos.x, MousePos.y);
                                GameObject PlatformCol = new GameObject();
                                PlatformCol.name = "PlatformCol";
                                PlatformCol.AddComponent<SemiSolidScript>();
                                PlatformCol.AddComponent<BoxCollider2D>();
                                PlatformCol.GetComponent<SemiSolidScript>().Player = Player;
                                PlatformCol.GetComponent<SemiSolidScript>().storedPos = new Vector2(MousePos.x, MousePos.y);
                                PlatformCol.GetComponent<SemiSolidScript>().Handler = PlatformCol;
                                PlatformCol.GetComponent<BoxCollider2D>().size = new Vector2(1, 0.01f);
                                PlatformCol.GetComponent<BoxCollider2D>().tag = "Ground";

                                PlatformCol.transform.position = MousePos + (Vector3.one * 0.5f) + new Vector3(0, 0.5f, 0);

                            }

                            else if (Selected.Slope)
                            {
                                GameManager.Instance.tileEditManager.PlaceTile(Selected, MousePos.x, MousePos.y);
                                GameObject SlopeCol = new GameObject();
                                SlopeCol.name = "PlatformCol";
                                SlopeCol.AddComponent<SlopeHandler>();
                                SlopeCol.AddComponent<EdgeCollider2D>();
                                SlopeCol.GetComponent<SlopeHandler>().storedPos = new Vector2(MousePos.x, MousePos.y);
                                SlopeCol.GetComponent<SlopeHandler>().Handler = SlopeCol;
                                SlopeCol.GetComponent<EdgeCollider2D>().sharedMaterial = SlopeFM;

                                SlopeCol.GetComponent<EdgeCollider2D>().tag = "Ground";

                                SlopeCol.transform.position = MousePos;
                            }

                            else if (Selected.Tree)
                            {
                                GameManager.Instance.tileEditManager.PlaceTile(Selected, MousePos.x, MousePos.y);
                            }

                            else
                            {
                                GameManager.Instance.tileEditManager.PlaceTile(Selected, MousePos.x, MousePos.y);
                            }

                        }
                    }
                    else
                    {
                        if (!GameManager.Instance.tileEditManager.worldWalls.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
                        {
                            GameManager.Instance.tileEditManager.PlaceTile(Selected, MousePos.x, MousePos.y);


                        }
                    }
                } else
                {
                    Vector2[] checkPos = new Vector2[(int)(Selected.multiTileSize.x * Selected.multiTileSize.y)];
                    Vector2[] sendPos = new Vector2[(int)(Selected.multiTileSize.x * Selected.multiTileSize.y)];
                    int index = 0;
                    for(int i = 0; i < Selected.multiTileSize.x; i++)
                    {
                        for (int j = 0; j < Selected.multiTileSize.y; j++)
                        {
                            checkPos[index] = new Vector2(i, j);
                            
                            index++;
                        }
                    }
                    bool canPlace = true;
                    foreach(Vector2 pos in checkPos)
                    {
                        if(GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y) + pos))
                        {
                            canPlace = false;
                        }
                    }
                    if(canPlace)
                    {
                        foreach(Vector2 pos in checkPos)
                        {
                            Vector3Int posToV3 = new Vector3Int((int)pos.x, (int)pos.y, 0);

                            GameManager.Instance.tileEditManager.PlaceTile(Selected, MousePos.x + posToV3.x, MousePos.y + posToV3.y);                          
                        }

                        GameObject multiTile = new GameObject();
                        multiTile.name = Selected.tileName;
                        multiTile.transform.position = new Vector2(MousePos.x, MousePos.y) + new Vector2(Selected.multiTileSize.x / 2, Selected.multiTileSize.y / 2);

                        index = 0;
                        for (int i = 0; i < Selected.multiTileSize.x; i++)
                        {
                            for (int j = 0; j < Selected.multiTileSize.y; j++)
                            {
                                sendPos[index] = new Vector2(i, j) + new Vector2(MousePos.x, MousePos.y);

                                index++;
                            }
                        }

                        multiTile.AddComponent<MultiTileHandler>();
                        multiTile.AddComponent<SpriteRenderer>();
                        multiTile.GetComponent<MultiTileHandler>().storedPositions = sendPos;
                        multiTile.GetComponent<MultiTileHandler>().tileMap = GameManager.Instance.tileEditManager.tileMap;
                        multiTile.GetComponent<MultiTileHandler>().multiGO = multiTile;
                        multiTile.GetComponent<SpriteRenderer>().sprite = Selected.tileSprite;
                    }

                    
                }
                
            } else
            {
                //GameManager.Instance.tileEditManager.RemoveTile(MousePos.x, MousePos.y);
                if(SelectedLayer != GameManager.Instance.tileEditManager.wallMap)
                {

                    GameManager.Instance.tileEditManager.worldTiles.Remove(new Vector2(MousePos.x, MousePos.y));
                    
                    SelectedLayer.SetTile(MousePos, null);
                } else
                {

                    GameManager.Instance.tileEditManager.worldWalls.Remove(new Vector2(MousePos.x, MousePos.y));
                    GameManager.Instance.tileEditManager.wallMap.SetTile(MousePos, null);
                }
                
            }
            
                
        } else if (Input.GetMouseButtonDown(1))
        {
            if (GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                
                Selected = GameManager.Instance.tileEditManager.worldTiles[new Vector2(MousePos.x, MousePos.y)];

                Cursor.GetComponent<SpriteRenderer>().sprite = Selected.tileSprite;
            }
            else if (GameManager.Instance.tileEditManager.worldWalls.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                Selected = GameManager.Instance.tileEditManager.worldWalls[new Vector2(MousePos.x, MousePos.y)];
                Cursor.GetComponent<SpriteRenderer>().sprite = CursorTexture;
            }else
            {
                Selected = null;
                Cursor.GetComponent<SpriteRenderer>().sprite = CursorTexture;
            }




        }
    }
}