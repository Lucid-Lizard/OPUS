using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    public int textureSize = 500;
    public int numWhitePixels = 100;
    public int minDistance = 15;
    public Texture2D genTex;
    public Texture2D bruh;
    void Start()
    {
        
        // Create a new texture
        Texture2D genTex = new Texture2D(textureSize, textureSize);

        // Set the background color to black
        Color[] pixels = new Color[textureSize * textureSize];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.black;
        }

        // Generate random white pixels
        for (int i = 0; i < numWhitePixels; i++)
        {
            Vector2Int randomPos = RandomPositionWithinRadius(textureSize, minDistance);
            int index = randomPos.x + randomPos.y * textureSize;
            pixels[index] = Color.white;
        }

        // Apply the pixel data to the texture
        genTex.SetPixels(pixels);
        genTex.Apply();

        bruh = genTex;

        // Assign the texture to a material or object

    }

    // Helper function to get a random position within a certain radius
    Vector2Int RandomPositionWithinRadius(int textureSize, int radius)
    {
        Vector2Int randomPos;
        do
        {
            randomPos = new Vector2Int(Random.Range(0, textureSize), Random.Range(0, textureSize));
        } while (Vector2Int.Distance(randomPos, new Vector2Int(textureSize / 2, textureSize / 2)) < radius);
        return randomPos;
    }
}