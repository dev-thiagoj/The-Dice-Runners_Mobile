using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableBase : MonoBehaviour
{
    public PlayerController player;
    public Vector3 targetPosition;
    public float timeToHide = 0.1f;
    public float timeToDestroy = 0.1f;
    public GameObject graphicItem;

    [Header("Particle System")]
    public ParticleSystem particleSystem;

    private void Awake()
    {
        player = GameObject.Find("=== PLAYER ===").GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        targetPosition = player.currPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Collect();
        }
    }

    protected virtual void HideItens()
    {
        if (graphicItem != null) graphicItem.SetActive(false);
        Invoke(nameof(HideObject), timeToHide);
    }

    protected virtual void Collect()
    {
        Debug.Log("Coin collect");
        HideItens();
        OnCollect();
    }

    private void HideObject()
    {
        gameObject.SetActive(false);
        Invoke(nameof(DestroyObject), timeToDestroy);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    protected virtual void OnCollect()
    {
        
    }
}
