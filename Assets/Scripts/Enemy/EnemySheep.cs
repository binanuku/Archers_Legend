using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySheep : Enemy_Base
{
    RoomCondition RoomConditionGo;

    public GameObject DangerMarker;
    public GameObject EnemyBolt;
    public Transform BoltGenPosition;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        RoomConditionGo = transform.parent.transform.parent.gameObject.GetComponent<RoomCondition>();
        StartCoroutine(WaitPlayer());
    }

    IEnumerator WaitPlayer()
    {
        yield return null;

        while(!RoomConditionGo.playerInThisRoom)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        transform.LookAt(Player.transform.position);
        DangerMarkerShoot();

        yield return new WaitForSeconds(2f);
        Shoot();
    }

    void DangerMarkerShoot()
    {
        Vector3 NewPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Physics.Raycast(NewPosition, transform.forward, out RaycastHit hit, 30f, layerMask);

        if (hit.transform.CompareTag("Wall"))
        {
            GameObject DangerMarkerClone = Instantiate(DangerMarker, NewPosition, transform.rotation);
            DangerMarkerClone.GetComponent<DangerLine>().EndPosition = hit.point;
        }
    }

    void Shoot()
    {
        Instantiate(EnemyBolt, BoltGenPosition.position, transform.rotation);
    }
}
