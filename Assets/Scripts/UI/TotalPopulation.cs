using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalPopulation : InfoClass
{
    protected override void OnIterate()
    { 
        int population = 0;
        foreach(Cell cell in cellRenderer.cells)
        {
            population += cell.totalPopulation;
        }

        text.text = $"{suffixText}{population}"; 
    }
}
