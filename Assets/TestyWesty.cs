using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestyWesty : MonoBehaviour
{
    public GameObject Dad;
    public Texture2D Spritea;

    // Start is called before the first frame update
    void Start()
    {
        Dad.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Spritea, new Rect(0, Spritea.height - 56, 56, 56), new Vector2(0,0),56);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
