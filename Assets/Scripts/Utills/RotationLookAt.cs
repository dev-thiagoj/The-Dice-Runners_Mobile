using UnityEngine;

public class RotationLookAt : MonoBehaviour
{
    public bool canLook;
    public Transform target;
    

    void Update()
    {
        if (canLook)
        {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Finish"))
        {
            canLook = true;
        }
    }
}
