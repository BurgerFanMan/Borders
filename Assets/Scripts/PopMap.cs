using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopMap : MonoBehaviour
{
    public int maxPopulation;
    [Range(1f, 300f)]
    public float maxPopulationMult = 5f;
    public int minPopulation;
    public Color maxColor;
    public Color minColor;

    [Header("Population Tiles")]
    [Tooltip("The interval between iterations in seconds.")]
    public float intervalInSeconds = 0.2f;
    public int tilesPerIteration = 40;
    public float expandedPopRatio = 1f;
    public float popRatioRange = 0.2f;
    public int startingTiles = 10;
    [Range(0f, 100f)]
    public float avoidPopTiles = 0.5f;

    [Header("Population Decay")]
    [Tooltip("The percentage of max population by which each cell's population decays")]
    [Range(0f, 1f)]
    public float popDecayPercent = 0.1f;
    public float popDecayRange = 0.03f;

    public float tilesPerIterationPub { get { return tilesPerIteration;  } set { tilesPerIteration = Mathf.RoundToInt(value); } }
    public float iterationPub { get { return iteration; } set { iteration = Mathf.RoundToInt(value); } }

    private int iteration;
    private bool generated = false;

    private CellRenderer cellRenderer;
    private List<Cell> populatedCells = new List<Cell>();

    private int width;

    private bool autoIterate;

    void Start()
    {
        cellRenderer = FindObjectOfType<CellRenderer>();

        width = cellRenderer.width;
    }

    //Actual rendering of the population map, decently expensive operation
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
            float weight = (float)cellPop / (float)(maxPopulation * maxPopulationMult);
            float returnValue = ((maxC * weight) + minC);

            return returnValue;
        }

        cellRenderer.UpdateImage(colors.ToArray());
    }

    public void GeneratePopMap()
    {
        foreach(Cell cell in cellRenderer.cells)
        {
            cell.totalPopulation = 0;
        }

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

    //Gets more expensive as more tiles are populated, look for potential fix
    public void IteratePop()
    {
        if(!generated)
        {
            generated = true;
            GeneratePopMap();
        }

        iteration += 1;

        List<Cell> newPopulatedCells = new List<Cell>();
        HashSet<Cell> subjectCells = new HashSet<Cell>(cellRenderer.cells);

        int maxPop = (int)(maxPopulation * maxPopulationMult);

        for (int i = 0; i < tilesPerIteration; i++)
        {
            int index = Random.Range(0, populatedCells.Count);
            Cell cell = populatedCells[index];

            List<Cell> adjacentCells = GetAdjacentCells(cellRenderer.cells.IndexOf(cell), subjectCells);
            if(adjacentCells.Count == 0)
            {
                i -= 1;
                continue;
            }

            Cell populateCell = adjacentCells[Random.Range(0, adjacentCells.Count)];

            float chanceToSkip = (1f / (float)(populateCell.totalPopulation / (float)maxPop)) - 1f;
            bool skipPopulated = Random.Range(0f, avoidPopTiles * 4f) > chanceToSkip;
            if (populateCell.totalPopulation >= maxPop || skipPopulated)
            {
                subjectCells.Remove(populateCell);

                i -= 1;
                continue;
            }

            int population = (int)(cell.totalPopulation * (expandedPopRatio + Random.Range(-popRatioRange, popRatioRange)));

            populateCell.totalPopulation += population;
            populateCell.totalPopulation = Mathf.Clamp(populateCell.totalPopulation, 0, maxPop); ;

            if(!populatedCells.Contains(populateCell))
                newPopulatedCells.Add(populateCell);
        }

        populatedCells.AddRange(newPopulatedCells);

        DecayPopulation();

        foreach(Cell cell in populatedCells)
        {
            cellRenderer.cells[cell.index] = cell;
        }

        DisplayPopMap();

        List<Cell> GetAdjacentCells(int index, HashSet<Cell> checkCells)
        {
            List<Cell> cells = new List<Cell>(4);

            if(index > 0)
            cells.Add(cellRenderer.cells[index - 1]);
            if(index + 1 < cellRenderer.cells.Count)
            cells.Add(cellRenderer.cells[index + 1]);
            if(index - width >= 0)
            cells.Add(cellRenderer.cells[index - width]);
            if(index + width < cellRenderer.cells.Count)
            cells.Add(cellRenderer.cells[index + width]);

            cells.RemoveAll(cell => !checkCells.Contains(cell));

            return cells;
        }

        void DecayPopulation()
        {
            HashSet<Cell> deadCells = new HashSet<Cell>();
            float maxDecay = (float)(maxPopulation * maxPopulationMult * popDecayPercent)/100f;

            foreach (Cell cell in populatedCells)
            {
                cell.totalPopulation -= (int)(maxDecay * (1f + Random.Range(-popDecayRange, popDecayRange)));

                if(cell.totalPopulation <= 0)
                {
                    cell.totalPopulation = 0;
                    deadCells.Add(cell);
                }
            }

            populatedCells.RemoveAll(cell => deadCells.Contains(cell));
        }
    }

    public void AutoIterate(float intervals)
    {
        if (autoIterate)
        {
            CancelInvoke("IteratePop");
            InvokeRepeating("IteratePop", 0.1f, intervals);
        }
            intervalInSeconds = intervals;
    }
    public void AutoIterate(bool enable)
    {
        if(!enable)
            CancelInvoke("IteratePop");
        else
            InvokeRepeating("IteratePop", 0.1f, intervalInSeconds);

        autoIterate = enable;
    }
}
