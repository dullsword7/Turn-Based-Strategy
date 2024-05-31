using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySprite : MonoBehaviour
{
    public void Hello()
    {
        Debug.Log("Hello im destroting myself");
        Destroy(this.gameObject);
    }
    public void PrintEvent(string s)
    {
        Debug.Log("PrintEvent called at " + Time.time + " with a value of " + s);
    }
}
