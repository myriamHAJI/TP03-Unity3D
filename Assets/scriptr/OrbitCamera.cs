using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    public Transform player;

    private float yaw;
    private float pitch = 25f;
    private float distance = 6f;

    void LateUpdate()
    {
        if (player == null) return;

        Mouse mouse = Mouse.current;

        if (mouse != null)
        {
            bool rightHeld = mouse.rightButton.isPressed;
            bool leftHeld = mouse.leftButton.isPressed;

            if (rightHeld || leftHeld)
            {
                Vector2 movement = mouse.delta.ReadValue();

                yaw += movement.x * 0.15f;
                pitch -= movement.y * 0.15f;
                pitch = Mathf.Clamp(pitch, 10f, 60f);

                if (rightHeld)
                    player.rotation = Quaternion.Euler(0f, yaw, 0f);
            }

            distance -= mouse.scroll.ReadValue().y * 0.01f;
            distance = Mathf.Clamp(distance, 3f, 10f);
        }

        Vector3 target = player.position + Vector3.up;
        Quaternion orbit = Quaternion.Euler(pitch, yaw, 0f);

        transform.position = target + orbit * Vector3.back * distance;
        transform.LookAt(target);
    }
}