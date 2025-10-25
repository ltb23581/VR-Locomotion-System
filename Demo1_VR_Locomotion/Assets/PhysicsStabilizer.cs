using System.Collections;
using UnityEngine;

public class PhysicsStabilizer : MonoBehaviour
{
    Rigidbody rb;
    void Awake() => rb = GetComponent<Rigidbody>();

    IEnumerator Start()
    {
        rb.isKinematic = true;
        yield return new WaitForFixedUpdate();
        rb.isKinematic = false;
    }
}
