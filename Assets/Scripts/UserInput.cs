using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private const int LEFT_MOUSE = 0;

    public static Vector2 movement = new Vector2();
    public static bool attack = false;

    public static bool middleMouse = false;
    public static Vector2 mouseVelocity = new Vector2();
    public static Vector2 mousePosition = new Vector2();

    private InputEvent attackEvent = new InputEvent(LEFT_MOUSE);

    private Vector2 lastMousePos = new Vector2();

    private void Start()
    {
        mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        lastMousePos = mousePosition;
    }

    private void UpdateInputEvents()
    {
        attack = attackEvent.Update();
    }

    private void GetKeyInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void GetMouseInput()
    {
        middleMouse = Input.GetMouseButton(2);

        mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        mouseVelocity.x = Input.GetAxisRaw("Mouse X");
        mouseVelocity.y = Input.GetAxisRaw("Mouse Y");
        mouseVelocity /= Time.deltaTime;

        lastMousePos = mousePosition;
    }

    private void Update()
    {
        UpdateInputEvents();
        GetKeyInput();
        GetMouseInput();
    }
}