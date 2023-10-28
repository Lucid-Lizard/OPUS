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
    public ItemClass Selected = null;
    public Vector3Int MousePos;
    public GameObject Cursor;
    public Sprite CursorTexture;

    public TileClass[] SemiSolids;

    public ItemClass[] InventorySlots;
    public int SelectedSlot;

    private void Start()
    {
        
        
    }

    private void Update()
    {
        InventorySlots = GameManager.Instance.inventoryManager.InventorySlots;
        SelectedSlot = GameManager.Instance.inventoryManager.SelectedSlot;
        MousePos = new Vector3Int(Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).y), 0);

        
            
        if(Input.GetMouseButton(0))
        {          
            if (InventorySlots[SelectedSlot] != null)
                {

                if (InventorySlots[SelectedSlot].Placeable)
                {
                    if (GameManager.Instance.tileEditManager.CanPlace)
                    {
                        if (InventorySlots[SelectedSlot].ItemTile.multiTile == false)
                        {
                            if (!InventorySlots[SelectedSlot].ItemTile.Wall)
                            {
                                if (!GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
                                {

                                    if (InventorySlots[SelectedSlot].ItemTile.Semisolid == true)
                                    {
                                        GameManager.Instance.tileEditManager.PlaceTile(InventorySlots[SelectedSlot].ItemTile, MousePos.x, MousePos.y);
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

                                        GameManager.Instance.inventoryManager.RemoveItem(InventorySlots[SelectedSlot], SelectedSlot);

                                    }

                                    else if (InventorySlots[SelectedSlot].ItemTile.Slope)
                                    {
                                        GameManager.Instance.tileEditManager.PlaceTile(InventorySlots[SelectedSlot].ItemTile, MousePos.x, MousePos.y);
                                        GameObject SlopeCol = new GameObject();
                                        SlopeCol.name = "PlatformCol";
                                        SlopeCol.AddComponent<SlopeHandler>();
                                        SlopeCol.AddComponent<EdgeCollider2D>();
                                        SlopeCol.GetComponent<SlopeHandler>().storedPos = new Vector2(MousePos.x, MousePos.y);
                                        SlopeCol.GetComponent<SlopeHandler>().Handler = SlopeCol;
                                        SlopeCol.GetComponent<EdgeCollider2D>().sharedMaterial = SlopeFM;

                                        SlopeCol.GetComponent<EdgeCollider2D>().tag = "Ground";

                                        SlopeCol.transform.position = MousePos;

                                        GameManager.Instance.inventoryManager.RemoveItem(InventorySlots[SelectedSlot], SelectedSlot);
                                    }

                                    else if (InventorySlots[SelectedSlot].ItemTile.Tree)
                                    {
                                        GameManager.Instance.tileEditManager.PlaceTile(InventorySlots[SelectedSlot].ItemTile, MousePos.x, MousePos.y);

                                        GameManager.Instance.inventoryManager.RemoveItem(InventorySlots[SelectedSlot], SelectedSlot);
                                    }

                                    else
                                    {
                                        GameManager.Instance.tileEditManager.PlaceTile(InventorySlots[SelectedSlot].ItemTile, MousePos.x, MousePos.y);

                                        GameManager.Instance.inventoryManager.RemoveItem(InventorySlots[SelectedSlot], SelectedSlot);
                                    }



                                }
                            }
                            else
                            {
                                if (!GameManager.Instance.tileEditManager.worldWalls.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
                                {
                                    GameManager.Instance.tileEditManager.PlaceTile(InventorySlots[SelectedSlot].ItemTile, MousePos.x, MousePos.y);

                                    GameManager.Instance.inventoryManager.RemoveItem(InventorySlots[SelectedSlot], SelectedSlot);
                                }
                            }
                        }
                        else
                        {

                            Vector2[] checkPos = new Vector2[(int)(InventorySlots[SelectedSlot].ItemTile.multiTileSize.x * InventorySlots[SelectedSlot].ItemTile.multiTileSize.y)];
                            Vector2[] sendPos = new Vector2[(int)(InventorySlots[SelectedSlot].ItemTile.multiTileSize.x * InventorySlots[SelectedSlot].ItemTile.multiTileSize.y)];
                            int index = 0;
                            for (int i = 0; i < InventorySlots[SelectedSlot].ItemTile.multiTileSize.x; i++)
                            {
                                for (int j = 0; j < InventorySlots[SelectedSlot].ItemTile.multiTileSize.y; j++)
                                {
                                    checkPos[index] = new Vector2(i, j);

                                    index++;
                                }
                            }
                            bool canPlace = true;
                            foreach (Vector2 pos in checkPos)
                            {
                                if (GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y) + pos))
                                {
                                    canPlace = false;
                                }
                            }
                            if (canPlace)
                            {
                                foreach (Vector2 pos in checkPos)
                                {
                                    Vector3Int posToV3 = new Vector3Int((int)pos.x, (int)pos.y, 0);

                                    GameManager.Instance.tileEditManager.PlaceTile(InventorySlots[SelectedSlot].ItemTile, MousePos.x + posToV3.x, MousePos.y + posToV3.y);
                                }

                                GameObject multiTile = new GameObject();
                                multiTile.name = InventorySlots[SelectedSlot].ItemTile.tileName;
                                multiTile.transform.position = new Vector2(MousePos.x, MousePos.y) + new Vector2(InventorySlots[SelectedSlot].ItemTile.multiTileSize.x / 2, InventorySlots[SelectedSlot].ItemTile.multiTileSize.y / 2);

                                index = 0;
                                for (int i = 0; i < InventorySlots[SelectedSlot].ItemTile.multiTileSize.x; i++)
                                {
                                    for (int j = 0; j < InventorySlots[SelectedSlot].ItemTile.multiTileSize.y; j++)
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
                                multiTile.GetComponent<MultiTileHandler>().Item = InventorySlots[SelectedSlot].ItemTile.tileItem;
                                multiTile.GetComponent<SpriteRenderer>().sprite = InventorySlots[SelectedSlot].ItemTile.tileSprite;
                            }

                            GameManager.Instance.inventoryManager.RemoveItem(InventorySlots[SelectedSlot], SelectedSlot);
                        }
                    }
                }

                else if (InventorySlots[SelectedSlot].CanBreak)
                {
                        if (InventorySlots[SelectedSlot].BreakType != "Hammer")
                        {
                            if (GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
                            {
                                Debug.Log(InventorySlots[SelectedSlot].BreakType);
                                GameManager.Instance.tileEditManager.RemoveTile(MousePos.x, MousePos.y, InventorySlots[SelectedSlot].BreakType);

                            }
                        }
                        else
                        {

                        }

                        if (GameManager.Instance.tileEditManager.worldWalls.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
                        {

                            GameManager.Instance.tileEditManager.RemoveTile(MousePos.x, MousePos.y, InventorySlots[SelectedSlot].BreakType);

                        }
                    

                }
            }


            

            
            
        }/* else if (Input.GetMouseButtonDown(1))
        {
            if (GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                
                Selected = GameManager.Instance.tileEditManager.worldTiles[new Vector2(MousePos.x, MousePos.y)];

                Cursor.GetComponent<SpriteRenderer>().sprite = Selected.tileSprite;
            }
            else if (GameManager.Instance.tileEditManager.worldWalls.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                Selected = GameManager.Instance.tileEditManager.worldWalls[new Vector2(MousePos.x, MousePos.y)];
                Cursor.GetComponent<SpriteRenderer>().sprite = null;
            }else
            {
                Selected = null;
                Cursor.GetComponent<SpriteRenderer>().sprite = null;
            }




        }*/
    }
}