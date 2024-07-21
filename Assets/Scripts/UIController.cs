using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIController>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("UIController");
                    instance = instanceContainer.AddComponent<UIController>();
                }
            }
            return instance;
        }
    }
    private static UIController instance;

    public GameObject JoyStick;
    public GameObject JoyStickPanel;
    public GameObject SlotMachine;
    public GameObject Roulette;
    public GameObject EndGameObj;

    public Text clearRoomCnt;

    public Slider PlayerExpBar;
    public Slider BossHpBar;
    public Slider BossBackHpSlider;
    public bool backHHpHit = false;
    public bool bossRoom = false;

    public GameObject playerLvText;

    public float BossCurrentHp;
    public float BossMaxHp;

    public void PlayerLvUp(bool isSlotMachineOn)
    {
        if(isSlotMachineOn)
        {
            JoyStick.SetActive(false);
            JoyStickPanel.SetActive(false);
            SlotMachine.SetActive(true);
        }
        else
        {
            JoyStick.SetActive(true);
            JoyStickPanel.SetActive(true);
            SlotMachine.SetActive(false);
        }
    }

    public void EndGame()
    {
        JoyStick.gameObject.SetActive(false);
        JoyStickPanel.gameObject.SetActive(false);
        StartCoroutine(EndGamePopUp());
    }

    IEnumerator EndGamePopUp()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        EndGameObj.SetActive(true);
        clearRoomCnt.text = "Clear" + (StageMgr.Instance.currentStage - 1);
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
