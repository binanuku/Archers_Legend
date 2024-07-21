using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public static Player_Movement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player_Movement>();
                if (instance == null )
                {
                    var instanceContainer = new GameObject("Player_Movement");
                    instance = instanceContainer.AddComponent<Player_Movement>();
                }
            }
            return instance;
        }
    }
    private static Player_Movement instance;

    Rigidbody rb;
    public float moveSpeed = 5f;

    [HideInInspector] public Animator Anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (JoyStick_Movement.Instance.joyVec.x != 0 || JoyStick_Movement.Instance.joyVec.y != 0)
        {
            rb.velocity = new Vector3(JoyStick_Movement.Instance.joyVec.x, rb.velocity.y, JoyStick_Movement.Instance.joyVec.y) * moveSpeed;
            rb.rotation = Quaternion.LookRotation(new Vector3(JoyStick_Movement.Instance.joyVec.x, 0, JoyStick_Movement.Instance.joyVec.y));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("NextRoom"))
            StageMgr.Instance.NextStage();

        if (other.transform.CompareTag("HpBooster"))
            PlayerHpBar.Instance.GetHpBoost();

        if (other.transform.CompareTag("MeleeAtk"))
        {
            other.transform.parent.GetComponent<EnemyDuck>().meleeAtkArea.SetActive(false);
            PlayerData.Instance.currentHp -= other.transform.parent.GetComponent<EnemyDuck>().damage;

            if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
            {
                Anim.SetTrigger("Damaged");
                Instantiate(EffectSet.Instance.PlayerDmgEffect, Player_Targeting.Instance.AtkPoint.position, Quaternion.Euler(90, 0, 0));
            }
            Anim.SetBool("Idle", true);
        }

        if(Player_Targeting.Instance.MonsterList.Count <= 0 && other.transform.CompareTag("EXP"))
        {
            PlayerData.Instance.PlayerExpCalc(100f);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
