using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatToString : MonoBehaviour
{
    public UnityEvent<string> StringEvent;

    public void CallStringEvent(float floatToPass)
    {
        StringEvent.Invoke(string.Format("{0:0.0}", floatToPass));
    }
}
