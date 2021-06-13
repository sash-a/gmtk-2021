using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Attacker
{
    public float attackRange; // distance guard will stop and shoot at
    public float bulletRange; // range of the gun
    public GameObject muzzleFlash;
    public float framesToFlash;
    private bool _isFlashing = false;

    public GameObject humanHitEffect;
    public GameObject envHitEffect;
    public GameObject sauceHitEffect;
    public GameObject bloodPuddle;

    private void Start()
    {
        if(muzzleFlash != null)
            muzzleFlash.SetActive(false);
    }

    public override void Attack(Vector3 dir = new Vector3(), bool isPlayer = false)
    {
        if (!_isFlashing && muzzleFlash != null)
        {
            StartCoroutine(doFlash());
        }
        
        if (dir == Vector3.zero)
            dir = transform.right;

        dir = dir.normalized;

        int layerMask = LayerMask.GetMask("player", "zombie", "wall");
        if(isPlayer)
            layerMask = LayerMask.GetMask( "wall", "human");

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, dir, attackRange, layerMask);

        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            Character character = hitInfo.transform.GetComponent<Character>();

            if (character) {
                character.die();
            }
            else
            {
                Debug.Log("ranged guard hit: " + hitInfo.transform.gameObject);
            }

            // TODO: Add hit effect on impact
            if(hitInfo.transform.GetComponent<Controller>() is Human)
            {
                Instantiate(humanHitEffect, hitInfo.point, Quaternion.identity);
                Instantiate(bloodPuddle, hitInfo.point, Quaternion.identity);
            }
            else if(hitInfo.transform.GetComponent<Controller>() is Player)
            {
                Instantiate(sauceHitEffect, hitInfo.point, Quaternion.identity);
            }
            else
            {
                Instantiate(envHitEffect, hitInfo.point, Quaternion.identity);
            }

            


        }

    }

    public bool checkCleanLineSight()
    {    
        int layerMask = LayerMask.GetMask("player", "zombie", "wall");
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, attackRange, layerMask);
        if (hitInfo)
        {
            Character character = hitInfo.transform.GetComponent<Character>();

            if (character)
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator doFlash()
    {
        muzzleFlash.SetActive(true);
        var framesFlashed = 0;
        _isFlashing = true;

        while(framesFlashed <= framesToFlash)
        {
            framesFlashed++;
            yield return null;
        }

        muzzleFlash.SetActive(false);
        _isFlashing = false;
    }

}
