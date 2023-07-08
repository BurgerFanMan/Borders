using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoClass : MonoBehaviour
{
    public string suffixText;
    protected TMP_Text text;
    protected CellRenderer cellRenderer;
    protected PopMap popMap;

    // Start is called before the first frame update
    void Start()
    {
        popMap = FindObjectOfType<PopMap>();
        cellRenderer = FindObjectOfType<CellRenderer>();

        text = GetComponent<TMP_Text>();

        popMap.onIterate += OnIterate;
        popMap.onGenerate += OnGenerate;

        OnGenerate();
    }

    protected virtual void OnIterate()
    {

    }

    protected virtual void OnGenerate()
    {
        OnIterate();
    }
}
