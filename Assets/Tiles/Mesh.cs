using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateTileMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateTileMesh()
    {
        UnityEngine.Mesh mesh = new UnityEngine.Mesh();

        int width = 10;
        int height = 10;
        float tileSize = 5;

        Vector3[] vertices = new Vector3[4 * (width * height)];
        Vector2[] uv = new Vector2[4 * (width * height)];
        int[] triangles = new int[6 * (width * height)];

        for (int i = 0; i < width; width++)
        {
            for (int j = 0; j < height; height++)
            {
                int index = i * height + j;

                
                    vertices[index * 4 + 0] = new Vector3(tileSize * i, tileSize * j);
                    vertices[index * 4 + 1] = new Vector3(tileSize * i, tileSize * (j + 1));
                    vertices[index * 4 + 2] = new Vector3(tileSize * (i + 1), tileSize * (j + 1));
                    vertices[index * 4 + 3] = new Vector3(tileSize * (i + 1), tileSize * j);

                    uv[index * 4 + 0] = new Vector2(0, 0);
                    uv[index * 4 + 1] = new Vector2(0, 1);
                    uv[index * 4 + 2] = new Vector2(1, 1);
                    uv[index * 4 + 3] = new Vector2(1, 0);

                    triangles[index * 6 + 0] = index * 4 + 0;
                    triangles[index * 6 + 1] = index * 4 + 1;
                    triangles[index * 6 + 2] = index * 4 + 2;

                    triangles[index * 6 + 3] = index * 4 + 0;
                    triangles[index * 6 + 4] = index * 4 + 2;
                    triangles[index * 6 + 5] = index * 4 + 3;

                
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
