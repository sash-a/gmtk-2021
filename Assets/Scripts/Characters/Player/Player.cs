using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Controller
{
    public static Player instance;
    public static float infectionDistance = 3;
    public static float infectionAngle = 45;
    public static float ejectForce = 40f;
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private static readonly int Zombiefying = Animator.StringToHash("zombiefying");

    [NonSerialized] public float remainingSlideTime = 0;

    void Awake()
    {
        base.Awake();
        instance = this;
    }


    // ReSharper disable Unity.PerformanceAnalysis
    void Update()
    {
        FindHosts();
        TryAttack();
    }

    private void FixedUpdate()
    {
        if (remainingSlideTime > 0)
        {
            remainingSlideTime -= Time.deltaTime;
            return;
        }//no inputs until slide is over
        Vector2 dir = move();
        HandleRotation(dir);
    }

    void HandleRotation(Vector2 moveDir)
    {
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
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

            if (distance < minDistance && !canSeeYou)
            {
                minDistance = distance;
                targetHuman = human;
            }
        }

        if (targetHuman == null)
        {
            return;
            throw new Exception("should never happen");
        }

        targetHuman.glow();
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
                character.eject();
                CharacterManager.humanify(this);
            }
        }
    }

    private void JumpToHost(Controller targetHuman)
    {
        // the human should be given a player controller, and the current player object should be destroyed
        CharacterManager.bodySnatch(targetHuman, this);
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
        character.SpriteController.legsAnimator.SetBool(Walking, dir.magnitude > 0);
        character.SpriteController.torsoAnimator.SetBool(Walking, dir.magnitude > 0);

        if (character is Sauce)
        {
            ((Sauce) character).sauceAnimator.SetBool(Walking, dir.magnitude > 0);

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
            character.SpriteController.torsoAnimator.SetBool(Attacking, true);
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
}