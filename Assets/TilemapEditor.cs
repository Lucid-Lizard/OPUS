using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEditor : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera cam;

    public Vector3Int MousePos;
    private void Update()
    {
        MousePos = new Vector3Int(Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).y), 0);
        if(Input.GetMouseButtonDown(0))
        {
            
            tilemap.SetTile(MousePos, null);
        }
    }
}