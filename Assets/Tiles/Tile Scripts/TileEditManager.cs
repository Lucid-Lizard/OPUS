using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TileEditManager : MonoBehaviour
{
    public Sprite BreakSprite;
    
    public IDictionary<Vector2, TileClass> worldTiles = new Dictionary<Vector2, TileClass>();
    public IDictionary<Vector2, TileClass> worldWalls = new Dictionary<Vector2, TileClass>();
    public IDictionary<Vector2, GameObject> BreakObjs = new Dictionary<Vector2, GameObject>();
    public IDictionary<Vector2, int> tileBreaks = new Dictionary<Vector2, int>();
    public IDictionary<Vector2, int> wallBreaks = new Dictionary<Vector2, int>();

    public List<Vector2> PlayerTiles;

    public GameObject Player;

    public Tilemap tileMap;
    public Tilemap semiSolidMap;
    public Tilemap treeMap;
    public Tilemap wallMap;
    public GameObject ItemParent;

    public bool CanPlace;
    




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

    public void RemoveTile(int x, int y, string SpecType)
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

        if(worldTiles.ContainsKey(new Vector2(x,y+1))) {
            if(worldTiles[new Vector2(x,y+1)].Tree && !worldTiles[new Vector2(x, y )].Tree)
            {
                return;
            }
        }

        if (Tile != null && SpecType == Tile.TypeToBreak)
        {
            if (Tile.tileItem != null)
            {
                if(Tile.MultipleLoot)
                {
                    for (int d = 0; d < Tile.Loot.Length; d++)
                    {

                        if (Random.Range(0, Tile.Loot[d].DropChance) == 0)
                        {
                            GameManager.Instance.itemManager.SpawnItem(Tile.Loot[d], new Vector2(x, y), new Vector2(Random.Range(-1,1), Random.Range(-1, 1)));
                        }


                    }
                } else
                {
                    GameManager.Instance.itemManager.SpawnItem(Tile.tileItem, new Vector2(x, y), new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)));
                }
                
                
            }
        }

        if (Tile != null)
        {
            if (SpecType == Tile.TypeToBreak && Tile.Wall)
            {
                worldWalls.Remove(new Vector2(x, y));
                wallMap.SetTile(new Vector3Int(x, y, 0), null);
            }
            else
            {
                if (SpecType == Tile.TypeToBreak && Tile.Semisolid)
                {
                    worldTiles.Remove(new Vector2(x, y));
                    semiSolidMap.SetTile(new Vector3Int(x, y, 0), null);
                }
                else if (SpecType == Tile.TypeToBreak && Tile.Tree)
                {
                    worldTiles.Remove(new Vector2(x, y));
                    treeMap.SetTile(new Vector3Int(x, y, 0), null);

                    for(int tx = -1; tx != 2; tx++)
                    {
                        if(worldTiles.ContainsKey(new Vector2(x + tx, y + 1)))
                        {
                            if(worldTiles[new Vector2(x + tx, y + 1)].Tree)
                            {
                                RemoveTile(x + tx, y + 1, "Axe");
                            }
                        }
                    }
                }
                else if (SpecType == Tile.TypeToBreak)
                {
                    worldTiles.Remove(new Vector2(x, y));
                    tileMap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }

    }
}
