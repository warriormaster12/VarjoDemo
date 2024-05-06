using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitPosition : MonoBehaviour
{
    [SerializeField] private float distance = 20f;

    private Vector3 originalPosition = Vector3.zero;
    private Rigidbody rb = null;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        if (rb == null )
        {
            Debug.LogWarning("no rigidbody component found");
        }
    }

    // Update is called once per physics frame
    void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }
        float currentDistance = Mathf.Abs((transform.position - originalPosition).magnitude);
        if (distance < currentDistance) {
            RigidbodyConstraints constraints = rb.constraints;
            transform.localPosition = new Vector3(
                (constraints & RigidbodyConstraints.FreezePositionX) != RigidbodyConstraints.None ? transform.localPosition.x : Mathf.Clamp(transform.localPosition.x, -distance, distance),
                (constraints & RigidbodyConstraints.FreezePositionY) != RigidbodyConstraints.None ? transform.localPosition.y : Mathf.Clamp(transform.localPosition.y, -distance, distance),
                (constraints & RigidbodyConstraints.FreezePositionZ) != RigidbodyConstraints.None ? transform.localPosition.z : Mathf.Clamp(transform.localPosition.z, -distance, distance)
            );
        }
    }
}
