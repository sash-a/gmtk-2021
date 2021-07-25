using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePointManager : MonoBehaviour
{
    public static ZombiePointManager instance;
    public static float callDistance = 15;
    public static Dictionary<Controller, ZombiePoint> zombiePointsMap; 
    private void Awake()
    {
        instance = this;
        zombiePointsMap = new Dictionary<Controller, ZombiePoint>();
    }
    
    public static HashSet<Controller> getGroupZombies(Player player, Transform armTip)
    {
        /*
         * gets all zombies which are in line of sight of both the player and the waypoint
         */
        List<string> layers = new List<string>();
        layers.Add("zombie");
        layers.Add("wall");

        HashSet<Controller> groupZombies = new HashSet<Controller>(); 
        foreach (var zombie in CharacterManager.instance.zombies)
        {
            bool canSeePlayer = VisibilityManager.canLookerSeeObject(player.transform, zombie.gameObject, 360, callDistance, layers);
            bool canSeeTip = VisibilityManager.canLookerSeeObject(armTip, zombie.gameObject, 360, callDistance, layers);

            if (canSeePlayer || canSeeTip)
            {
                groupZombies.Add(zombie);
            }
        }

        return groupZombies;
    }

    public static GameObject spawnZombiePoint(Player player, Transform armTip)
    {
        HashSet<Controller> groupedZombies = getGroupZombies(player, armTip);
        if (groupedZombies.Count == 0)
        {
            return null;
        }   
        
        GameObject point = Instantiate(PrefabManager.instance.zombiePoint, armTip.position, player.transform.rotation);
        ZombiePoint zPoint = point.GetComponent<ZombiePoint>();
        updateMap(groupedZombies, zPoint);
        return point;
    }

    public static void updateMap(HashSet<Controller> groupedZombies, ZombiePoint zPoint)
    {
        
        //remove each newly grouped zombie from their old zombie points
        foreach (var zom in groupedZombies)
        {
            if (zombiePointsMap.ContainsKey(zom))
            {
                ZombiePoint point = zombiePointsMap[zom];
                point.removeZombie(zom);
            }
        }

        foreach (Controller zom in groupedZombies) // override the zombie-> point map
        {
            zombiePointsMap[zom] = zPoint;
        }
        
        zPoint.addZombies(groupedZombies);  // adds the new groups to the new zombie point
    }
}
