using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatToString : MonoBehaviour
{
    public UnityEvent<string> StringEvent;
    public bool decimalNumber;
    public int decimals = 1;

    public void CallStringEvent(float floatToPass)
    {
        if (decimalNumber)
        {
            string format = "{0:0." + new string('0', decimals) + "}";
            StringEvent.Invoke(string.Format(format, floatToPass));
        }
        else
            StringEvent.Invoke(string.Format("{0:0}", floatToPass));
    }
}
