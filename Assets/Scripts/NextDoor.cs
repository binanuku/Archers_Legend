using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageMgr.Instance.NextStage();
        }
    }
}
