using UnityEngine;

public class Human : Ai
{
    private void Start()
    {
        CharacterManager.registerHuman(this);
        base.Start();   
    }

    private void FixedUpdate()
    {
        if(transform.GetComponent<Ai>().agent.velocity != Vector3.zero)
        {
            AudioManager.instance.Play("footstep_1");
        }

        base.FixedUpdate();
    }

    // private void LateUpdate()
    // {
    //     // Debug.DrawLine(transform.position, (transform.position + transform.forward) * 3, Color.red);
    //     // Debug.DrawLine(transform.position, transform.position + agent.velocity, Color.blue);
    // }
}
