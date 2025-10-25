using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;


public class Teleportation : MonoBehaviour
{
    [Header("References")]
    public Transform rayOrigin;
    public InputActionProperty teleportAction;
    public LayerMask teleportLayer;
    public Transform xrRig;
    public AudioSource teleportSound;

    [Header("Arc Settings")]
    public int segments = 30;
    public float initialSpeed = 10f;
    public float upwardBoost = 2f;
    public Vector3 gravity = new Vector3(0, -9.81f, 0);

    [Header("Hit Visual (optional)")]
    public GameObject reticle;
    public float reticleScale = 0.15f;

    bool hasHit;
    Vector3 hitPoint;

    void Awake()
    {
        if (xrRig == null) xrRig = transform.root;

        if (reticle == null)
        {
            reticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            reticle.GetComponent<Collider>().enabled = false;
            var mr = reticle.GetComponent<MeshRenderer>();
            mr.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            mr.material.color = new Color(0, 1, 0, 0.9f);
            reticle.transform.localScale = Vector3.one * reticleScale;
        }
        reticle.SetActive(false);
    }

    void OnEnable() => teleportAction.action.Enable();
    void OnDisable() => teleportAction.action.Disable();

    void Update()
    {
        bool pressed = teleportAction.action.IsPressed();

        if (!pressed)
        {
            if (hasHit && teleportAction.action.WasReleasedThisFrame())
            {
                if (xrRig.TryGetComponent(out CharacterController cc))
                {
                    cc.enabled = false;
                    xrRig.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
                    cc.enabled = true;
                }
                else
                {
                    xrRig.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
                }

                if (teleportSound != null)
                    teleportSound.Play();

                UnityEngine.XR.InputDevice device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.LeftHand);
                device.SendHapticImpulse(0, 0.6f, 0.15f); 
            }

            reticle.SetActive(false);
            return;
        }

        DrawArcAndRaycast();
        reticle.SetActive(hasHit);
        if (hasHit)
            reticle.transform.position = hitPoint + Vector3.up * 0.05f;
    }

    void DrawArcAndRaycast()
    {
        hasHit = false;
        if (rayOrigin == null) return;

        Vector3 p0 = rayOrigin.position;
        Vector3 v0 = rayOrigin.forward * initialSpeed + Vector3.up * upwardBoost;

        float dt = 0.05f;
        Vector3 prev = p0;

        for (int i = 1; i <= segments; i++)
        {
            float t = i * dt;
            Vector3 pos = p0 + v0 * t + 0.5f * gravity * (t * t);

            Vector3 dir = (pos - prev);
            float dist = dir.magnitude;
            if (dist > 0.0001f)
            {
                dir /= dist;
                if (Physics.Raycast(prev, dir, out RaycastHit hit, dist, teleportLayer, QueryTriggerInteraction.Ignore))
                {
                    hasHit = true;
                    hitPoint = hit.point;
                    return;
                }
            }
            prev = pos;
        }
    }
}
