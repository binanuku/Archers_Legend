using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerData>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerData");
                    instance = instanceContainer.AddComponent<PlayerData>();
                }
            }
            return instance;
        }
    }
    private static PlayerData instance;

    public float dmg = 500;
    public float maxHp;
    public float currentHp;
    //public GameObject Player;

    //public GameObject playerBullet;
    public GameObject[] playerBullet;

    float PlayerCurrentExp;
    float PlayerLvUpExp;
    int PlayerLv;

    bool playerDead = false;

    public List<int> playerSkill = new List<int>();

    private void Awake()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        if(!playerDead && currentHp <= 0)
        {
            currentHp = 0;
            playerDead = true;
            Player_Movement.Instance.Anim.SetTrigger("Dead");
            UIController.Instance.EndGame();
            return;
        }
    }

    public void PlayerExpCalc(float exp)
    {
        PlayerCurrentExp += exp;
        if (PlayerCurrentExp >= PlayerLvUpExp)
        {
            PlayerLv++;
            PlayerCurrentExp -= PlayerLvUpExp;
            PlayerLvUpExp *= 1.3f;
            StartCoroutine(PlayerLevelUp());
        }
    }

    IEnumerator PlayerLevelUp()
    {
        yield return null;
        EffectSet.Instance.PlayerLevelUpEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        UIController.Instance.PlayerLvUp(true);
        yield return new WaitForSeconds(1.5f);
        EffectSet.Instance.PlayerLevelUpEffect.SetActive(false);
    }
}
