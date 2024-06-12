using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInCanvasGroup : MonoBehaviour
{
    public void FadeInGameOver()
    {
        LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 1, 2f);
    }
}
