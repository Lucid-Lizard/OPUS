using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TreeHandler : MonoBehaviour
{
    public GameObject This;
    public Vector2 origin;
    public List<Vector2> trackedTiles;


    public bool startTrack = false;

    // Update is called once per frame

    private void Update()
    {
        if (startTrack)
        {
            for(int i = 0; i < trackedTiles.Count; i++)
            {
                if (!GameManager.Instance.tileEditManager.worldTiles.ContainsKey(trackedTiles[i]))
                {
                    trackedTiles[i] = new Vector2(-420, -420);
                }
            }
            if (Input.GetMouseButton(0) && GameManager.Instance.inventoryManager.InventorySlots[GameManager.Instance.inventoryManager.SelectedSlot] != null)
            {
                
                if (GameManager.Instance.inventoryManager.InventorySlots[GameManager.Instance.inventoryManager.SelectedSlot].BreakType == "Axe")
                {
                    
                    
                        
                    Vector3Int MousePos = new Vector3Int(Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y), 0);



                    if (trackedTiles.Contains(new Vector2(MousePos.x, MousePos.y)))
                    {
                        
                            
                            
                            BreakTree(new Vector2(MousePos.x, MousePos.y));
                            
                        
                    }
                }
            }
            bool Kill = true;
            for(int i = 0; i < trackedTiles.Count; i++)
            {
                if (trackedTiles[i] != new Vector2(-420,-420))
                {
                    
                    Kill = false;
                }
            }
            if(Kill)
            {
                Destroy(This);
            }
        }
    }

    public void BreakTree(Vector2 breakOrigin)
    {
        Debug.Log("Start break tree");
            if (breakOrigin.x == origin.x)
            {
            Debug.Log(breakOrigin.x == origin.x);
                foreach (Vector2 pos in trackedTiles)
                {
                Debug.Log(pos);
                    if (pos != null && pos != new Vector2(-420, -420))
                    {
                    Debug.Log(pos != null && pos != new Vector2(-420, -420));

                        if (pos.y >= breakOrigin.y)
                        {
                        Debug.Log(pos.y >= breakOrigin.y);
                            GameManager.Instance.tileEditManager.RemoveTile((int)pos.x, (int)pos.y, "Axe");
                        Debug.Log("removed Tile");

                        }
                    }
                }


            }
        
    }
}
