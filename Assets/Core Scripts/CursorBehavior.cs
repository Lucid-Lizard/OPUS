using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.tileEditManager.CanPlace = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int MousePos = new Vector3Int(Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y), 0);

        transform.position = new Vector3(MousePos.x + 0.5f, MousePos.y + 0.5f, 10);
        if(GameManager.Instance.tileEditManager.CanPlace == false)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        } else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
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
