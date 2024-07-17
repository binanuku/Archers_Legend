using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick_Movement : MonoBehaviour
{
    public static JoyStick_Movement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<JoyStick_Movement>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("JoyStick_Movement");
                    instance = instanceContainer.AddComponent<JoyStick_Movement>();
                }
            }
            return instance;
        }
    }
    private static JoyStick_Movement instance;

    public GameObject smallStick;
    public GameObject bGStick;
    Vector3 stickFirstPosition;
    public Vector3 joyVec;
    Vector3 joyStickFirstPosition;
    float stickRadius;

    public bool isPlayerMoving = false;

    void Start()
    {
        stickRadius = bGStick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyStickFirstPosition = bGStick.transform.position;
    }

    public void PointDown()
    {
        bGStick.transform.position = Input.mousePosition;
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;

        isPlayerMoving = true;

        if (isPlayerMoving)
        {
            Player_Movement.Instance.Anim.SetBool("Throw", false);
            Player_Movement.Instance.Anim.SetBool("Run", true);
            Player_Movement.Instance.Anim.SetBool("Idle", false);
        }

        Player_Targeting.Instance.getATarget = false;
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        joyVec = (DragPosition - stickFirstPosition).normalized;

        float stickDistance = Vector3.Distance(DragPosition, stickFirstPosition);

        if (stickDistance < stickRadius)
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
        }
        else
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;
        }
    }

    public void Drop()
    {
        joyVec = Vector3.zero;
        bGStick.transform.position = joyStickFirstPosition;
        smallStick.transform.position = joyStickFirstPosition;

        isPlayerMoving = false;

        if (!isPlayerMoving)
        {
            Player_Movement.Instance.Anim.SetBool("Throw", false);
            Player_Movement.Instance.Anim.SetBool("Run", false);
            Player_Movement.Instance.Anim.SetBool("Idle", true);
        }
    }
}
