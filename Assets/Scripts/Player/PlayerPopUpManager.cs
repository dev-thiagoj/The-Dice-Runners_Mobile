using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerPopUpManager : MonoBehaviour
{
    public Transform[] popUps;
    public float duration;
    public Ease ease;

    public void CallExpression(int index)
    {
        StartCoroutine(ExpressionsCoroutine(index));
    }

    public IEnumerator ExpressionsCoroutine(int index)
    {
        popUps[index].transform.DOScale(0.5f, duration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        yield return new WaitForEndOfFrame();
    }
}
