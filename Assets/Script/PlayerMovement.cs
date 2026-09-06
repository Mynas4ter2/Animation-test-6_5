using UnityEngine;
using UnityEngine.InputSystem; // บังคับใช้ New Input System

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("การตั้งค่าตัวละคร")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 3.5f;

    [Header("การตั้งค่ากล้อง")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 0.2f; // ปรับลงมาหน่อยเพราะ Input System ค่าเมาส์จะแรงกว่า
    [SerializeField] private float maxLookAngle = 85f;

    [Header("ระบบอนิเมชัน")]
    [SerializeField] private Animator anim;


    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (controller == null) controller = GetComponent<CharacterController>();

        if (playerCamera != null)
        {
            xRotation = playerCamera.localEulerAngles.x;
            if (xRotation > 180f) xRotation -= 360f;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {

        float mouseX = 0f;
        float mouseY = 0f;

        // ดึงค่าการขยับเมาส์จาก New Input System
        if (Mouse.current != null)
        {
            mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity;
            mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        float x = 0f;
        float z = 0f;

        // เช็คปุ่มคีย์บอร์ดโดยตรง
        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed) x += 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude > 1f) move.Normalize();

        controller.Move(move * speed * Time.deltaTime + Vector3.down * 2f * Time.deltaTime);

        if (anim != null)
        {
            float moveMagnitude = new Vector2(x, z).magnitude;
            anim.SetFloat("Speed", Mathf.Clamp01(moveMagnitude));
        }
    }
}