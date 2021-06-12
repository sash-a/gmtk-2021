using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : Ai
{
    private void Start()
    {
        CharacterManager.registerHuman(this);
        base.Start();   
    }

    private void FixedUpdate()
    {

        // print($"dest:{agent.destination}|player:{Player.instance.transform.position}");
            // print("visible:" + checkVisisble(Player.instance.gameObject));
        // agent.SetDestination(Player.instance.transform.position);

        if (Input.GetKeyUp(KeyCode.Z))
        {
            character.infect();
        }

        base.FixedUpdate();

    }

    private void LateUpdate()
    {
        // agent.updateRotation = false;
        Debug.DrawLine(transform.position, (transform.position + transform.forward) * 3, Color.red);
        Debug.DrawLine(transform.position, transform.position + agent.velocity, Color.blue);
        // var targetLocation = agent.velocity.normalized;
        //
        //
        // targetLocation.z = transform.position.z; // ensure there is no 3D rotation by aligning Z position
        //
        // // vector from this object towards the target location
        // Vector3 vectorToTarget = targetLocation - transform.position;
        // // rotate that vector by 90 degrees around the Z axis
        // Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
        //
        // // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
        // // (resulting in the X axis facing the target)
        // Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
        // transform.rotation = targetRotation;
        
        Vector3 targetPosition = transform.position + agent.velocity.normalized;
        Vector3 dir = targetPosition - this.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
