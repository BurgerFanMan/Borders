using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopMap : MonoBehaviour
{
    public int maxPopulation;
    public int minPopulation;
    public Color maxColor;
    public Color minColor;

    [Header("Population Tiles")]
    [Tooltip("The interval between iterations in seconds.")]
    public float intervalInSeconds = 0.2f;
    public int maxIterations = 100;
    public int tilesPerIteration = 40;
    public float expandedPopRatio = 1f;
    public float popRatioRange = 0.2f;
    public int startingTiles = 10;
    [Range(1f, 300f)]
    public float maxPopulationRatio = 5f;
    [Tooltip("The percentage of max population by which each cell's population decays")]
    [Range(0f, 1f)]
    public float popDecayPercent = 0.1f;
    public float popDecayRange = 0.03f;

    private int iteration;


    private CellRenderer cellRenderer;
    private List<Cell> populatedCells = new List<Cell>();

    private int width;

    void Start()
    {
        cellRenderer = FindObjectOfType<CellRenderer>();

        width = cellRenderer.width;
    }

    void DisplayPopMap()
    {
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
            float weight = (float)cellPop / (float)(maxPopulation * maxPopulationRatio);
            float returnValue = ((maxC * weight) + minC);

            return returnValue;
        }

        cellRenderer.UpdateImage(colors.ToArray());
    }

    public void GeneratePopMap()
    {
        for(int i = 0; i < startingTiles; i++)
        {
            int index = Random.Range(0, cellRenderer.cells.Count - 1);
            Cell cell = cellRenderer.cells[index];

            if (populatedCells.Contains(cell))
            {
                i -= 1;
                continue;
            }

            cell.totalPopulation = Random.Range(minPopulation, maxPopulation);
            cellRenderer.cells[index] = cell;

            populatedCells.Add(cellRenderer.cells[index]);
        }

        DisplayPopMap();
    }

    public void IteratePop()
    {
        iteration += 1;

        List<Cell> newPopulatedCells = new List<Cell>();
        for(int i = 0; i < tilesPerIteration; i++)
        {
            int index = Random.Range(0, populatedCells.Count - 1);
            Cell cell = populatedCells[index];

            int population = (int)(cell.totalPopulation * (expandedPopRatio + Random.Range(-popRatioRange, popRatioRange)));

            List<Cell> adjacentCells = GetAdjacentCells(cellRenderer.cells.IndexOf(cell));

            Cell populateCell = adjacentCells[Random.Range(0, adjacentCells.Count)];

            if (populateCell.totalPopulation >= (int)(maxPopulation * maxPopulationRatio))
            {
                i -= 1;
                continue;
            }

            populateCell.totalPopulation += Mathf.Clamp(population, 0, (int)(maxPopulation * maxPopulationRatio));

            newPopulatedCells.Add(populateCell);
        }

        populatedCells.AddRange(newPopulatedCells);

        DecayPopulation();

        foreach(Cell cell in populatedCells)
        {
            cellRenderer.cells[cell.index] = cell;
        }

        DisplayPopMap();

        List<Cell> GetAdjacentCells(int index)
        {
            List<Cell> cells = new List<Cell>();

            if(index > 0)
            cells.Add(cellRenderer.cells[index - 1]);
            if(index + 1 < cellRenderer.cells.Count)
            cells.Add(cellRenderer.cells[index + 1]);
            if(index - width >= 0)
            cells.Add(cellRenderer.cells[index - width]);
            if(index + width < cellRenderer.cells.Count)
            cells.Add(cellRenderer.cells[index + width]);

            return cells;
        }

        void DecayPopulation()
        {
            foreach(Cell cell in populatedCells)
            {
                cell.totalPopulation -= 
                    (int)(maxPopulation * maxPopulationRatio * (popDecayPercent + Random.Range(-popDecayRange, popDecayRange)))/100;
            }
        }
    }

    public void AutoIterate(float timesPerSecond)
    {
        CancelInvoke("IteratePop");
        InvokeRepeating("IteratePop", 0.1f, timesPerSecond);

        intervalInSeconds = timesPerSecond;
    }

    public void AutoIterate(bool enable)
    {
        if(!enable)
            CancelInvoke("IteratePop");
        else
            InvokeRepeating("IteratePop", 0.1f, intervalInSeconds);
    }
}
