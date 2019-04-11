using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconScript : MonoBehaviour
{
    [FMODUnity.EventRef] //pour fmod
    public string TPSound;
    FMOD.Studio.EventInstance sonTP;

    public List<Transform> beaconSpawnList = new List<Transform>();
    public Transform player;

    public float maxAngle;
    public float maxRadius;

    private int rand;
    public static bool canTeleport;

    void Awake() //pour fmod
    {
        sonTP = FMODUnity.RuntimeManager.CreateInstance(TPSound);
    }

    private void Start()
    {
        sonTP.start();
        canTeleport = false;
        rand = Random.Range(0, beaconSpawnList.Count);
        transform.position = beaconSpawnList[rand].position;
    }

    private void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sonTP, GetComponent<Transform>(), GetComponent<Rigidbody>());

        if (!inFOV(player, transform, maxAngle, maxRadius) && canTeleport)
        {
            sonTP.start();
            ChangePosition();
        }
    }

    public static bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);
        for (int i = 0; i < count + 1; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == target)
                {
                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);
                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
                            if (hit.transform == target)
                            {
                                BeaconScript.canTeleport = true;
                                return true;
                            }
                        }
                    }
                }
            }
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
