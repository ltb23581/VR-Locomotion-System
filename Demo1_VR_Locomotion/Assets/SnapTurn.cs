using UnityEngine;
using UnityEngine.InputSystem;

public class SnapTurn : MonoBehaviour
{
    public InputActionProperty rotateAction;
    public float snapAngle = 45f;
    bool gating;

    void Update()
    {
        float x = rotateAction.action.ReadValue<Vector2>().x;
        if (!gating && Mathf.Abs(x) > 0.8f)
        {
            transform.Rotate(0, Mathf.Sign(x) * snapAngle, 0);
            gating = true;
        }
        if (Mathf.Abs(x) < 0.2f) gating = false;
    }

    void OnEnable()
    {
        rotateAction.action.Enable();
    }

    void OnDisable()
    {
        rotateAction.action.Disable();
    }

}
