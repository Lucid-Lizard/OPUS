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

    public IDictionary<Vector2, Color> lightMap = new Dictionary<Vector2, Color>();


    public List<Vector2> PlayerTiles;

    public GameObject Player;

    public Tilemap tileMap;
    public Tilemap semiSolidMap;
    public Tilemap treeMap;
    public Tilemap wallMap;
    public GameObject ItemParent;

    public bool CanPlace;

    public double BreakTime;
    public bool CanBreak;

    


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

    public void DigZigZag(int Length,int Speed, int Drop, int HoleSize, int ZigZaggyNess,int x, int y)
    {
        Vector2 Position = new Vector2(x, y);
        int direction = 1;
        int vdirection = 1;
        if(Random.RandomRange(0, ZigZaggyNess) <= 1)
        {
            direction *= -1;
        }
        
        for(int d = 0; d < Length; d++)
        {
            int scale = Random.Range(0, 2);

            RemoveCircle(Position, HoleSize + scale, false);

            Position.x += Speed * direction + Random.Range(-2,2);
            Position.y -= Drop + Random.Range(-2, 2);

            if (Random.RandomRange(0, ZigZaggyNess) <= 1)
            {
                direction *= -1;
            }
            
        }
    }
    public bool inside_circle(Vector2 center, Vector2 tile, float radius)
    {
        
        float dx = center.x - tile.x;
        float dy = center.y - tile.y;
        float distance = Mathf.Sqrt((dx * dx) + (dy * dy));
        
        return distance <= radius;
    }
    public void RemoveCircle(Vector2 origin, int radius, bool doBreak)
    {
        Debug.Log("Remove Circle");
        Debug.Log(origin);
        Debug.Log(radius);
        for(int y = (radius - 1) / -2; y <= (radius - 1) / 2; y++)
        {
            for (int x = (radius - 1) / -2; x <= (radius - 1) / 2; x++)
            {
                Debug.Log("poop");
                Debug.Log(new Vector2(origin.x + x, origin.y + y));
                if (inside_circle(origin, new Vector2(origin.x + x,origin.y + y), radius / 2))
                {
                    Debug.Log("in range");
                    RemoveTile((int)origin.x + x, (int)origin.y + y, "Pickaxe", doBreak);
                } 
            }
        }
    }
    public void RemoveTile(int x, int y, string SpecType, bool DoDrop = true)
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

        if(worldTiles.ContainsKey(new Vector2(x,y+1))&& worldTiles.ContainsKey(new Vector2(x, y))) {
            if(worldTiles[new Vector2(x,y+1)].Rooted && !worldTiles[new Vector2(x, y )].Rooted)
            {
                return;
            }
        }

        if(worldTiles.ContainsKey(new Vector2(x, y))) 
        { 
            if (SpecType == Tile.TypeToBreak)
            {
                if (worldTiles[new Vector2(x, y)].StartTree)
                {
                    worldTiles.Remove(new Vector2(x, y));
                    tileMap.SetTile(new Vector3Int(x, y, 0), null);
                    for (int tx = -1; tx < 2; tx++)
                    {
                        for (int ty = 0; ty < 2; ty++)
                        {
                            if (worldTiles.ContainsKey(new Vector2(x + tx, y + ty)))
                            {
                                if (worldTiles[new Vector2(x + tx, y + ty)].tree)
                                    RemoveTile(x + tx, y + ty, "Axe");
                            }
                        }
                    }
                }
            }
        }

        if (Tile != null && SpecType == Tile.TypeToBreak)
        {
            if (DoDrop) {
                if (Tile.Items != null)
                {
                    for (int l = 0; l < Tile.Items.Length; l++)
                    {
                        int RandomRandy = Random.Range(0, Tile.ItemChance[l] + 1);
                        if (RandomRandy <= 1)
                        {
                            GameManager.Instance.itemManager.SpawnItem(Tile.Items[l], new Vector2(x, y), new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)));
                        }
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
