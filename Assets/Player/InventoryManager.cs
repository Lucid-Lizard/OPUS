using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public GameObject Player;
    public int InventorySize = 9;
    public GameObject Hotbar;
    public Sprite HotbarSprite;
    public ItemClass[] InventorySlots;
    public GameObject[] InventoryBars;
    public GameObject[] InventoryBarsQuant;
    public GameObject[] InventoryBarsRend;
    public int[] InventorySlotQuant;
    public Sprite[] Text;

    public int SelectedSlot;
    public GameObject Selector;

    public ItemClass[] StartingGear;
    
    // Start is called before the first frame update
    public void Start()
    {
        SelectedSlot = 0;
        InventorySlots = new ItemClass[InventorySize];
        InventoryBars = new GameObject[InventorySize];
        InventoryBarsQuant = new GameObject[InventorySize];
        InventoryBarsRend = new GameObject[InventorySize];
        InventorySlotQuant = new int[InventorySize];

        

        for (int i = 0; i < InventoryBars.Length; i++)
        {
            Debug.Log(i);
            GameObject NewSlot = new GameObject();
            NewSlot.transform.parent = Hotbar.transform;
            NewSlot.transform.position = new Vector3(-15.5f, 9.25f, 0) + new Vector3(i * 2, 0,0);
            NewSlot.transform.localScale = new Vector2(2, 2);
            NewSlot.name = "Slot " + i.ToString();
            NewSlot.AddComponent<SpriteRenderer>();
            NewSlot.GetComponent<SpriteRenderer>().sprite = HotbarSprite;
            NewSlot.GetComponent<SpriteRenderer>().sortingOrder = 100;
            GameObject NewSlotSpriteThing = new GameObject();
            NewSlotSpriteThing.transform.parent = NewSlot.transform;
            NewSlotSpriteThing.transform.position = NewSlot.transform.position;
            
            NewSlotSpriteThing.name = "Sub Slot " + i.ToString();
            NewSlotSpriteThing.AddComponent<SpriteRenderer>();
            NewSlotSpriteThing.GetComponent<SpriteRenderer>().sortingOrder = 101;

            GameObject SlotNum = new GameObject();
            SlotNum.transform.parent = NewSlot.transform;
            SlotNum.transform.position = NewSlot.transform.position + new Vector3(.75f, -.75f);
            SlotNum.transform.localScale = new Vector2(0.25f,0.25f);
            SlotNum.name = "Slot Text " + i.ToString();
            SlotNum.AddComponent<SpriteRenderer>();
            SlotNum.GetComponent<SpriteRenderer>().sprite = Text[0];
            SlotNum.GetComponent<SpriteRenderer>().sortingOrder = 102;

            InventoryBars.SetValue(NewSlot, i);
            InventoryBarsRend.SetValue(NewSlotSpriteThing, i);
            InventoryBarsQuant.SetValue(SlotNum, i);
            InventorySlots.SetValue(null, i);
            InventorySlotQuant.SetValue(0, i);
        }
        foreach (ItemClass item in StartingGear)
        {
            AddItem(item);
        }
    }

    public void Update()
    {
        if(Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectedSlot = 0;
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectedSlot = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectedSlot = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SelectedSlot = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SelectedSlot = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SelectedSlot = 5;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SelectedSlot = 6;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SelectedSlot = 7;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                SelectedSlot = 8;
            } else
            {
                SelectedSlot = SelectedSlot;
            }

        }
        Selector.transform.position = InventoryBars[SelectedSlot].transform.position - new Vector3(0, 1.5f, 0);
    }
    public void AddItem(ItemClass Item)
    {
        if(FindSlot(Item) != 420)
        {
            int SlotID = FindSlot(Item);
            InventorySlots[SlotID] = Item;
            InventoryBarsRend[SlotID].GetComponent<SpriteRenderer>().sprite = Item.ItemSprite;
            InventorySlotQuant[SlotID] += 1;
            if(InventorySlotQuant[SlotID] < 10)
            {
                InventoryBarsQuant[SlotID].GetComponent<SpriteRenderer>().sprite = Text[InventorySlotQuant[SlotID]];
            }
        }
    }

    public int FindSlot(ItemClass Item)
    {
        if (InventorySlots.Contains<ItemClass>(null) || InventorySlots.Contains<ItemClass>(Item))
        {
            for (int Slot = 0; Slot < InventorySlots.Length; Slot++)
            {
                Debug.Log(Slot);
                Debug.Log(InventorySlots.Length);
                if (InventorySlots[Slot] == null || InventorySlots[Slot] == Item)
                {
                    Debug.Log("Slot " + Slot + " Is available");
                    return Slot;
                }
            }
        } 
        
        Debug.Log("No Slot Is available");
        return 420;
    }

    public void RemoveItem(ItemClass Item, int Slot)
    {
        if(InventorySlotQuant[Slot] != 0)
        {
            InventorySlotQuant[Slot] -= 1;
            if (InventorySlotQuant[Slot] <= 9)
            {
                InventoryBarsQuant[Slot].GetComponent<SpriteRenderer>().sprite = Text[InventorySlotQuant[Slot]];
            }


            if (InventorySlotQuant[Slot] == 0)
            {
                InventorySlots[Slot] = null;
                InventoryBarsRend[Slot].GetComponent<SpriteRenderer>().sprite = null;
            }
            
        } 
    }
}
