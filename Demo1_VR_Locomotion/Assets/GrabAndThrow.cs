using UnityEngine;
using UnityEngine.XR;

public class GrabAndThrow : MonoBehaviour
{
    public bool rightHand = true;
    public float throwForceMultiplier = 1.5f;

    private Rigidbody hoveredRb;
    private Rigidbody grabbedRb;
    private Transform originalParent;
    private Vector3 lastPos;
    private Vector3 velocity;

    private UnityEngine.XR.InputDevice device; 

    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer hoveredRenderer;

    public AudioSource grabSound; 

    void Start()
    {
        device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(
            rightHand ? XRNode.RightHand : XRNode.LeftHand);
    }

    void Update()
    {
        velocity = (transform.position - lastPos) / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPos = transform.position;

        if (!device.isValid)
        {
            device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(
                rightHand ? XRNode.RightHand : XRNode.LeftHand);
        }

        if (device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            if (triggerValue > 0.8f && grabbedRb == null && hoveredRb != null)
            {
                grabbedRb = hoveredRb;
                originalParent = grabbedRb.transform.parent;
                grabbedRb.isKinematic = true;
                grabbedRb.useGravity = false;
                grabbedRb.transform.SetParent(transform, true);

                if (grabSound != null)
                    grabSound.Play();

                device.SendHapticImpulse(0, 0.7f, 0.1f); 

                Debug.Log("Grabbed " + grabbedRb.name);
            }

            if (triggerValue < 0.1f && grabbedRb != null)
            {
                grabbedRb.transform.SetParent(originalParent, true);
                grabbedRb.isKinematic = false;
                grabbedRb.useGravity = true;

                grabbedRb.linearVelocity = velocity * throwForceMultiplier;

                device.SendHapticImpulse(0, 0.3f, 0.05f);

                Debug.Log("Released " + grabbedRb.name);
                grabbedRb = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            hoveredRb = other.attachedRigidbody;

            hoveredRenderer = hoveredRb.GetComponent<Renderer>();
            if (hoveredRenderer != null)
            {
                originalMaterial = hoveredRenderer.material;
                hoveredRenderer.material = highlightMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hoveredRb != null && other.attachedRigidbody == hoveredRb)
        {
            if (hoveredRenderer != null)
            {
                hoveredRenderer.material = originalMaterial;
                hoveredRenderer = null;
            }

            hoveredRb = null;
        }
    }
}
