using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCode : MonoBehaviour
{
    public ItemClass itemClass;
    public GameObject This;
    public GameObject Player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // This object has collided with the Player object
            GameManager.Instance.inventoryManager.AddItem(itemClass);
            Destroy(This);

            // You can perform any actions or logic here when the collision occurs with the Player object.
        }
    }
}
