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
    public bool IsSwapping = false;
    public bool DoSwap = false;
    public int SwapFirstSlot = 0;
    public int SwapLastSlot = 0;
    public int SwapFirstQuant = 0;
    public int SwapLastQuant = 0;
    public ItemClass SwapFirstItem = null;
    public ItemClass SwapLastItem = null;
    public void Update()
    {
        Vector3Int MousePos = new Vector3Int(Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y), 0);
        if (Input.anyKey)
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

            if(Input.GetKey(KeyCode.Q))
            {
                

                if (InventorySlots[SelectedSlot] != null)
                {
                    for(int i = 1; i <= InventorySlotQuant[SelectedSlot]; i++)
                    {
                        GameManager.Instance.itemManager.SpawnItem(InventorySlots[SelectedSlot], new Vector2(MousePos.x, MousePos.y), new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)));
                        RemoveItem(InventorySlots[SelectedSlot], SelectedSlot);
                    }
                }
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            
            SwapFirstItem = InventorySlots[SelectedSlot];
            SwapFirstSlot = SelectedSlot;
            SwapFirstQuant = InventorySlotQuant[SelectedSlot];
            Selector.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            
                
                SwapLastItem = InventorySlots[SelectedSlot];
                SwapLastSlot = SelectedSlot;
                SwapLastQuant = InventorySlotQuant[SelectedSlot];
                Selector.GetComponent<SpriteRenderer>().color = Color.white;
                SwapItems();
            
        }
        if (Input.mousePosition.x < Screen.width / 2 && Input.mousePosition.y > (Screen.height / 4 ) * 3|| !ShowInventory)
        {
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
        }
        if (!ShowInventory && SelectedSlot >= 8)
        {
            SelectedSlot = SelectedSlot % 9;
        }
        
        Selector.transform.position = InventoryBars[SelectedSlot].transform.position;
        if (InventorySlots[SelectedSlot] != null)
        {
            if (InventorySlots[SelectedSlot].name == "Bomb" && Input.GetMouseButtonDown(0))
            {
                
                GameManager.Instance.tileEditManager.RemoveCircle(new Vector2Int(MousePos.x, MousePos.y), 7, true);
                RemoveItem(InventorySlots[SelectedSlot], SelectedSlot);
            }

            else if(InventorySlots[SelectedSlot].name == "Manashroom" && Input.GetMouseButtonDown(0))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity +=  new Vector2(0,25f);
                RemoveItem(InventorySlots[SelectedSlot], SelectedSlot);
            }
        }

        
    }
    public void SwapItems()
    {
        
        InventorySlots[SwapFirstSlot] = SwapLastItem;
        InventorySlotQuant[SwapFirstSlot] = SwapLastQuant;
        if(SwapLastItem != null)
            InventoryBarsRend[SwapFirstSlot].GetComponent<SpriteRenderer>().sprite = SwapLastItem.ItemSprite;
        else
            InventoryBarsRend[SwapFirstSlot].GetComponent<SpriteRenderer>().sprite = null;
        UpdateText(SwapLastQuant, SwapFirstSlot);
        InventorySlots[SwapLastSlot] = SwapFirstItem;
        InventorySlotQuant[SwapLastSlot] = SwapFirstQuant;
        if (SwapFirstItem != null)
            InventoryBarsRend[SwapLastSlot].GetComponent<SpriteRenderer>().sprite = SwapFirstItem.ItemSprite;
        else
            InventoryBarsRend[SwapLastSlot].GetComponent<SpriteRenderer>().sprite = null;
        UpdateText(SwapFirstQuant, SwapLastSlot);
    }
    public void ShowSlot(bool Show, GameObject Slot, int Min)
    {
        if (System.Array.IndexOf(InventoryBars, Slot) >= Min )
        {
            Slot.SetActive(Show);
        }
    }
    public void AddItem(ItemClass Item, int SpecifiedSlot = -420, int quant = 1)
    {
        if(SpecifiedSlot == -420)
        {
            if (FindSlot(Item) != -420)
            {
                int SlotID = FindSlot(Item);
                InventorySlots[SlotID] = Item;
                InventoryBarsRend[SlotID].GetComponent<SpriteRenderer>().sprite = Item.ItemSprite;
                InventorySlotQuant[SlotID] += quant;
                if (InventorySlotQuant[SlotID] < 1000)
                {
                    UpdateText(InventorySlotQuant[SlotID], SlotID);
                }
            }
        } else
        {
            InventorySlots[SpecifiedSlot] = Item;
            InventoryBarsRend[SpecifiedSlot].GetComponent<SpriteRenderer>().sprite = Item.ItemSprite;
            InventorySlotQuant[SpecifiedSlot] += quant;
            if (InventorySlotQuant[SpecifiedSlot] < 1000)
            {
                UpdateText(InventorySlotQuant[SpecifiedSlot], SpecifiedSlot);
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

    public void RemoveItem(ItemClass Item, int Slot, int quant = 1)
    {
        if(InventorySlotQuant[Slot] != 0)
        {
            InventorySlotQuant[Slot] -= quant;
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
