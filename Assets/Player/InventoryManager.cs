using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    IDictionary<char, int> CharInt = new Dictionary<char, int>() {
        { '0',0 }, 
        { '1',1 }, 
        { '2',2 }, 
        { '3',3 }, 
        { '4',4 }, 
        { '5',5 }, 
        { '6',6 }, 
        { '7',7 }, 
        { '8',8 }, 
        { '9',9 }, 
    
    };
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
    public bool ShowInventory;

    public ItemClass[] StartingGear;
    
    // Start is called before the first frame update
    public void Start()
    {
        SelectedSlot = 0;
        InventorySlots = new ItemClass[InventorySize];
        InventoryBars = new GameObject[InventorySize];
        InventoryBarsQuant = new GameObject[InventorySize * 3];
        InventoryBarsRend = new GameObject[InventorySize];
        InventorySlotQuant = new int[InventorySize];

        ShowInventory = false;

        int Row = 1;
        for (int i = 0; i < InventoryBars.Length; i++)
        {
            if (i % 9 == 0)
            {
                Row++;
            }
            Debug.Log(i);
            GameObject NewSlot = new GameObject();
            NewSlot.transform.parent = Hotbar.transform;
            NewSlot.transform.position = new Vector3(-15.5f, 9.25f, 0) + new Vector3((i%9) * 2, -2 * (Row - 1),0);
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
            SlotNum.transform.localScale = new Vector2(0.25f, 0.25f);
            SlotNum.name = "Slot Text a" + i.ToString();
            SlotNum.AddComponent<SpriteRenderer>();
            SlotNum.GetComponent<SpriteRenderer>().sprite = Text[0];
            SlotNum.GetComponent<SpriteRenderer>().sortingOrder = 102;

            InventoryBarsQuant.SetValue(SlotNum, i * 3);

            GameObject SlotNum2 = new GameObject();
            SlotNum2.transform.parent = NewSlot.transform;
            SlotNum2.transform.position = NewSlot.transform.position + new Vector3(.30f, -.75f);
            SlotNum2.transform.localScale = new Vector2(0.25f, 0.25f);
            SlotNum2.name = "Slot Text b" + i.ToString();
            SlotNum2.AddComponent<SpriteRenderer>();
            SlotNum2.GetComponent<SpriteRenderer>().sprite = Text[0];
            SlotNum2.GetComponent<SpriteRenderer>().sortingOrder = 102;

            InventoryBarsQuant.SetValue(SlotNum2, (i * 3) + 1);

            GameObject SlotNum3 = new GameObject();
            SlotNum3.transform.parent = NewSlot.transform;
            SlotNum3.transform.position = NewSlot.transform.position + new Vector3(-0.15f, -.75f);
            SlotNum3.transform.localScale = new Vector2(0.25f, 0.25f);
            SlotNum3.name = "Slot Text b" + i.ToString();
            SlotNum3.AddComponent<SpriteRenderer>();
            SlotNum3.GetComponent<SpriteRenderer>().sprite = Text[0];
            SlotNum3.GetComponent<SpriteRenderer>().sortingOrder = 102;

            InventoryBarsQuant.SetValue(SlotNum3, (i * 3) + 2);

            InventoryBars.SetValue(NewSlot, i);
            InventoryBarsRend.SetValue(NewSlotSpriteThing, i);
            
            InventorySlots.SetValue(null, i);
            InventorySlotQuant.SetValue(0, i);

            UpdateText(0, i);
        }
        for (int s = 0; s < InventoryBars.Length; s++)
        {
            ShowSlot(ShowInventory, InventoryBars[s], 9);
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

            

            if(Input.GetKeyDown(KeyCode.E))
            {
                if(ShowInventory)
                {
                    ShowInventory = false;
                } else
                {
                    ShowInventory = true;
                }
                for (int s = 0; s < InventoryBars.Length; s++)
                {
                    ShowSlot(ShowInventory, InventoryBars[s], 9);
                }
            }
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            if (!ShowInventory)
            {
                if (SelectedSlot - Input.mouseScrollDelta.y >= 0 && SelectedSlot - Input.mouseScrollDelta.y <= 8)
                {
                    SelectedSlot -= (int)Input.mouseScrollDelta.y;
                }
            }
            else
            {
                if (SelectedSlot - Input.mouseScrollDelta.y >= 0 && SelectedSlot - Input.mouseScrollDelta.y <= InventorySize - 1)
                {
                    SelectedSlot -= (int)Input.mouseScrollDelta.y;
                }
            }
        }

        if (!ShowInventory && SelectedSlot >= 8)
        {
            SelectedSlot = SelectedSlot % 9;
        }

        Selector.transform.position = InventoryBars[SelectedSlot].transform.position;
    }

    public void ShowSlot(bool Show, GameObject Slot, int Min)
    {
        if (System.Array.IndexOf(InventoryBars, Slot) >= Min )
        {
            Slot.SetActive(Show);
        }
    }
    public void AddItem(ItemClass Item)
    {
        if(FindSlot(Item) != -420)
        {
            int SlotID = FindSlot(Item);
            InventorySlots[SlotID] = Item;
            InventoryBarsRend[SlotID].GetComponent<SpriteRenderer>().sprite = Item.ItemSprite;
            InventorySlotQuant[SlotID] += 1;
            if(InventorySlotQuant[SlotID] < 1000)
            {
                UpdateText(InventorySlotQuant[SlotID], SlotID);
            }
        }
    }

    public int FindSlot(ItemClass Item)
    {
        if (InventorySlots.Contains<ItemClass>(null) || InventorySlots.Contains<ItemClass>(Item))
        {
            for (int Slot = 0; Slot < InventorySlots.Length; Slot++)
            {

                if (InventorySlots[Slot] == null || InventorySlots[Slot] == Item && InventorySlotQuant[Slot] < Item.MaxStackSize)
                {

                    return Slot;
                }
            }
        } 
        

        return -420;
    }

    public void RemoveItem(ItemClass Item, int Slot)
    {
        if(InventorySlotQuant[Slot] != 0)
        {
            InventorySlotQuant[Slot] -= 1;
            if (InventorySlotQuant[Slot] < 1000)
            {
                UpdateText(InventorySlotQuant[Slot], Slot);
            }


            if (InventorySlotQuant[Slot] == 0)
            {
                InventorySlots[Slot] = null;
                InventoryBarsRend[Slot].GetComponent<SpriteRenderer>().sprite = null;
            }
            
        } 
    }

    public void UpdateText(int num, int SlotID)
    {

        if (num.ToString().Length == 1)
        {
            InventoryBarsQuant[SlotID * 3].GetComponent<SpriteRenderer>().sprite = Text[num];
            InventoryBarsQuant[SlotID * 3 + 1].SetActive(false);
            InventoryBarsQuant[SlotID * 3 + 2].SetActive(false);
        }
        else if (num.ToString().Length == 2)
        {
            
            char[] SplitNum = num.ToString().ToCharArray();
            InventoryBarsQuant[SlotID * 3 + 1].SetActive(true);
            InventoryBarsQuant[SlotID * 3].GetComponent<SpriteRenderer>().sprite = Text[CharInt[SplitNum[1]]];
            InventoryBarsQuant[SlotID * 3 + 1].GetComponent<SpriteRenderer>().sprite = Text[CharInt[SplitNum[0]]];
            InventoryBarsQuant[SlotID * 3 + 2].SetActive(false);
        }
        else if (num.ToString().Length == 3)
        {
            InventoryBarsQuant[SlotID * 3 + 1].SetActive(true);
            InventoryBarsQuant[SlotID * 3 + 2].SetActive(true);
            char[] SplitNum = num.ToString().ToCharArray();
            InventoryBarsQuant[SlotID * 3].GetComponent<SpriteRenderer>().sprite = Text[CharInt[SplitNum[2]]];
            InventoryBarsQuant[SlotID * 3 + 1].GetComponent<SpriteRenderer>().sprite = Text[CharInt[SplitNum[1]]];
            InventoryBarsQuant[SlotID * 3 + 2].GetComponent<SpriteRenderer>().sprite = Text[CharInt[SplitNum[0]]];
        }
    }
}
