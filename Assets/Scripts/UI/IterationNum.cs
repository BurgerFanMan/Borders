using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IterationNum : InfoClass
{
    int iteration = 0;
    protected override void OnIterate()
    {
       iteration++;
       text.text = $"{suffixText}{iteration}";
    }

    protected override void OnGenerate()
    {
        iteration = -1;

        OnIterate();
    }
}
