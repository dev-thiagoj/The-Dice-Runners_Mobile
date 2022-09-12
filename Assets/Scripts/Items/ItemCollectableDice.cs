using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableDice : ItemCollectableBase
{
    public Collider collider;
    public bool collect = false;
    public float lerpSpeed = 5;
    public float minDistance = 1f;

    protected override void Collect()
    {
        OnCollect();
    }

    protected override void OnCollect()
    {
        base.OnCollect();
        collider.enabled = false;
        SFXPool.Instance.Play(SFXType.DICE_COLLECT_04);
        collect = true;
        ItemManager.Instance.AddCoins();
    }

    private void LateUpdate()
    {
        if (collect)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < minDistance)
            {
                HideItens();
                Destroy(gameObject);
            }
        }
    }
}
