using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatToString : MonoBehaviour
{
    public UnityEvent<string> StringEvent;
    public bool decimalNumber;

    public void CallStringEvent(float floatToPass)
    {
        if (decimalNumber)
            StringEvent.Invoke(string.Format("{0:0.0}", floatToPass));
        else
            StringEvent.Invoke(string.Format("{0:0}", floatToPass));
    }
}
