﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : State
{

    public float moveSpeed = 5;
    public CharacterController controller;
    public float gravityScale = 10;
    // Start is called before the first frame update

    public Vector3 movementDirection;


    public MovementState(Player player) : base(player)
    {

    }

    public override void Tick()//MovementStatement update
    {
        //horizontal plane movement (3D)
        movementDirection = (player.transform.forward * Input.GetAxis("Vertical") * moveSpeed) + (player.transform.right * Input.GetAxis("Horizontal") * moveSpeed);
        //vertical movement(3D) 
        movementDirection.y = movementDirection.y + gravityScale * (Physics.gravity.y * Time.deltaTime);

        // GONNA PUT PLACEHOLDER CODE HERE
        if (movementDirection.x != 0 || movementDirection.z != 0)
        {
            player.transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", true);
            player.transform.GetChild(0).GetComponent<Animator>().SetFloat("WalkingSpeed", movementDirection.z);

        }
        else
        {

            player.transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", false);

        }

        if (Input.GetButtonDown("Fire1"))
        {
            player.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");

        }


        if (Input.GetButtonDown("Dash"))//when the dash button is pressed we make the calculations of movement direction and use this information that it is going to be used for the DashState
        {
            Vector3 dahsDirection;
            if (movementDirection.x == 0 && movementDirection.z == 0)
            {//if the player is not movig the dash is backwards
                dahsDirection =  -player.transform.forward;
                player.SetState(new DashState(player, dahsDirection));
            }
            else
            {//if not we calculate the dash direction on the direction of the player movement
                Vector3 forwardMovement = player.transform.forward * Input.GetAxis("Vertical");
                Vector3 sideMovement = player.transform.right * Input.GetAxis("Horizontal");
                dahsDirection = sideMovement + forwardMovement;

                player.SetState(new DashState(player, dahsDirection));
            }
        }

        controller.Move(movementDirection * Time.deltaTime);
    }

    public override void OnStateEnter()//MoementStatement start
    {
        controller = player.GetComponent<CharacterController>();
    }
}
