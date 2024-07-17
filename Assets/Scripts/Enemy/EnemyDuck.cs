using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDuck : EnemyMeleeFSM
{
    public GameObject enemyCanvasGo;
    public GameObject meleeAtkArea;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRealizeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Start()
    {
        base.Start();
        attackCoolTime = 2f;
        attackCoolTimeCacl = attackCoolTime;

        attackRange = 3f;
        nvAgent.stoppingDistance = 1f;

        StartCoroutine(ResetAtkArea());
    }

    IEnumerator ResetAtkArea()
    {
        while (true)
        {
            yield return null;
            if(!meleeAtkArea.activeInHierarchy && currentState == State.Attack)
            {
                yield return new WaitForSeconds(attackCoolTime);
                meleeAtkArea.SetActive(true);
            }
        }
    }

    protected override void InitMonster()
    {
        maxHp += (StageMgr.Instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        damage += (StageMgr.Instance.currentStage + 1) * 100f;
    }

    protected override void AtkEffect()
    {
        Instantiate(EffectSet.Instance.DuckAtkEffect, transform.position, Quaternion.Euler(90, 0, 0));
    }

    void Update()
    {
        if (currentHp <= 0)
        {
            nvAgent.isStopped = true;

            rb.gameObject.SetActive(false);
            Player_Targeting.Instance.MonsterList.Remove(transform.parent.gameObject);
            Player_Targeting.Instance.TargetIndex = -1;
            Destroy(transform.parent.gameObject);
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Bullet"))
        {
            enemyCanvasGo.GetComponent<EnemyHpBar>().Dmg();
            currentHp -= 250f;
            Instantiate(EffectSet.Instance.DuckDmgEffect, collision.contacts[0].point, Quaternion.Euler(90, 0, 0));
        }
    }
}
