using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{

    public List<Vector3> newVertices = new List<Vector3>();
    public List<int> newTriangles = new List<int>();
    public List<Vector2> newUV = new List<Vector2>();

    private UnityEngine.Mesh mesh;

    private float tUnit = 0.25f;
    private Vector2 tStone = new Vector2(0, 1);
    private Vector2 tDirt = new Vector2(0, 2);
    private Vector2 tGrass = new Vector2(0, 3);
    private Vector2 tGrassAutumn = new Vector2(2, 3);
    private Vector2 tGrassSakura = new Vector2(3, 3);
    private Vector2 tLog = new Vector2(1, 3);
    private Vector2 tLogAutumn = new Vector2(1, 1);
    private Vector2 tLeaf = new Vector2(1, 2);
    private Vector2 tLeafAutumn = new Vector2(1, 0);
    private Vector2 tLeafSakura = new Vector2(3, 2);
    private Vector2 tGrass1 = new Vector2(2, 2);
    private Vector2 tGrass2 = new Vector2(2, 1);
    private Vector2 tGrass3 = new Vector2(3, 1);
    private Vector2 tCoal = new Vector2(2, 0);
    private Vector2 tIron = new Vector2(3, 0);

    private int squareCount;



    public byte[,] blocks;
    void GenSquare(int x, int y, Vector2 texture)
    {

        newVertices.Add(new Vector3(x, y, 0));
        newVertices.Add(new Vector3(x + 1, y, 0));
        newVertices.Add(new Vector3(x + 1, y - 1, 0));
        newVertices.Add(new Vector3(x, y - 1, 0));

        newTriangles.Add(squareCount * 4);
        newTriangles.Add((squareCount * 4) + 1);
        newTriangles.Add((squareCount * 4) + 3);
        newTriangles.Add((squareCount * 4) + 1);
        newTriangles.Add((squareCount * 4) + 2);
        newTriangles.Add((squareCount * 4) + 3);

        newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y + tUnit));
        newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
        newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y));
        newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y));

        squareCount++;

    }


    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        squareCount = 0;
        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();

        UnityEngine.Mesh newMesh = new UnityEngine.Mesh();
        newMesh.vertices = colVertices.ToArray();
        newMesh.triangles = colTriangles.ToArray();
        col.sharedMesh = newMesh;

        colVertices.Clear();
        colTriangles.Clear();
        colCount = 0;

    }

    public int treeCoolDown;

    public int startBiome;
    void GenTerrain()
    {
        startBiome = Random.Range(1, 4);
        Debug.Log(startBiome);
        treeCoolDown = 0;
        blocks = new byte[96, 128];
        saplings = new byte[96, 128];

        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            int stone = Noise(px, 0, 80, 15, 1);
            stone += Noise(px, 0, 50, 30, 1);
            stone += Noise(px, 0, 10, 10, 1);
            stone += 75;

            int dirt = Noise(px, 0, 100f, 35, 1);
            dirt += Noise(px, 100, 50, 30, 1);
            dirt += 75;


            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                if (py < stone)
                {
                    blocks[px, py] = 1;

                    //The next three lines make dirt spots in random places
                    if (Noise(px, py, 12, 16, 1) > 10)
                    {
                        blocks[px, py] = 3;

                    }

                    if (Noise(px * 2, py * 2, 12, 15, 1) > 10)
                    {
                        blocks[px, py] = 7;

                    }if (Noise(px * 2 + 100, py * 2 + 100, 14, 15, 1) > 10 && (py < stone - 40))
                    {
                        blocks[px, py] = 8;

                    }

                    //The next three lines remove dirt and rock to make caves in certain places
                    /*if (Noise(px, py * 2, 14, 25, 1) > 10)
                    { //Caves
                        blocks[px, py] = 0;

                    }*/

                }
                else if (py < dirt)
                {
                    blocks[px, py] = 3;
                } else if (py == dirt)
                {
                    if (startBiome == 1)
                        blocks[px, py] = 2;
                    if (startBiome == 2)
                        blocks[px, py] = 9;
                    if (startBiome == 3)
                        blocks[px, py] = 10;
                    if (Random.Range(0, 5) <= 3)
                    {

                        if (startBiome == 1)
                            blocks[px, py + 1] = 6;
                        if (startBiome == 2)
                            blocks[px, py + 1] = 14;
                        if (startBiome == 3)
                            blocks[px, py + 1] = 15;
                    }
                    if (Random.Range(0,5) <= 1 && treeCoolDown <= 0)
                    {
                        saplings[px, py + 1] = 1;
                        treeCoolDown = 5;
                    }
                }


            }

            treeCoolDown--;
        }

        

    }

    public void TreePass()
    {
        for (int px = 0; px < saplings.GetLength(0); px++)
        {
            for (int py = 0; py < saplings.GetLength(1); py++)
            {

                //If the block is not air
                if (saplings[px, py] != 0)
                {

                    GenerateTree(px, py, blocks);
                }//End air block check
            }
        }
    }

    int Noise(int x, int y, float scale, float mag, float exp)
    {

        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));

    }

    
    void BuildMesh()
    {
        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            for (int py = 0; py < blocks.GetLength(1); py++)
            {

                //If the block is not air
                if (blocks[px, py] != 0)
                {

                    // GenCollider here, this will apply it
                    // to every block other than air
                    if (!nonsolids.Contains(blocks[px,py]))
                    {
                        GenCollider(px, py);
                    }
                    if (blocks[px, py] == 1)
                    {
                        GenSquare(px, py, tStone);
                    }
                    else if (blocks[px, py] == 2)
                    {
                        GenSquare(px, py, tGrass);
                    }
                    else if (blocks[px, py] == 3)
                    {
                        GenSquare(px, py, tDirt);
                    }else if (blocks[px, py] == 4)
                    {
                        GenSquare(px, py, tLog);
                    }else if (blocks[px, py] == 5)
                    {
                        GenSquare(px, py, tLeaf);
                    }else if (blocks[px, py] == 6)
                    {
                        GenSquare(px, py, tGrass1);
                    }else if (blocks[px, py] == 7)
                    {
                        GenSquare(px, py, tCoal);
                    }else if (blocks[px, py] == 8)
                    {
                        GenSquare(px, py, tIron);
                    }else if (blocks[px, py] == 9)
                    {
                        GenSquare(px, py, tGrassAutumn);
                    }else if (blocks[px, py] == 10)
                    {
                        GenSquare(px, py, tGrassSakura);
                    }else if (blocks[px, py] == 11)
                    {
                        GenSquare(px, py, tLogAutumn);
                    }else if (blocks[px, py] == 12)
                    {
                        GenSquare(px, py, tLeafAutumn);
                    }else if (blocks[px, py] == 13)
                    {
                        GenSquare(px, py, tLeafSakura);
                    }else if (blocks[px, py] == 14)
                    {
                        GenSquare(px, py, tGrass2);
                    }else if (blocks[px, py] == 15)
                    {
                        GenSquare(px, py, tGrass3);
                    }
                }//End air block check
            }
        }
    }
    
    void safePlace(int x, int y, byte b)
    {
        if(x >= 0 && x < blocks.GetLength(0) && y >= 0 && y < blocks.GetLength(1))
        {
            blocks[x, y] = b;
        }
    }
    public void GenerateTree(int x, int y, byte[,] map)
    {
        byte log = 5;
        byte leaf = 6;

        if (startBiome == 1) {
            log = 4;
            leaf = 5;
        }
        else if (startBiome == 2) {
            log = 11;
            leaf = 12;
            }
        else if (startBiome == 3) {
            log = 4;
            leaf = 13;
            }

        int treeheight = Random.Range(4, 6);
        int height = 0;
        for(int t = 0; t < treeheight; t++)
        {
            map[x, y + t] = log;
            height++;
        }
        
        safePlace(x - 2, y + height + 0,leaf);
        safePlace(x - 2, y + height + 1, leaf);

        safePlace(x - 1, y + height + 0, leaf);
        safePlace(x - 1, y + height + 1, leaf);
        safePlace(x - 1, y + height + 2, leaf);
        safePlace(x - 1, y + height + 3, leaf);

        safePlace(x - 0, y + height + 0, leaf);
        safePlace(x - 0, y + height + 1, leaf);
        safePlace(x - 0, y + height + 2, leaf);
        safePlace(x - 0, y + height + 3, leaf);

        safePlace(x + 1, y + height + 0,leaf);
        safePlace(x + 1, y + height + 1, leaf);
        safePlace(x + 1, y + height + 2, leaf);
        safePlace(x + 1, y + height + 3, leaf);

        safePlace(x + 2, y + height + 0, leaf);
        safePlace(x + 2, y + height + 1, leaf);
        
    }

    public byte[,] saplings;

    public void Start()
    {

        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        GenTerrain();
        
        TreePass();
        BuildMesh();
        UpdateMesh();
    }

    

    public List<Vector3> colVertices = new List<Vector3>();
    public List<int> colTriangles = new List<int>();
    private int colCount;

    private MeshCollider col;

    List<byte> nonsolids = new List<byte>()
    {
        4,
        5,
        6,
        11,
        12,
        13,
        14
    };
    void ColliderTriangles()
    {
        colTriangles.Add(colCount * 4);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 3);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 2);
        colTriangles.Add((colCount * 4) + 3);
    }

    void GenCollider(int x, int y)
    {

        //Top
        if (Block(x, y + 1) == 0 || nonsolids.Contains(Block(x, y + 1)))
        {
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 0));
            colVertices.Add(new Vector3(x, y, 0));

            ColliderTriangles();

            colCount++;
        }

        //bot
        if (Block(x, y - 1) == 0 || nonsolids.Contains(Block(x, y - 1)))
        {
            colVertices.Add(new Vector3(x, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x, y - 1, 1));

            ColliderTriangles();
            colCount++;
        }

        //left
        if (Block(x - 1, y) == 0 || nonsolids.Contains(Block(x - 1, y)))
        {
            colVertices.Add(new Vector3(x, y - 1, 1));
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x, y, 0));
            colVertices.Add(new Vector3(x, y - 1, 0));

            ColliderTriangles();

            colCount++;
        }

        //right
        if (Block(x + 1, y) == 0 || nonsolids.Contains(Block(x + 1, y)))
        {
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y, 0));

            ColliderTriangles();

            colCount++;
        }

    }

    byte Block(int x, int y)
    {

        if (x == -1 || x == blocks.GetLength(0) || y == -1 || y == blocks.GetLength(1))
        {
            return (byte)1;
        }

        return blocks[x, y];
    }

}
