using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private const int LEFT_MOUSE = 0;

    public static Vector2 movement = new Vector2();
    public static bool attack;

    public static Vector2 mouseDirection;

    private InputEvent attackEvent = new InputEvent(LEFT_MOUSE);

    private void GetKeyInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void GetMouseInput()
    {
        attack = attackEvent.Update();

        Vector2 halfScreenSize = new Vector2(Screen.width, Screen.height)*0.5f;
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mouseDirection = mousePos - halfScreenSize;
        mouseDirection.Normalize();
    }

    private void Update()
    {
        GetKeyInput();
        GetMouseInput();
    }
}