using UnityEngine;

public class RotationLookAt : MonoBehaviour
{
    public Transform target { get; private set; }

    [SerializeField] bool _canLook;

    void Update()
    {
        if (_canLook)
        {
            Vector3 lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }
    }

    public Transform FindPlayerTarget()
    {
        target = GameObject.Find("CharacterPos").GetComponent<Transform>();
        return target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Finish"))
        {
            _canLook = true;
        }
    }
}
