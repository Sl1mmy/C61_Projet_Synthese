using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    public float speed = 3;
    public float rotationSpeed = 90;
    public float gravity = -20f;
    public float jumpSpeed = 15;

    CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        if (characterController.isGrounded)
        {
            transform.Translate(direction * Time.deltaTime * speed, Space.World);
            transform.LookAt(direction + transform.position);
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
        }
        //Adding gravity
        characterController.Move(direction * Time.deltaTime * speed);
    }
}

