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
            if (Tile.Wall)
            {
                if (!worldWalls.ContainsKey(new Vector2(x, y)))
                {
                    wallMap.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);
                    worldWalls.Add(new Vector2(x, y), Tile);
                }
            }
            else
            {

                if (!worldTiles.ContainsKey(new Vector2(x, y)))
                {
                    worldTiles.Add(new Vector2(x, y), Tile);
                    tileMap.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);
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
            if(worldTiles[new Vector2(x,y+1)].Rooted && !worldTiles[new Vector2(x, y )].Rooted)
            {
                return;
            }
        }

        if (Tile != null && SpecType == Tile.TypeToBreak)
        {
            if (Tile.Items != null)
            {
                for(int l = 0; l < Tile.Items.Length; l++)
                {
                    int RandomRandy = Random.Range(0, Tile.ItemChance[l]);
                    if(RandomRandy <= 1)
                    {
                        GameManager.Instance.itemManager.SpawnItem(Tile.Items[l], new Vector2(x, y), new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)));
                    }
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
            else if (SpecType == Tile.TypeToBreak)
            {
                    worldTiles.Remove(new Vector2(x, y));
                    tileMap.SetTile(new Vector3Int(x, y, 0), null);
                
            }
        }
    }
}
