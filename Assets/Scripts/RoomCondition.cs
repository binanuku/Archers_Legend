using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    List<GameObject> MonsterListInRoom = new List<GameObject>();
    public bool playerInThisRoom = false;
    public bool isClearRoom = false;

    private void Update()
    {
        if (playerInThisRoom)
        {
            if (MonsterListInRoom.Count <= 0 && !isClearRoom)
            {
                isClearRoom = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            MonsterListInRoom.Add(other.gameObject);
        }

        if (other.CompareTag("Player")) 
        {
            playerInThisRoom = true;
            Player_Targeting.Instance.MonsterList = new List<GameObject>(MonsterListInRoom);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInThisRoom = false;
        }

        if (other.CompareTag("Monster"))
        {
            MonsterListInRoom.Remove(other.gameObject);
        }
    }
}
