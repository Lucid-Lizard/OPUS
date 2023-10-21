using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEditor : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera cam;
    public TileBase Selected = null;
    public Vector3Int MousePos;
    public GameObject Cursor;
    private void Update()
    {
        MousePos = new Vector3Int(Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition).y), 0);
        Cursor.transform.position = MousePos + (Vector3.one / 2);
        if(Input.GetMouseButtonDown(0))
        {
            
            tilemap.SetTile(MousePos, Selected);
        } else if (Input.GetMouseButtonDown(1))
        {
            Selected = tilemap.GetTile(MousePos);
            
        }
    }
}