using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum ExpressionType
{
    SURPRISE,
    DEATH
}

[Serializable]
public class ExpressionSetup
{
    public ExpressionType expressionType;
    public Transform expressionGO;
}

public class PlayerPopUpManager : MonoBehaviour
{
    [SerializeField] float endSize;
    [SerializeField] float duration;
    [SerializeField] Ease ease;
    [SerializeField] List<ExpressionSetup> expressionsSetup;

    public void CallExpression(ExpressionType type)
    {
        Transform obj = GetExpressionByType(type).expressionGO;

        StartCoroutine(ExpressionsCoroutine(obj));
    }

    ExpressionSetup GetExpressionByType(ExpressionType expressionType)
    {
        return expressionsSetup.Find(i => i.expressionType == expressionType);
    }

    IEnumerator ExpressionsCoroutine(Transform obj)
    {
        obj.DOScale(endSize, duration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        yield return new WaitForEndOfFrame();
    }
}