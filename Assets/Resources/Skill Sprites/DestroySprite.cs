using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySprite : MonoBehaviour
{
    public void DestroyWhenAnimationFinished()
    {
        Destroy(this.gameObject);
    }
}
