using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int bounceCnt;
    int wallBounceCnt;
    public float dmg;
    Vector3 NewDir;
    Rigidbody Rb;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Rb.velocity = transform.forward * 20f;
        dmg = PlayerData.Instance.dmg;
        bounceCnt = 2;
        wallBounceCnt = 2;
        NewDir = transform.forward;
    }

    Vector3 ResultDir(int idx)
    {
        int closetIndex = -1;
        float closetDis = 500f;
        float currentDis = 0f;

        for (int i = 0; i < Player_Targeting.Instance.MonsterList.Count; i++)
        {
            if (i == idx) continue;

            currentDis = Vector3.Distance(Player_Targeting.Instance.MonsterList[i].transform.position, transform.position);

            if (currentDis > 5f) continue;

            if(closetDis > currentDis)
            {
                closetDis = currentDis;
                closetIndex = i;
            }
        }

        if (closetIndex == -1)
        {
            Destroy(gameObject, 0.2f);
            return Vector3.zero;
        }
        return (Player_Targeting.Instance.MonsterList[closetIndex].transform.position - transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Monster"))
        {
            if (PlayerData.Instance.playerSkill[0] != 0 && Player_Targeting.Instance.MonsterList.Count >= 2)
            {
                int myIndex = Player_Targeting.Instance.MonsterList.IndexOf(other.gameObject);
                Debug.Log(myIndex);
                if (bounceCnt > 0)
                {
                    bounceCnt--;
                    dmg *= 0.7f;
                    NewDir = ResultDir(myIndex) * 20;
                    Rb.velocity = NewDir;
                    return;
                }
            }
            Debug.Log("Stop");
            Rb.velocity = Vector3.zero;
            Destroy(gameObject, 0.2f);
        }
        else if (other.transform.CompareTag("Wall"))
        {
            if (PlayerData.Instance.playerSkill[4] == 0)
            {
                Rb.velocity = Vector3.zero;
                Destroy(gameObject, 0.2f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            if (PlayerData.Instance.playerSkill[4] != 0)
            {
                if(wallBounceCnt > 0)
                {
                    wallBounceCnt--;
                    dmg *= 0.5f;
                    NewDir = Vector3.Reflect(NewDir, collision.contacts[0].normal);
                    Rb.velocity = NewDir * 20f;
                    return;
                }
            }
            Rb.velocity = Vector3.zero;
            Destroy(gameObject, 0.2f);
        }
    }
}
