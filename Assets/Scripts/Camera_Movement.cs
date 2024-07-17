using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public static Camera_Movement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Camera_Movement>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("Camera_Movement");
                    instance = instanceContainer.AddComponent<Camera_Movement>();
                }
            }
            return instance;
        }
    }
    private static Camera_Movement instance;

    [SerializeField] Transform player;

    Vector3 cameraPosition;

    float offsetY = 15;
    float offsetZ = -10;

    private void Start()
    {
        cameraPosition.x = player.transform.position.x;
    }

    private void Update()
    {
        cameraPosition.y = player.position.y + offsetY;
        cameraPosition.z = player.position.z + offsetZ;
        transform.position = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z);
    }

    public void CameraNextRoom()
    {
        cameraPosition.x = player.transform.position.x;
    }
}
