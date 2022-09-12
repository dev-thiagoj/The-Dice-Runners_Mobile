using UnityEngine;

public class Magnetic : MonoBehaviour
{
    public PlayerController playerPosition;
    public Vector3 targetPosition;
    public float dist = .2f;
    public float coinSpeed = 3f;

    private void Awake()
    {
        playerPosition = GameObject.Find("=== PLAYER ===").GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        targetPosition = playerPosition.currPosition;

        if(Vector3.Distance(transform.position, targetPosition) > dist)
        {
            coinSpeed++;
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * coinSpeed);
        } 
    }
}
