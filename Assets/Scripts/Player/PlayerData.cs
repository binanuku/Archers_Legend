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
    //public float maxHp;
    //public float currentHp;
    //public GameObject Player;

    //public GameObject playerBullet;
    public GameObject[] playerBullet;

    public List<int> playerSkill = new List<int>();
    
}
