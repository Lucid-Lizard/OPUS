using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> Children;
    public GameObject ItemParent;

    public void SpawnItem(ItemClass Item, Vector2 SpawnPos, Vector2 Velocity)
    {
        GameObject NewItem = new GameObject();
        NewItem.name = Item.ItemName;
        NewItem.AddComponent<SpriteRenderer>();
        NewItem.GetComponent<SpriteRenderer>().sprite = Item.ItemSprite;
        NewItem.AddComponent<Rigidbody2D>();
        NewItem.AddComponent<BoxCollider2D>();
        NewItem.AddComponent<ItemCode>();
        NewItem.GetComponent<ItemCode>().itemClass = Item;
        NewItem.GetComponent<ItemCode>().This = NewItem;
        NewItem.transform.position = new Vector2(SpawnPos.x + 0.5f, SpawnPos.y + 0.5f);
        NewItem.transform.localScale = new Vector2(0.7f, 0.7f);
        NewItem.transform.parent = ItemParent.transform;
        NewItem.GetComponent<Rigidbody2D>().velocity = Velocity;
        NewItem.layer = 7;
    }

}
