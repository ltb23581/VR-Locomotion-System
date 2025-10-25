using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class LocomotionSystem : MonoBehaviour
{
    public InputActionProperty moveAction;
    public Transform head;
    public float speed = 2f;
    CharacterController cc;
    float gravity = -9.81f, vSpeed;

    void Awake() => cc = GetComponent<CharacterController>();

    void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 fwd = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(head.right, Vector3.up).normalized;
        Vector3 move = (fwd * input.y + right * input.x) * speed;
        vSpeed = cc.isGrounded ? -0.5f : vSpeed + gravity * Time.deltaTime;
        cc.Move((move + Vector3.up * vSpeed) * Time.deltaTime);
    }

    void OnEnable()
    {
        moveAction.action.Enable();
    }
    void OnDisable()
    {
        moveAction.action.Disable();
    }

}
