using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellRenderer : MonoBehaviour
{
    [Header("Settings")]
    public int width;
    public int height;
    public SpriteRenderer spriteRenderer;
    public Vector2 imagePosition;
    public Color defaultColor = Color.black;
    public Texture2D writeTo;

    private List<Color> cellColors = new List<Color>();

    public List<Cell> cells = new List<Cell>();

    void Start()
    {
        InitialRender();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitialRender()
    {
        Color color = defaultColor;

        for (int a = 0; a < height; a++)
        {
            for (int b = 0; b < width; b++)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                    color = defaultColor;
                else
                    color = Color.white;

                cells.Add(new Cell());
                cellColors.Add(color);
            }
        }

        UpdateImage(cellColors.ToArray());
    }

    public void UpdateImage(Color[] colors)
    {
        writeTo.SetPixels(colors, 0);
        writeTo.Apply();

        spriteRenderer.sprite = Sprite.Create(writeTo, new Rect(0f, 0f, writeTo.width, writeTo.height), new Vector2(0f, 0f));
    }
}

public struct Cell
{
    public float supportLevel;

    public int totalPopulation;
    public int supporters;
    public int hostiles;
}
