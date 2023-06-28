using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialisationDeprecated : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width;
    public int height;
    public GameObject cellPrefab;
    [Tooltip("The position of the bottom-left corner")]
    public Vector2 startFrom;
    public Color defaultColor = Color.black;

    private List<Color> cellColors = new List<Color>();
    private List<SpriteRenderer> cellRenderers = new List<SpriteRenderer>();

    void Awake()
    {
        if (cellPrefab == null)
            return;

        Vector2 cellPosition = startFrom;

        SpriteRenderer renderer;

        for(int a = 0; a < height; a++)
        {
            for(int  b = 0; b < width; b++)
            {
                renderer = Instantiate(cellPrefab, cellPosition, Quaternion.identity).GetComponent<SpriteRenderer>();

                if (Random.Range(0f, 1f) < 0.5f)
                    renderer.color = defaultColor;
                else
                    renderer.color = Color.white;

                cellRenderers.Add(renderer);
                cellColors.Add(defaultColor);

                cellPosition.x++;
            }

            cellPosition.x = startFrom.x;
            cellPosition.y++;
        }

        OptimiseCells();
    }

    void OptimiseCells()
    {
        List<SpriteRenderer> cellsToGroup = new List<SpriteRenderer>();
        List<Color> cellColorsLocal = new List<Color>(cellColors);

        foreach (Color cell in cellColorsLocal)
        {
            int index = cellColorsLocal.IndexOf(cell);
            cellsToGroup = new List<SpriteRenderer>();


            if (CheckCellColor(index, Color.black))
            {

            }
         
        }

        bool CheckCellColor(int index, Color colorToCheck)
        {
            if (cellColors[index] == colorToCheck)
                return true;

            return false;
        }
    }

    

    public void EditCells()
    {

    }
}
