using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //классическое перемещение игрока

    public float movespeed;
    public float sensitivity;
    public float gravity;
    private float mouseX;
    private float mouseY;
    private float vertical;
    private float horizontal;

    public Vector2 clampangle;
    private Vector3 Velocity;
    private Vector2 angle;
    public Transform cameraTransform; //камера внутри игрока

    private CharacterController charactercontroller;

    private void Start()
    {
        charactercontroller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        Vector3 playerMovementInput = new Vector3(horizontal, 0.0f, vertical);
        Vector3 moveVector = transform.TransformDirection(playerMovementInput);

        if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0)
            {
                movespeed = 6;
                //бежит
            }
            else
            {
                movespeed = 2f;
                //идет
            }
        }
        else
        {
            //стоит
        }

        if (charactercontroller.isGrounded)
        {
            Velocity.y = -1f;
        }
        else
        {
            Velocity.y -= gravity * Time.deltaTime;
        }

        charactercontroller.Move(moveVector * movespeed * Time.deltaTime);
        charactercontroller.Move(Velocity * Time.deltaTime);

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        angle.x -= mouseY * sensitivity;
        angle.y -= mouseX * -sensitivity;

        angle.x = Mathf.Clamp(angle.x, -clampangle.x, clampangle.y);

        Quaternion rotation = Quaternion.Euler(angle.x, angle.y, 0.0f);
        Quaternion rotationTwo = Quaternion.Euler(0.0f, angle.y, 0.0f);
        transform.rotation = rotationTwo;
        cameraTransform.rotation = rotation;
    }


}