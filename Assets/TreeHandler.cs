using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TreeHandler : MonoBehaviour
{
    public Vector2 origin;
    public List<Vector2> trackedTiles;


    public bool startTrack = false;

    // Update is called once per frame

    private void Update()
    {
        if (startTrack)
        {
            if (Input.GetMouseButton(0) && GameManager.Instance.inventoryManager.InventorySlots[GameManager.Instance.inventoryManager.SelectedSlot] != null)
            {
                Debug.Log("Left clicked");
                if (GameManager.Instance.inventoryManager.InventorySlots[GameManager.Instance.inventoryManager.SelectedSlot].CanBreak)
                {
                    Debug.Log("holding pick");
                    
                        
                        Vector3Int MousePos = new Vector3Int(Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y), 0);
                        Debug.Log(MousePos);
                        
                            
                            if (trackedTiles.Contains(new Vector2(MousePos.x, MousePos.y)))
                            {
                                Debug.Log("Tile is being tracked");
                                BreakTree(new Vector2(MousePos.x, MousePos.y));
                            }
                        
                    
                }
            }
        }
    }

    public void BreakTree(Vector2 breakOrigin)
    {

            if (breakOrigin.x == origin.x)
            {

                foreach (Vector2 pos in trackedTiles)
                {
                    if (pos != null && pos != new Vector2(-420, -420))
                    {


                        if (pos.y >= breakOrigin.y)
                        {
                            GameManager.Instance.tileEditManager.RemoveTile((int)pos.x, (int)pos.y);
                            

                        }
                    }
                }


            }
        
    }
}
