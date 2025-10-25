using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HeadHeight : MonoBehaviour
{
    public Transform head;
    CharacterController cc;

    void Awake() => cc = GetComponent<CharacterController>();

    void LateUpdate()
    {
        float height = Mathf.Max(1.2f, head.localPosition.y + 0.2f);
        cc.height = height;
        cc.center = new Vector3(0, height / 2f, 0);
    }
}
