using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileEditManager : MonoBehaviour
{
    public IDictionary<Vector2, TileClass> worldTiles = new Dictionary<Vector2, TileClass>();
    public IDictionary<Vector2, TileClass> worldWalls = new Dictionary<Vector2, TileClass>();

    public GameObject Player;

    public Tilemap tileMap;
    public Tilemap semiSolidMap;
    public Tilemap treeMap;
    public Tilemap wallMap;

    public void PlaceTileRect(TileClass Tile, Vector2 Origin, Vector2 Size, Tilemap tileMap, bool Fill = true, bool Override = true, bool FillAir = false)
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int((int)Origin.x + x, (int)Origin.y + y, 0);

                // Check if Fill is true or if the position is on the border
                if (Fill || x == 0 || x == Size.x - 1 || y == 0 || y == Size.y - 1)
                {
                    if (Override)
                    {
                        if (Tile.Wall)
                        {
                            worldWalls.Remove(new Vector2(tilePosition.x, tilePosition.y));
                            wallMap.SetTile(tilePosition, null);
                        }
                        else
                        {
                            worldTiles.Remove(new Vector2(tilePosition.x, tilePosition.y));

                            semiSolidMap.SetTile(tilePosition, null);

                            treeMap.SetTile(tilePosition, null);

                            tileMap.SetTile(tilePosition, null);

                        }

                    }

                    

                    if (tileMap.GetTile(tilePosition) == null)
                    {
                        if(Tile == null)
                        {
                            PlaceTile(null, (int)Origin.x + x, (int)Origin.y + y);
                        } else
                        {
                            PlaceTile(Tile, (int)Origin.x + x, (int)Origin.y + y);
                        }
                        
                    }
                } else
                {
                    if (FillAir)
                    {
                        if (x != 0 || x != Size.x - 1 || y != 0 || y != Size.y - 1)
                        {
                            if (Tile.Wall)
                            {
                                worldWalls.Remove(new Vector2(tilePosition.x, tilePosition.y));
                                wallMap.SetTile(tilePosition, null);
                            }
                            else
                            {
                                worldTiles.Remove(new Vector2(tilePosition.x, tilePosition.y));

                                semiSolidMap.SetTile(tilePosition, null);

                                treeMap.SetTile(tilePosition, null);

                                tileMap.SetTile(tilePosition, null);

                            }
                        }
                    }
                }
            }
        }
    }

    

    public void PlaceTile(TileClass Tile, int x, int y)
    {
        
        
            if (Tile != null)
            {
                if(Tile.Wall)
                {
                    
                    if (!worldWalls.ContainsKey(new Vector2(x, y)))
                    {

                        wallMap.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);
                        worldWalls.Add(new Vector2(x, y), Tile);
                    }
                } else
                {
                    
                    if (!worldTiles.ContainsKey(new Vector2(x, y)))
                    {
                        if (Tile.Semisolid)
                        {
                            
                            worldTiles.Add(new Vector2(x, y), Tile);
                            semiSolidMap.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);
                        }
                        else if (Tile.Tree)
                        {
                            
                            worldTiles.Add(new Vector2(x, y), Tile);
                            treeMap.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);
                        } else
                        {
                            worldTiles.Add(new Vector2(x, y), Tile);
                            tileMap.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);

                        }
                    }
                }                
            }
        
    }

    public void RemoveTile(int x, int y)
    {
        TileClass Tile;
        if (worldTiles.ContainsKey(new Vector2(x, y)))
        {
            Tile = worldTiles[new Vector2(x, y)];
        } else if (worldWalls.ContainsKey(new Vector2(x, y)))
        {
            Tile = worldWalls[new Vector2(x, y)];
        } else
        {
            Tile = null;
        }

        if(Tile != null)
        {
            if (Tile.tileItem != null)
            {
                GameObject NewItem = new GameObject();
                NewItem.name = Tile.tileItem.ItemName;
                NewItem.AddComponent<SpriteRenderer>();
                NewItem.GetComponent<SpriteRenderer>().sprite = Tile.tileItem.ItemSprite;
                NewItem.AddComponent<Rigidbody2D>();
                NewItem.AddComponent<BoxCollider2D>();
                NewItem.AddComponent<ItemCode>();
                NewItem.GetComponent<ItemCode>().itemClass = Tile.tileItem;
                NewItem.GetComponent<ItemCode>().This = NewItem;
                NewItem.transform.position = new Vector2(x + 0.5f, y + 0.5f);
                NewItem.transform.localScale = new Vector2(0.7f,0.7f);
            }
        }
        
        
        if (Tile != null)
        {
            if (Tile.Wall)
            {
                worldWalls.Remove(new Vector2(x, y));
                wallMap.SetTile(new Vector3Int(x, y, 0), null);
            }
            else
            {
                if (Tile.Semisolid)
                {
                    worldTiles.Remove(new Vector2(x, y));
                    semiSolidMap.SetTile(new Vector3Int(x, y, 0), null);
                }
                else if (Tile.Tree)
                {
                    worldTiles.Remove(new Vector2(x, y));
                    treeMap.SetTile(new Vector3Int(x, y, 0), null);
                }
                else
                {
                    worldTiles.Remove(new Vector2(x, y));
                    tileMap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
    }
}
