using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopMap : MonoBehaviour
{
    public int maxPopulation;
    public int minPopulation;
    [Tooltip("0 = completeley even pop, 1 = all pop in one cell")]
    public float populationConcentration;
    public Color maxColor;
    public Color minColor;

    private CellRenderer cellRenderer;

    void Start()
    {
        cellRenderer = FindObjectOfType<CellRenderer>();
    }

    public void DisplayPopMap()
    {
        GeneratePopMap();

        List<Cell> cells = cellRenderer.cells;
        List<Color> colors = new List<Color>();

        for (int i = 0; i < cells.Count; i++)
        {
            Cell cell = cells[i];

            float colorR = newColor(maxColor.r, minColor.r, cell.totalPopulation);
            float colorG = newColor(maxColor.g, minColor.g, cell.totalPopulation);
            float colorB = newColor(maxColor.b, minColor.b, cell.totalPopulation);

            colors.Add(new Color(colorR, colorG, colorB));
        }

        float newColor(float maxC, float minC, int cellPop)
        {
            float weight = (float)cellPop / (float)maxPopulation;
            float returnValue = ((maxC * weight) + minC);

            if (weight > 0.8f)
            {
                Debug.Log($"Weight:{weight}, Float: {returnValue}");
            }

            return returnValue;
        }

        cellRenderer.UpdateImage(colors.ToArray());
    }

    public void GeneratePopMap()
    {
        for(int i = 0; i < cellRenderer.cells.Count; i++)
        {
            Cell cell = cellRenderer.cells[i];
    
            cell.totalPopulation = Random.Range(minPopulation, maxPopulation);

            cellRenderer.cells[i] = cell;

            if(i == 0)
            {
                Debug.Log(cellRenderer.cells[i].totalPopulation);
                Debug.Log($"Cell pop {cell.totalPopulation}");
            }
        }
    }
}
