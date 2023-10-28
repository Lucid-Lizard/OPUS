using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.inventoryManager.InventorySlots[GameManager.Instance.inventoryManager.SelectedSlot] != null)
        {

            GetComponent<SpriteRenderer>().sprite = GameManager.Instance.inventoryManager.InventorySlots[GameManager.Instance.inventoryManager.SelectedSlot].ItemSprite;
            transform.localScale = GameManager.Instance.inventoryManager.InventorySlots[GameManager.Instance.inventoryManager.SelectedSlot].ItemScaleInHand;

        }

        else
            GetComponent<SpriteRenderer>().sprite = null;
    }
}
