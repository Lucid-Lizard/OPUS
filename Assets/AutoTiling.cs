using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AutoTiling : MonoBehaviour
{
    public GameObject Tile;

    public string TileType;

    public int randomTileSprite;

    private TerrainGeneration terrainGeneration;

    private SpriteRenderer spriteRenderer; // Add a reference to the SpriteRenderer component

    //Dictionary<string, string>

    public string TileCode;

    public string CTNCode;

    public Dictionary<string, string> CTN = new Dictionary<string, string>();

    public IDictionary<Vector2, GameObject> worldTiles;

    public List<string> Siblings = new List<string>();

    private void Start()
    {
        InitiateCTN();
        TerrainGeneration terrainGen = GameManager.Instance.terrainGeneration;

    }

    public void AddSiblings(List<string> TileSiblings)
    {
        foreach (string Tile in TileSiblings)
        {
            Siblings.Add(Tile);
        }
    }

    public string GetTileCode(string Input, int Frame)
    {
        
        string result = CTN[Input];
        if (CTN.ContainsKey(Input))
        {

            int intResult = Int32.Parse(result);
            intResult += randomTileSprite * 12;
            intResult += 144 * Frame;
            result = intResult.ToString();
            if (result.Length == 1)
            {
                result = "00" + result;
            }
            else if (result.Length == 2)
            {
                result = "0" + result;
            }
            else
            {
                result = result;
            }
            return result;
        }
        return result;
    }

    private void Update()
    {

        TileCode = CheckTile(Tile.transform.position, Siblings);
        if(Tile.name == "Dirt")
        {
            if (CheckAdjFor(Tile.transform.position, "Stone") != "00000000")
            {
                CTNCode = GetTileCode(TileCode, 1);
            }
            else if (CheckAdjFor(Tile.transform.position, null) != "111111111")
            {
                CTNCode = GetTileCode(TileCode, 2);
            }
             else
            {
                CTNCode = GetTileCode(TileCode, 0);
            }

        } else if (Tile.name == "Permafrost")
        {
               
                if (CheckAdjFor(Tile.transform.position, "Stone") != "00000000")
                {
                    CTNCode = GetTileCode(TileCode, 1);
                }
                else if (CheckAdjFor(Tile.transform.position, null) != "111111111")
                {
                    CTNCode = GetTileCode(TileCode, 2);
                }
                else
                {
                    CTNCode = GetTileCode(TileCode, 0);
                }

        }
        else
        {
            CTNCode = GetTileCode(TileCode, 0);
        }
        
        Sprite sp = Resources.Load<Sprite>("Tiles/" + TileType + "/tile" + CTNCode);
        Tile.GetComponent<SpriteRenderer>().sprite = sp;

    }


    public void SpriteWork()
    {
        
    }

    public string CheckTile(Vector2 Location, List<string> Siblings)
    {

        string Code = "";
        TerrainGeneration terrainGeneration = GameManager.Instance.terrainGeneration;


        Vector2[] adjacentPositions =
        {
            new Vector2(Location.x - 1, Location.y),
            new Vector2(Location.x - 1, Location.y - 1),
            new Vector2(Location.x, Location.y - 1),
            new Vector2(Location.x + 1, Location.y - 1),
            new Vector2(Location.x + 1, Location.y),
            new Vector2(Location.x + 1, Location.y + 1),
            new Vector2(Location.x, Location.y + 1),
            new Vector2(Location.x - 1, Location.y + 1)
        };

        foreach (Vector2 position in adjacentPositions)
        {
            string AddCode = null;
            if (worldTiles.ContainsKey(position))
            {
                GameObject CheckedTile = worldTiles[position];
                if (CheckedTile == null)
                {
                    AddCode = "0";
                } else if (CheckedTile.GetComponent<AutoTiling>() == null)
                {
                    AddCode = "0";
                }
                else if (CheckedTile.GetComponent<AutoTiling>().TileType == TileType || Siblings.Contains(CheckedTile.GetComponent<AutoTiling>().TileType))
                {
                    AddCode = "1";
                }
                else if (CheckedTile != null)
                {
                    AddCode = "0";
                }
                else
                {
                    AddCode = "0";
                }

            }
            else
            {
                AddCode = "0";
            }

            Code += AddCode;
        }
        return Code;
    }

    public string CheckAdjFor(Vector2 Location, string ForTile)
    {
        
        string Code = "";
        TerrainGeneration terrainGeneration = GameManager.Instance.terrainGeneration;

        Vector2[] adjacentPositions =
        {
            new Vector2(Location.x - 1, Location.y),
            new Vector2(Location.x - 1, Location.y - 1),
            new Vector2(Location.x, Location.y - 1),
            new Vector2(Location.x + 1, Location.y - 1),
            new Vector2(Location.x + 1, Location.y),
            new Vector2(Location.x + 1, Location.y + 1),
            new Vector2(Location.x, Location.y + 1),
            new Vector2(Location.x - 1, Location.y + 1)

        };

        foreach (Vector2 position in adjacentPositions)
        {
            string AddCode = null;
            if (worldTiles.ContainsKey(position))
            {
                GameObject CheckedTile = worldTiles[position];
                if (CheckedTile.GetComponent<AutoTiling>() == null)
                {
                    AddCode = "0";
                }
                else if (CheckedTile.GetComponent<AutoTiling>().TileType == ForTile)
                {
                    AddCode = "1";
                } 
                else
                {
                    AddCode = "0";
                }

            }
            else
            {
                AddCode = "0";
            }

            Code += AddCode;
        }
        return Code;
    }

    public void InitiateCTN()
    {
        CTN.Add("11111111", "082");
        CTN.Add("11111110", "041");
        CTN.Add("11111101", "010");
        CTN.Add("11111100", "010");
        CTN.Add("11111011", "042");
        CTN.Add("11111010", "009");
        CTN.Add("11111001", "010");
        CTN.Add("11111000", "010");
        CTN.Add("11110111", "083");
        CTN.Add("11110110", "043");
        CTN.Add("11110101", "011");
        CTN.Add("11110100", "011");
        CTN.Add("11110011", "083");
        CTN.Add("11110010", "043");
        CTN.Add("11110001", "011");
        CTN.Add("11110000", "011");
        CTN.Add("11101111", "078");
        CTN.Add("11101110", "045");
        CTN.Add("11101101", "006");
        CTN.Add("11101100", "006");
        CTN.Add("11101011", "047");
        CTN.Add("11101010", "112");
        CTN.Add("11101001", "006");
        CTN.Add("11101000", "006");
        CTN.Add("11100111", "083");
        CTN.Add("11100110", "043");
        CTN.Add("11100101", "011");
        CTN.Add("11100100", "011");
        CTN.Add("11100011", "083");
        CTN.Add("11100010", "043");
        CTN.Add("11100001", "011");
        CTN.Add("11100000", "011");
        CTN.Add("11011111", "117");
        CTN.Add("11011110", "113");
        CTN.Add("11011101", "109");
        CTN.Add("11011100", "109");
        CTN.Add("11011011", "114");
        CTN.Add("11011010", "073");
        CTN.Add("11011001", "109");
        CTN.Add("11011000", "109");
        CTN.Add("11010111", "119");
        CTN.Add("11010110", "074");
        CTN.Add("11010101", "110");
        CTN.Add("11010100", "110");
        CTN.Add("11010011", "074");
        CTN.Add("11010010", "074");
        CTN.Add("11010001", "110");
        CTN.Add("11010000", "110");
        CTN.Add("11001111", "117");
        CTN.Add("11001110", "113");
        CTN.Add("11001101", "109");
        CTN.Add("11001100", "109");
        CTN.Add("11001011", "114");
        CTN.Add("11001010", "073");
        CTN.Add("11001001", "109");
        CTN.Add("11001000", "109");
        CTN.Add("11000111", "119");
        CTN.Add("11000110", "074");
        CTN.Add("11000101", "110");
        CTN.Add("11000100", "110");
        CTN.Add("11000011", "119");
        CTN.Add("11000010", "074");
        CTN.Add("11000001", "110");
        CTN.Add("11000000", "110");
        CTN.Add("10111111", "077");
        CTN.Add("10111110", "080");
        CTN.Add("10111101", "005");
        CTN.Add("10111100", "005");
        CTN.Add("10111011", "081");
        CTN.Add("10111010", "115");
        CTN.Add("10111001", "005");
        CTN.Add("10111000", "005");
        CTN.Add("10110111", "079");
        CTN.Add("10110110", "038");
        CTN.Add("10110101", "002");
        CTN.Add("10110100", "002");
        CTN.Add("10110011", "079");
        CTN.Add("10110010", "038");
        CTN.Add("10110001", "002");
        CTN.Add("10110000", "002");
        CTN.Add("10101111", "118");
        CTN.Add("10101110", "115");
        CTN.Add("10101101", "001");
        CTN.Add("10101100", "001");
        CTN.Add("10101011", "004");
        CTN.Add("10101010", "037");
        CTN.Add("10101001", "001");
        CTN.Add("10101000", "001");
        CTN.Add("10100111", "079");
        CTN.Add("10100110", "038");
        CTN.Add("10100101", "002");
        CTN.Add("10100100", "002");
        CTN.Add("10100011", "079");
        CTN.Add("10100010", "038");
        CTN.Add("10100001", "002");
        CTN.Add("10100000", "002");
        CTN.Add("10011111", "117");
        CTN.Add("10011110", "113");
        CTN.Add("10011101", "109");
        CTN.Add("10011100", "109");
        CTN.Add("10011011", "114");
        CTN.Add("10011010", "073");
        CTN.Add("10011001", "109");
        CTN.Add("10011000", "109");
        CTN.Add("10010111", "119");
        CTN.Add("10010110", "074");
        CTN.Add("10010101", "110");
        CTN.Add("10010100", "110");
        CTN.Add("10010011", "119");
        CTN.Add("10010010", "074");
        CTN.Add("10010001", "110");
        CTN.Add("10010000", "110");
        CTN.Add("10001111", "117");
        CTN.Add("10001110", "113");
        CTN.Add("10001101", "109");
        CTN.Add("10001100", "109");
        CTN.Add("10001011", "114");
        CTN.Add("10001010", "073");
        CTN.Add("10001001", "109");
        CTN.Add("10001000", "109");
        CTN.Add("10000111", "119");
        CTN.Add("10000110", "074");
        CTN.Add("10000101", "110");
        CTN.Add("10000100", "110");
        CTN.Add("10000011", "119");
        CTN.Add("10000010", "074");
        CTN.Add("10000001", "110");
        CTN.Add("10000000", "110");
        CTN.Add("01111111", "044");
        CTN.Add("01111110", "044");
        CTN.Add("01111101", "008");
        CTN.Add("01111100", "008");
        CTN.Add("01111011", "040");
        CTN.Add("01111010", "040");
        CTN.Add("01111001", "008");
        CTN.Add("01111000", "008");
        CTN.Add("01110111", "039");
        CTN.Add("01110110", "039");
        CTN.Add("01110101", "003");
        CTN.Add("01110100", "003");
        CTN.Add("01110011", "039");
        CTN.Add("01110010", "003");
        CTN.Add("01110001", "003");
        CTN.Add("01110000", "003");
        CTN.Add("01101111", "076");
        CTN.Add("01101110", "076");
        CTN.Add("01101101", "000");
        CTN.Add("01101100", "000");
        CTN.Add("01101011", "036");
        CTN.Add("01101010", "036");
        CTN.Add("01101001", "000");
        CTN.Add("01101000", "000");
        CTN.Add("01100111", "039");
        CTN.Add("01100110", "039");
        CTN.Add("01100101", "003");
        CTN.Add("01100100", "003");
        CTN.Add("01100011", "039");
        CTN.Add("01100010", "039");
        CTN.Add("01100001", "003");
        CTN.Add("01100000", "003");
        CTN.Add("01011111", "116");
        CTN.Add("01011110", "116");
        CTN.Add("01011101", "108");
        CTN.Add("01011100", "108");
        CTN.Add("01011011", "072");
        CTN.Add("01011010", "072");
        CTN.Add("01011001", "108");
        CTN.Add("01011000", "108");
        CTN.Add("01010111", "075");
        CTN.Add("01010110", "075");
        CTN.Add("01010101", "046");
        CTN.Add("01010100", "046");
        CTN.Add("01010011", "075");
        CTN.Add("01010010", "075");
        CTN.Add("01010001", "046");
        CTN.Add("01010000", "046");
        CTN.Add("01001111", "116");
        CTN.Add("01001110", "116");
        CTN.Add("01001101", "108");
        CTN.Add("01001100", "108");
        CTN.Add("01001011", "072");
        CTN.Add("01001010", "072");
        CTN.Add("01001001", "108");
        CTN.Add("01001000", "108");
        CTN.Add("01000111", "075");
        CTN.Add("01000110", "075");
        CTN.Add("01000101", "046");
        CTN.Add("01000100", "046");
        CTN.Add("01000011", "075");
        CTN.Add("01000010", "075");
        CTN.Add("01000001", "046");
        CTN.Add("01000000", "046");
        CTN.Add("00111111", "044");
        CTN.Add("00111110", "044");
        CTN.Add("00111101", "008");
        CTN.Add("00111100", "008");
        CTN.Add("00111011", "040");
        CTN.Add("00111010", "040");
        CTN.Add("00111001", "008");
        CTN.Add("00111000", "008");
        CTN.Add("00110111", "039");
        CTN.Add("00110110", "039");
        CTN.Add("00110101", "003");
        CTN.Add("00110100", "003");
        CTN.Add("00110011", "039");
        CTN.Add("00110010", "039");
        CTN.Add("00110001", "003");
        CTN.Add("00110000", "003");
        CTN.Add("00101111", "076");
        CTN.Add("00101110", "076");
        CTN.Add("00101101", "000");
        CTN.Add("00101100", "000");
        CTN.Add("00101011", "036");
        CTN.Add("00101010", "036");
        CTN.Add("00101001", "000");
        CTN.Add("00101000", "000");
        CTN.Add("00100111", "039");
        CTN.Add("00100110", "039");
        CTN.Add("00100101", "003");
        CTN.Add("00100100", "003");
        CTN.Add("00100011", "039");
        CTN.Add("00100010", "039");
        CTN.Add("00100001", "003");
        CTN.Add("00100000", "003");
        CTN.Add("00011111", "116");
        CTN.Add("00011110", "116");
        CTN.Add("00011101", "108");
        CTN.Add("00011100", "108");
        CTN.Add("00011011", "072");
        CTN.Add("00011010", "072");
        CTN.Add("00011001", "108");
        CTN.Add("00011000", "108");
        CTN.Add("00010111", "075");
        CTN.Add("00010110", "075");
        CTN.Add("00010101", "046");
        CTN.Add("00010100", "046");
        CTN.Add("00010011", "075");
        CTN.Add("00010010", "075");
        CTN.Add("00010001", "046");
        CTN.Add("00010000", "046");
        CTN.Add("00001111", "116");
        CTN.Add("00001110", "116");
        CTN.Add("00001101", "108");
        CTN.Add("00001100", "108");
        CTN.Add("00001011", "072");
        CTN.Add("00001010", "072");
        CTN.Add("00001001", "108");
        CTN.Add("00001000", "108");
        CTN.Add("00000111", "075");
        CTN.Add("00000110", "075");
        CTN.Add("00000101", "046");
        CTN.Add("00000100", "046");
        CTN.Add("00000011", "075");
        CTN.Add("00000010", "075");
        CTN.Add("00000001", "046");
        CTN.Add("00000000", "046");
    }
}
