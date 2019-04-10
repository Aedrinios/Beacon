using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconScript : MonoBehaviour
{
    public List<Transform> beaconSpawnList = new List<Transform>();
    public Transform player;

    private int rand;
    private bool canTeleport;

    private void Start()
    {
        canTeleport = false;
        rand = Random.Range(0, beaconSpawnList.Count);
        transform.position = beaconSpawnList[rand].position;
    }

    private void Update()
    {
        if (LosingSightOfPlayer(player) && canTeleport) 
        {
            ChangePosition();
        }
    }

    private bool LosingSightOfPlayer(Transform player)
    {
        Vector3 toPlayer = transform.position - player.position;
        float dot = Vector3.Dot(player.forward, toPlayer.normalized);

        if (dot < 0.65)
        {
            return true;
        }
    
        if(dot > 0.65)
        {
            canTeleport = true;
        }
        return false;
    }

    private void ChangePosition()
    {
        int newRand = Random.Range(0, beaconSpawnList.Count);
        while (newRand == rand)
        {
            newRand = Random.Range(0, beaconSpawnList.Count);
        }
        rand = newRand;
        transform.position = beaconSpawnList[rand].position;
        canTeleport = false;
    }   
}
