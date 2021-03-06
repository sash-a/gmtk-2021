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
    [NonSerialized] public float timeOfCharacterChange = -1;

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

        timeOfCharacterChange = Time.time;
    }


    // ReSharper disable Unity.PerformanceAnalysis
    void Update()
    {
        FindHosts();
        TryAttack();
        HandleZombiePoints();
    }

    private void HandleZombiePoints()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ZombiePointManager.spawnZombiePoint(this, arm.tip);
        }    
    }

    private void FixedUpdate()
    {
        if (remainingSlideTime > 0) // no inputs until slide is over
        {
            remainingSlideTime -= Time.deltaTime;
            gameObject.layer = LayerMask.NameToLayer("sliding");
            return;
        }
        
        gameObject.layer = LayerMask.NameToLayer("player");
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

        transform.rotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg);

        if (character is Sauce sauce)
        {
            // transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg);

            sauce.sauceAnimator.gameObject.transform.rotation =
                Quaternion.Euler(0, 0, Mathf.Atan2(-moveDir.x, moveDir.y) * Mathf.Rad2Deg);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void FindHosts() // finds candidate host humans. checks for space input to jump
    {
        HashSet<Controller> visibleHumans = CharacterManager.getVisibleHumans(this);
        visibleHumans.UnionWith(CharacterManager.getVisibleInfected(this));
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
            if (!canInfectHost(human))
            {
                continue;
            }
            
            float distance = Vector3.Distance(transform.position, human.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                targetHuman = human;
            }
        }

        if (targetHuman == null)
        {
            return;
        }

        targetHuman.character.glow(); // TODO glow anyways, but maybe in red
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpToHost(targetHuman);
        }
    }

    public bool canInfectHost(Controller human)
    {
        bool canSeeYou = human.checkVisible(gameObject);
        bool chasingSomeone = ((Attacker) human.character).playerState.GetBool(AnimatorFields.Chasing);
        bool isZombifying = ((Attacker) human.character).playerState.GetBool(AnimatorFields.Zombiefying);

        return !canSeeYou || isZombifying;
    }

    private void JumpToHost(Controller targetHuman)
    {
        // the human should be given a player controller, and the current player object should be destroyed
        alertWitnesses(targetHuman); // assumes the infection will be sucessful
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

    private void NoHostCandidates()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GetComponent<Sauce>() == null) // cannot eject from sauce
            {
                eject(transform.right);
                TransitionManager.humanify(this); // destroys this object
            }
        }
    }

    public void eject(Vector3 direction = new Vector3())
    {
        // method should be called when the player leaps out of the character
        alertWitnesses();// if anyone saw you leave this host, it will be sussed
        if (direction == Vector3.zero)
        {
            direction = transform.right;
        }

        GameObject newSauce =
            Instantiate(PrefabManager.instance.saucePrefab, transform.position + direction *  -0.5f, transform.rotation);
        Player newPlayer = newSauce.GetComponent<Player>();
        newPlayer.exitedHost = character;
        character.eject();
        newPlayer.leap(direction);
        arm.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public void leap(Vector3 direction = new Vector3()) // leaps forward, or custom direction
    {
        if (direction == Vector3.zero)
        {
            direction = transform.right;
        }

        direction = direction.normalized;


        rb.velocity = direction * Player.ejectForce;
        remainingSlideTime = 0.2f;
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
        if (character is Ranged)
        {
            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
            mouse.z = 10;
            Crosshair.instance.activate();
            Crosshair.instance.transform.position = mouse;
            //Debug.Log("setting cross hair pos to: " + mouse);
        }
        else
        {
            
            Crosshair.instance.deactivate();
        }
    }

    void Attack(Vector3 mouseDir)
    {
        if (character is Ranged ranged)
        {
            ranged.Attack(mouseDir, true);
            character.lastShotTime = Time.time;
        }

        if (character is Melee melee)
        {
            melee.Attack(mouseDir, true);
            alertWitnesses();
        }
    }

    public void alertWitnesses(Controller perpetrator=null)  // called whenever the player does something suss
    {
        if (perpetrator == null)
        {
            perpetrator = this;
        }
        HashSet<Controller> witnesses = CharacterManager.getAllWitnesses(perpetrator);
        foreach (var witness in witnesses)
        {
            ((Human) witness).sussPeople.Add(perpetrator.character);
        }
    }

    public override bool checkVisible(GameObject go, float visionAngle = -1, float visionDistance = -1,
        List<string> layers = null)
    {
        layers = new List<string>();
        layers.AddRange(new[] {"human", "wall", "obstacles", "infected"});
        return base.checkVisible(go, infectionAngle, infectionDistance, layers);
    }
}