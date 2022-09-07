using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableTurbo : ItemCollectableBase
{
    public Collider collider;
    public bool collect = false;
    public float lerpSpeed = 5;
    public float minDistance = 1f;

    [Header("Actions")]
    public static Action onTurboCollect;

    protected override void Collect()
    {
        onTurboCollect.Invoke();
        OnCollect();
    }

    protected override void OnCollect()
    {
        base.OnCollect();
        collider.enabled = false;
        SFXPool.Instance.Play(SFXType.TURBO_COLLECT_05);
        collect = true;
        ItemManager.Instance.AddTurbo();
    }

    private void Update()
    {
        if (collect)
        {
            transform.position = Vector3.Lerp(transform.position, PlayerController.Instance.transform.position, lerpSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < minDistance)
            {
                HideItens();
                Destroy(gameObject);
            }
        }
    }
}
