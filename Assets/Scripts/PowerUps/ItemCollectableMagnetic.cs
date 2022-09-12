using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableMagnetic : ItemCollectableBase
{
    public Collider collider;
    public bool collect = false;
    public float lerpSpeed = 5;
    public float minDistance = 1f;

    [Header("Player Particle Field")]
    [SerializeField] ForceFieldManager forceField;

    [Header("Actions")]
    public static Action onMagneticCollect;

    private void OnValidate()
    {
        if (forceField == null) forceField = GetComponentInChildren<ForceFieldManager>();
    }

    protected override void Collect()
    {
        onMagneticCollect.Invoke();
        OnCollect();
    }

    protected override void OnCollect()
    {
        base.OnCollect();
        collider.enabled = false;
        collect = true;
        SFXPool.Instance.Play(SFXType.MAGNETIC_COLLECT_07);
        forceField.StartParticleField();
    }

    private void Update()
    {
        if (collect)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, lerpSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.transform.position) < minDistance)
            {
                HideItens();
                Destroy(gameObject);
            }
        }
    }
}
