using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class EnemyPenguin : Enemy_Base
{
    RoomCondition RoomConditionGo;
    LineRenderer lr;

    public Transform BoltGenPosition;
    public GameObject EnemyBolt;
    public bool lookAtPlayer = true;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");
        RoomConditionGo = transform.parent.transform.parent.gameObject.GetComponent<RoomCondition>();

        lr.startColor = new Color(1, 0, 0, 0.5f);
        lr.endColor = new Color(1, 0, 0, 0.5f);
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;

        StartCoroutine(WaitPlayer());
    }

    IEnumerator WaitPlayer()
    {
        yield return null;

        while (!RoomConditionGo.playerInThisRoom)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SetTarget());

        yield return new WaitForSeconds(2f);
        DangerMarkerDeactive();
        Shoot();
    }

    IEnumerator SetTarget()
    {
        while (true)
        {
            yield return null;
            if (!lookAtPlayer) break;

            Debug.Log(" Set Player.transform.position ");
            transform.LookAt(Player.transform.position);
            DangerMarkerShoot();
        }
    }

    public void DangerMarkerShoot()
    {
        Vector3 NewPosition = BoltGenPosition.position;
        Vector3 NewDir = transform.forward;
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);
        for(int i = 1; i < 4; i++)
        {
            Physics.Raycast(NewPosition, NewDir, out RaycastHit hit, 30f, layerMask);

            lr.positionCount++;
            lr.SetPosition(i, hit.point);

            NewPosition = hit.point;
            NewDir = Vector3.Reflect(NewDir, hit.normal);
        }
    }

    public void DangerMarkerDeactive()
    {
        lookAtPlayer = false;

        for (int i = 0; i < lr.positionCount; i++)
        {
            lr.SetPosition(i, Vector3.zero);
        }
        lr.positionCount = 0;
    }

    public void Shoot()
    {
        Vector3 CurrentRotation = transform.eulerAngles + new Vector3(-90, 0, 0);
        Instantiate(EnemyBolt, BoltGenPosition.position, Quaternion.Euler(CurrentRotation));
    }
}
