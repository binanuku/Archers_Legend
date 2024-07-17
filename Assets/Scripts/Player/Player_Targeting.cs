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
    float currentDist = 0;      //���� �Ÿ�
    float closetDist = 100f;    //����� �Ÿ�
    float TargetDist = 100f;   //Ÿ�� �Ÿ�
    int closeDistIndex = 0;    //���� ����� �ε���
    public int TargetIndex = -1;      //Ÿ���� �� �ε���
    int prevTargetIndex = 0;
    public LayerMask layerMask;

    float atkSpd = 1.0f;

    public List<GameObject> MonsterList = new List<GameObject>();
    //Monster�� ��� List 

    public GameObject PlayerBullet;  //�߻�ü
    public Transform AtkPoint;

    void OnDrawGizmos()
    {
        if (getATarget)
        {
            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) { return; }// �߰�

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
                Gizmos.DrawRay(transform.position + new Vector3(0, 0.1f, 0), MonsterList[i].transform.GetChild(0).position - transform.position + new Vector3(0, 0.1f, 0));//���� 
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
                if (MonsterList[i] == null) { return; }   // �߰�
                currentDist = Vector3.Distance(transform.position, MonsterList[i].transform.GetChild(0).position);//���� 

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position,//���� 
                                            out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    if (TargetDist >= currentDist)
                    {
                        TargetIndex = i;

                        TargetDist = currentDist;

                        if (!JoyStick_Movement.Instance.isPlayerMoving && prevTargetIndex != TargetIndex)  // ��// �߰�
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
        if (TargetIndex == -1 || MonsterList.Count == 0)  // �߰� 
        {
            Player_Movement.Instance.Anim.SetBool("Throw", false);
            return;
        }
        if (getATarget && !JoyStick_Movement.Instance.isPlayerMoving && MonsterList.Count != 0)
        {
            transform.LookAt(MonsterList[TargetIndex].transform.GetChild(0));     // ����

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
