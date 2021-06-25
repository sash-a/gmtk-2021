using System;
using System.Collections;
using System.Collections.Generic;
using State_Machine;
using UnityEngine;
using UnityEngine.AI;

public class Player : Controller
{
    public static Player instance;
    public static float infectionDistance = 3;
    public static float infectionAngle = 45;
    public static float ejectForce = 60f;

    [NonSerialized] public float remainingSlideTime = 0;
    [NonSerialized] public Character exitedHost = null;

    public TentacleArm arm;
    
    void Awake()
    {
        base.Awake();
        instance = this;
        if (arm == null)
        {
            arm = GetComponentInChildren<TentacleArm>();
            arm.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
    }


    // ReSharper disable Unity.PerformanceAnalysis
    void Update()
    {
        FindHosts();
        TryAttack();
    }

    private void FixedUpdate()
    {
        if (remainingSlideTime > 0) // no inputs until slide is over
        {
            remainingSlideTime -= Time.deltaTime;
            return;
        }

        exitedHost = null;
        Vector2 dir = move();
        HandleRotation(dir);
    }

    void HandleRotation(Vector2 moveDir)
    {
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
        float armLen = Vector2.Distance(mouse, transform.position);
        armLen = Mathf.Min(armLen, infectionDistance);
        arm.armLength = armLen;
        arm.updateLength();
        
        if (character is Sauce)
        {
            // transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(0, 0,
                Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg);
            ((Sauce) character).sauceAnimator.gameObject.transform.rotation =
                Quaternion.Euler(0, 0, Mathf.Atan2(-moveDir.x, moveDir.y) * Mathf.Rad2Deg);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0,
                Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void FindHosts() // finds candidate host humans. checks for space input to jump
    {
        HashSet<Controller> visibleHumans = CharacterManager.getVisibleHumans(this);
        if (visibleHumans.Count == 0)
        {
            // noone in range
            NoHostCandidates();
            // Debug.Log("no one in range");
            return;
        }

        Controller targetHuman = null;
        // if multiple humans in range, must select the one which is closest
        float minDistance = float.MaxValue;
        foreach (var human in visibleHumans)
        {
            float distance = Vector3.Distance(transform.position, human.transform.position);
            bool canSeeYou = human.checkVisisble(gameObject);
            // Should you also not be able to assimilate while searching (rotating)?
            bool chasingYou = ((Attacker) human.character).playerState.GetBool(AnimatorFields.Chasing);

            if (distance < minDistance && !canSeeYou && !chasingYou)
            {
                minDistance = distance;
                targetHuman = human;
            }
        }

        if (targetHuman == null)
        {
            return;
        }

        targetHuman.glow(); // TODO glow anyways, but maybe in red
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpToHost(targetHuman);
        }
    }

    private void NoHostCandidates()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GetComponent<Sauce>() == null) // cannot eject from sauce
            {
                eject();
                TransitionManager.humanify(this); // destroys this object
            }
        }
    }
    
    public void eject(Vector3 direction=new Vector3()) // method should be called when the player leaps out of the character
    {
        GameObject newSauce = Instantiate(CharacterManager.instance.saucePrefab, transform.position, transform.rotation);
        Player newPlayer = newSauce.GetComponent<Player>();
        newPlayer.exitedHost = character;
        newPlayer.leap(direction);
        arm.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public void leap(Vector3 direction=new Vector3()) // leaps forward, or custom direction
    {
        if (direction == Vector3.zero)
        {
            direction = transform.right;
        }
        else
        {
            direction = direction.normalized;
        }
        rb.velocity =  direction * Player.ejectForce;
        remainingSlideTime = 0.2f;
    }

    private void JumpToHost(Controller targetHuman)
    {
        // the human should be given a player controller, and the current player object should be destroyed
        Vector3 dir = targetHuman.transform.position - transform.position;
        if (character is Sauce)
        {
            leap(dir);
        }
        else
        {
            eject(dir);
            TransitionManager.humanify(this); // destroys this object
        }
    }

    Vector2 move()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir += Vector2.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            dir += Vector2.right;
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir += Vector2.up;
        }

        if (Input.GetKey(KeyCode.S))
        {
            dir += Vector2.down;
        }

        // character.SpriteController.legs.transform.rotation = Quaternion.LookRotation(transform.forward, dir);
        if (character.SpriteController.legsAnimator.runtimeAnimatorController != null)
        {
            character.SpriteController.legsAnimator.SetBool(AnimatorFields.Walking, dir.magnitude > 0);
            character.SpriteController.torsoAnimator.SetBool(AnimatorFields.Walking, dir.magnitude > 0);
        }

        if (character is Sauce)
        {
            ((Sauce) character).sauceAnimator.SetBool(AnimatorFields.Walking, dir.magnitude > 0);

            if (dir.magnitude < 0)
            {
                AudioManager.instance.Play("sauce");
            }
        }

        dir = dir.normalized;
        moveDirection(dir);

        return dir;
    }

    private void TryAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Attack(mousePos - transform.position);
            character.SpriteController.torsoAnimator.SetBool(AnimatorFields.Attacking, true);
        }
    }

    void Attack(Vector3 mouseDir)
    {
        if (character is Ranged)
        {
            ((Ranged) character).Attack(mouseDir, isPlayer: true);
        }

        if (character is Melee)
        {
            ((Melee) character).Attack(mouseDir, isPlayer: true);
        }
    }

    public override bool checkVisisble(GameObject go, float visionAngle=-1, float visionDistance=-1, List<string> layers = null)
    {
        layers = new List<string>();
        layers.AddRange(new []{"human", "wall", "obstacles"});
        return base.checkVisisble(go, infectionAngle, infectionDistance, layers);
    }
}