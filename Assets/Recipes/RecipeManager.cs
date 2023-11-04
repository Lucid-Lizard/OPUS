using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeManager : MonoBehaviour
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

    public GameObject CraftingBar;
    public bool ShowCrafting = false;

    public ItemClass[] InventorySlots;
    public GameObject[] InventoryBars;
    public GameObject[] InventoryBarsQuant;
    public GameObject[] InventoryBarsRend;
    public int[] InventorySlotQuant;
    public Sprite[] Text;
    public RecipeClass[] Recipes;

    public int SelectedSlot;

    public RecipeAtlas RecipeAtlas;

    public void Start()
    {
        SelectedSlot = 0;
        InventorySlots = new ItemClass[15];
        InventoryBars = new GameObject[15];
        InventoryBarsQuant = new GameObject[15 * 3];
        InventoryBarsRend = new GameObject[15];
        InventorySlotQuant = new int[15];
        Text = GameManager.Instance.inventoryManager.Text;
        

        for(int i = 0; i < 15; i++)
        {
            if(i < 5)
            {
                GameObject NewSlot = new GameObject();
                NewSlot.transform.parent = CraftingBar.transform;
                NewSlot.transform.position = new Vector3(-15.5f, -9.25f, 0) + new Vector3(0, -2 * (i), 0);
                if(i == 0)
                    NewSlot.transform.localScale = new Vector2(1, 1);
                if (i == 1)
                    NewSlot.transform.localScale = new Vector2(1.5f, 1.5f);
                if (i == 2)
                    NewSlot.transform.localScale = new Vector2(2, 2);
                if (i == 3)
                    NewSlot.transform.localScale = new Vector2(1.5f, 1.5f);
                if (i == 4)
                    NewSlot.transform.localScale = new Vector2(1, 1);
                NewSlot.name = "Slot " + i.ToString();
                NewSlot.AddComponent<SpriteRenderer>();
                NewSlot.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.inventoryManager.HotbarSprite;
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
            } else
            {
                GameObject NewSlot = new GameObject();
                NewSlot.transform.parent = CraftingBar.transform;
                NewSlot.transform.position = new Vector3(-14.5f, -9.25f, 0) + new Vector3(2 * (i - 4), -3.5f, 0);
                NewSlot.transform.localScale = new Vector2(2, 2);
                NewSlot.name = "Slot " + i.ToString();
                NewSlot.AddComponent<SpriteRenderer>();
                NewSlot.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.inventoryManager.HotbarSprite;
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
                InventoryBarsQuant.SetValue(SlotNum, (i * 3));

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


        }

        for (int s = 0; s < InventoryBars.Length; s++)
        {
            ShowSlot(false, InventoryBars[s], 0);
        }
    }

    public void Update()
    {
        
        if(GameManager.Instance.inventoryManager.ShowInventory)
        {
            int Doables = 0;
            ShowCrafting = true;
            foreach(RecipeClass Recipie in RecipeAtlas.Recipes)
            {
                if (CheckCanCraft(Recipie))
                {
                    Doables++;
                }
            }
            Recipes = new RecipeClass[Doables];
            Doables = 0;
            foreach (RecipeClass Recipie in RecipeAtlas.Recipes)
            {
                if (CheckCanCraft(Recipie))
                {
                    Recipes[Doables] = Recipie;
                    Doables++;
                }
            }

        }
        for (int s = 0; s < InventoryBars.Length; s++)
        {
            ShowSlot(GameManager.Instance.inventoryManager.ShowInventory, InventoryBars[s], 0);
        }

        if (Input.mousePosition.x < Screen.width / 2 && Input.mousePosition.y < (Screen.height / 4) * 3 && Input.mousePosition.y > (Screen.height / 4) * 2)
        {
            if (Input.mouseScrollDelta.y != 0)
            {

                
                SelectedSlot -= (int)Input.mouseScrollDelta.y;

                
            }
            if(Recipes.Length > 0)
            {
                if(SelectedSlot > Recipes.Length - 1)
                {
                    SelectedSlot = 0;
                }
                if (SelectedSlot < 0)
                {
                    SelectedSlot = Recipes.Length - 1;
                }
            }



        }
        if(GameManager.Instance.inventoryManager.ShowInventory)
        {
            for (int i = -2; i < 3; i++)
            {
                int slotToRend = SelectedSlot + i;
                if (slotToRend > Recipes.Length - 1)
                {
                    slotToRend %= Recipes.Length;
                }
                if (slotToRend < 0)
                {
                    slotToRend = Recipes.Length + slotToRend;
                }
                InventoryBarsRend[2 + i].GetComponent<SpriteRenderer>().sprite = Recipes[slotToRend].Result.ItemSprite;
                UpdateText(Recipes[slotToRend].ResultQuant, slotToRend);
            }
            for(int j = 0; j < 10; j++)
            {
                if (j < Recipes[SelectedSlot].Ingredients.Length)
                {
                    InventoryBarsRend[j + 5].GetComponent<SpriteRenderer>().sprite = Recipes[SelectedSlot].Ingredients[j].ItemSprite;
                    UpdateText(Recipes[SelectedSlot].Quants[j], j + 5);
                    int totalOfIngredient = 0;
                    if (GameManager.Instance.inventoryManager.InventorySlots.Contains<ItemClass>(Recipes[SelectedSlot].Ingredients[j]))
                    {
                        List<int> Indices = new List<int>();
                        for(int i = 0; i < GameManager.Instance.inventoryManager.InventorySlots.Length; i++)
                        {
                            if(GameManager.Instance.inventoryManager.InventorySlots[i] == Recipes[SelectedSlot].Ingredients[j])
                            {
                                Indices.Add(i);
                            }
                        }
                        
                        foreach(int index in Indices)
                        {
                            totalOfIngredient += GameManager.Instance.inventoryManager.InventorySlotQuant[index];
                        }
                    }
                    if(totalOfIngredient >= Recipes[SelectedSlot].Quants[j])
                    {
                        InventoryBarsRend[j + 5].GetComponent<SpriteRenderer>().color = Color.white;
                    } else
                    {
                        InventoryBarsRend[j + 5].GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
                else
                {
                    InventoryBarsRend[j + 5].GetComponent<SpriteRenderer>().sprite = null;
                    UpdateText(0, j + 5);
                }
            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                if(CanCraftRecipe())
                {
                    Craft();
                }
            }
        }
        
    }

    public bool CanCraftRecipe()
    {
        int[] totalsOfIngredients = new int[Recipes[SelectedSlot].Ingredients.Length];
        for (int j = 0; j < Recipes[SelectedSlot].Ingredients.Length; j++)
        {
            int totalOfIngredient = 0;
            if (GameManager.Instance.inventoryManager.InventorySlots.Contains<ItemClass>(Recipes[SelectedSlot].Ingredients[j]))
            {
                List<int> Indices = new List<int>();
                for (int i = 0; i < GameManager.Instance.inventoryManager.InventorySlots.Length; i++)
                {
                    if (GameManager.Instance.inventoryManager.InventorySlots[i] == Recipes[SelectedSlot].Ingredients[j])
                    {
                        Indices.Add(i);
                    }
                }

                foreach (int index in Indices)
                {
                    totalOfIngredient += GameManager.Instance.inventoryManager.InventorySlotQuant[index];
                }
            }
            totalsOfIngredients[j] = totalOfIngredient;
        }
        bool canCraft = true;
        for(int k = 0; k < totalsOfIngredients.Length; k++)
        {
            if(totalsOfIngredients[k] < Recipes[SelectedSlot].Quants[k])
            {
                canCraft = false;
            }
        }
        return canCraft;
    }

    public void Craft()
    {
        for (int i = 0; i < Recipes[SelectedSlot].Quants.Length; i++) {
            int leftToCraft = Recipes[SelectedSlot].Quants.Length;
            for (int j = 0; j < GameManager.Instance.inventoryManager.InventorySlots.Length; j++)
            {
                if (GameManager.Instance.inventoryManager.InventorySlots[j] == Recipes[SelectedSlot].Ingredients[i])
                {
                    if (leftToCraft != 0)
                    {
                        if (GameManager.Instance.inventoryManager.InventorySlotQuant[j] >= Recipes[SelectedSlot].Quants[i])
                        {
                            GameManager.Instance.inventoryManager.RemoveItem(Recipes[SelectedSlot].Ingredients[i], j, Recipes[SelectedSlot].Quants[i]);
                            leftToCraft -= Recipes[SelectedSlot].Quants[i];
                        }
                        else
                        {
                            int howMuch = Recipes[SelectedSlot].Quants[i] - GameManager.Instance.inventoryManager.InventorySlotQuant[j];
                            GameManager.Instance.inventoryManager.RemoveItem(Recipes[SelectedSlot].Ingredients[i], j, howMuch);
                            leftToCraft -= howMuch;
                        }
                    }
                }
            }
            
        }
        GameManager.Instance.inventoryManager.AddItem(Recipes[SelectedSlot].Result, quant: Recipes[SelectedSlot].ResultQuant);

    }

    public void ShowSlot(bool Show, GameObject Slot, int Min)
    {
        if (System.Array.IndexOf(InventoryBars, Slot) >= Min)
        {
            Slot.SetActive(Show);
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
    public bool CheckCanCraft(RecipeClass recipe)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2Int origin = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
        for (int y = (5 - 1) / -2; y <= (5 - 1) / 2; y++)
        {
            for (int x = (5 - 1) / -2; x <= (5 - 1) / 2; x++)
            {
                if (GameManager.Instance.tileEditManager.inside_circle(origin, new Vector2(origin.x + x, origin.y + y), 5 / 2))
                {
                    if(GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(origin.x + x, origin.y + y)))
                    {
                        if(GameManager.Instance.tileEditManager.worldTiles[new Vector2(origin.x + x, origin.y + y)].Crafting)
                        {
                            if (GameManager.Instance.tileEditManager.worldTiles[new Vector2(origin.x + x, origin.y + y)].craftingType == recipe.RecipeType)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        if (recipe.RecipeType == TileClass.CraftingType.Anywhere)
            return true;
        return false;
    }

    public bool CheckNearCrafting()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2Int origin = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
        for (int y = (5 - 1) / -2; y <= (5 - 1) / 2; y++)
        {
            for (int x = (5 - 1) / -2; x <= (5 - 1) / 2; x++)
            {
                if (GameManager.Instance.tileEditManager.inside_circle(origin, new Vector2(origin.x + x, origin.y + y), 5 / 2))
                {
                    if (GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(origin.x + x, origin.y + y)))
                    {
                        if (GameManager.Instance.tileEditManager.worldTiles[new Vector2(origin.x + x, origin.y + y)].Crafting)
                        {
                           
                            
                           return true;
                           
                        }
                    }
                }
            }
        }
        return false;
    }
}
