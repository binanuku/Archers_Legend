using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMgr : MonoBehaviour
{
    public static StageMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageMgr>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("StageMgr");
                    instance = instanceContainer.AddComponent<StageMgr>();
                }
            }
            return instance;
        }
    }
    private static StageMgr instance;

    public GameObject Player;

    [System.Serializable]
    public class StartPositionArray
    {
        public List<Transform> StartPosition = new List<Transform>();
    }

    public StartPositionArray[] startPositionArrays;
    
    public List<Transform> StartPositionAngel = new List<Transform>();
    
    public List<Transform> StartPositionBoss = new List<Transform>();

    public Transform StartPositionLastBoss;

    public int currentStage = 0;
    int LastStage = 20;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void NextStage()
    {
        currentStage++;
        if(currentStage > LastStage)
        {
            UIController.Instance.EndGame();
            return;
        }

        if (currentStage % 5 != 0)
        {
            int arrayIndex = currentStage / 10;
            int randomIndex = Random.Range(0, startPositionArrays[arrayIndex].StartPosition.Count);
            Player.transform.position = startPositionArrays[arrayIndex].StartPosition[randomIndex].position;
            startPositionArrays[arrayIndex].StartPosition.RemoveAt(randomIndex);
        }
        else
        {
            if (currentStage % 10 == 5) 
            {
                int randomIndex = Random.Range(0, StartPositionAngel.Count);
                Player.transform.position = StartPositionAngel[randomIndex].position;
            }
            else
            {
                if (currentStage == LastStage)
                {
                    Player.transform.position = StartPositionLastBoss.position;
                }
                else
                {
                    int randomIndex = Random.Range(0, StartPositionBoss.Count);
                    Player.transform.position = StartPositionBoss[randomIndex].position;
                    StartPositionBoss.RemoveAt(currentStage / 10);
                }
            }
        }
        Camera_Movement.Instance.CameraNextRoom();
    }
}
