using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Targeting : MonoBehaviour
{
    public static Player_Targeting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player_Targeting>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("Player_Targeting");
                    instance = instanceContainer.AddComponent<Player_Targeting>();
                }
            }
            return instance;
        }
    }
    private static Player_Targeting instance;

    public bool getATarget = false;
    float currentDist = 0;      //현재 거리
    float closetDist = 100f;    //가까운 거리
    float TargetDist = 100f;   //타겟 거리
    int closeDistIndex = 0;    //가장 가까운 인덱스
    public int TargetIndex = -1;      //타겟팅 할 인덱스
    int prevTargetIndex = 0;
    public LayerMask layerMask;

    float atkSpd = 1.0f;

    public List<GameObject> MonsterList = new List<GameObject>();
    //Monster를 담는 List 

    public GameObject PlayerBullet;  //발사체
    public Transform AtkPoint;

    void OnDrawGizmos()
    {
        if (getATarget)
        {
            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) { return; }// 추가

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position, out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawRay(transform.position + new Vector3(0, 0.1f, 0), MonsterList[i].transform.GetChild(0).position - transform.position + new Vector3(0, 0.1f, 0));//변경 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetTarget();
        AtkTarget();
    }

    void Attack()
    {
        Player_Movement.Instance.Anim.SetFloat("AttackSpeed", atkSpd );
        Instantiate(PlayerBullet, AtkPoint.position, transform.rotation);
    }

    void SetTarget()
    {
        if (MonsterList.Count != 0)
        {
            prevTargetIndex = TargetIndex;
            currentDist = 0f;
            closeDistIndex = 0;
            TargetIndex = -1;

            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) { return; }   // 추가
                currentDist = Vector3.Distance(transform.position, MonsterList[i].transform.GetChild(0).position);//변경 

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position,//변경 
                                            out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    if (TargetDist >= currentDist)
                    {
                        TargetIndex = i;

                        TargetDist = currentDist;

                        if (!JoyStick_Movement.Instance.isPlayerMoving && prevTargetIndex != TargetIndex)  // 추// 추가
                        {
                            TargetIndex = prevTargetIndex;
                        }
                    }
                }

                if (closetDist >= currentDist)
                {
                    closeDistIndex = i;
                    closetDist = currentDist;
                }
            }

            if (TargetIndex == -1)
            {
                TargetIndex = closeDistIndex;
            }
            closetDist = 100f;
            TargetDist = 100f;
            getATarget = true;
        }

    }

    void AtkTarget()
    {
        if (TargetIndex == -1 || MonsterList.Count == 0)  // 추가 
        {
            Player_Movement.Instance.Anim.SetBool("Throw", false);
            return;
        }
        if (getATarget && !JoyStick_Movement.Instance.isPlayerMoving && MonsterList.Count != 0)
        {
            transform.LookAt(MonsterList[TargetIndex].transform.GetChild(0));     // 변경

            if (Player_Movement.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Player_Movement.Instance.Anim.SetBool("Throw", true);
                Player_Movement.Instance.Anim.SetBool("Idle", false);
                Player_Movement.Instance.Anim.SetBool("Run", false);
            }

        }
        else if (JoyStick_Movement.Instance.isPlayerMoving)
        {
            if (!Player_Movement.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                Player_Movement.Instance.Anim.SetBool("Throw", false);
                Player_Movement.Instance.Anim.SetBool("Idle", false);
                Player_Movement.Instance.Anim.SetBool("Run", true);
            }
        }
        else
        {
            Player_Movement.Instance.Anim.SetBool("Throw", false);
            Player_Movement.Instance.Anim.SetBool("Idle", true);
            Player_Movement.Instance.Anim.SetBool("Run", false);
        }
    }
}
