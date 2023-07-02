using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IterationNum : MonoBehaviour
{
    public string suffixText;
    private TMP_Text text;
    private PopMap popMap;

    void Start()
    {
        popMap = FindObjectOfType<PopMap>();
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
       text.text = $"{suffixText}{popMap.iterationPub}";
    }
}
