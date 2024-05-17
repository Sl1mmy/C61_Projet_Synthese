using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float playerSpeed;
    public float DeathDistance;

    private Animator anim;
    private Vector3 StartPos;


    private void Start()
    {
        anim = GetComponent<Animator>();
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < DeathDistance)
        {
            transform.position = StartPos;
        }
        
        MovePlayer();
    }

    void MovePlayer()
    {

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            transform.Translate(direction * Time.deltaTime * playerSpeed, Space.World);
            transform.LookAt(direction + transform.position);
        }

    }
}

