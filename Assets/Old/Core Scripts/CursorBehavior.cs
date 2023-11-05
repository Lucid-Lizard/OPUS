using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{
    public Sprite Default;
    public Sprite[] Breaking;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.tileEditManager.CanPlace = true;
    }
    public float BreakDuration;
    public Vector3 LastPosition;
    // Update is called once per frame
    void Update()
    {
        Vector3Int MousePos = new Vector3Int(Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y), 0);
        
        transform.position = new Vector3(MousePos.x + 0.5f, MousePos.y + 0.5f, 10);

        if(transform.position != LastPosition)
        {
            LastPosition = transform.position;
            GameManager.Instance.tileEditManager.BreakTime = 0;
            GameManager.Instance.tileEditManager.CanBreak = false;
        }


        if(GameManager.Instance.tileEditManager.CanPlace == false)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        } else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (GameManager.Instance.tileEditManager.BreakTime > 0)
        {
            if (GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                BreakDuration = (((60 * GameManager.Instance.tileEditManager.worldTiles[new Vector2(MousePos.x, MousePos.y)].Toughness) / GameManager.Instance.inventoryManager.InventorySlots[GameManager.Instance.inventoryManager.SelectedSlot].BreakSpeed) * 60);
            } else if (GameManager.Instance.tileEditManager.worldWalls.ContainsKey(new Vector2(MousePos.x, MousePos.y)))
            {
                BreakDuration = (((60 * GameManager.Instance.tileEditManager.worldWalls[new Vector2(MousePos.x, MousePos.y)].Toughness) / GameManager.Instance.inventoryManager.InventorySlots[GameManager.Instance.inventoryManager.SelectedSlot].BreakSpeed) * 60);
            }
        }

        if (GameManager.Instance.tileEditManager.BreakTime > 0)
        {
            if(GameManager.Instance.tileEditManager.BreakTime >= (BreakDuration / 6) * 0 && GameManager.Instance.tileEditManager.BreakTime < (BreakDuration / 6) * 1)
                GetComponent<SpriteRenderer>().sprite = Breaking[0];
            else if (GameManager.Instance.tileEditManager.BreakTime >= (BreakDuration / 6) * 1 && GameManager.Instance.tileEditManager.BreakTime < (BreakDuration / 6) * 2)
                GetComponent<SpriteRenderer>().sprite = Breaking[1];
            else if (GameManager.Instance.tileEditManager.BreakTime >= (BreakDuration / 6) * 2 && GameManager.Instance.tileEditManager.BreakTime < (BreakDuration / 6) * 3)
                GetComponent<SpriteRenderer>().sprite = Breaking[2];
            else if (GameManager.Instance.tileEditManager.BreakTime >= (BreakDuration / 6) * 3 && GameManager.Instance.tileEditManager.BreakTime < (BreakDuration / 6) * 4)
                GetComponent<SpriteRenderer>().sprite = Breaking[3];
            else if (GameManager.Instance.tileEditManager.BreakTime >= (BreakDuration / 6) * 4 && GameManager.Instance.tileEditManager.BreakTime < (BreakDuration / 6) * 5)
                GetComponent<SpriteRenderer>().sprite = Breaking[4];
            else if (GameManager.Instance.tileEditManager.BreakTime >= (BreakDuration / 6) * 5)
                GetComponent<SpriteRenderer>().sprite = Breaking[5];
        } else
        {
            GetComponent<SpriteRenderer>().sprite = Default;
        }







    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.tileEditManager.CanPlace = false;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.tileEditManager.CanPlace = true;
        }
    }
}
