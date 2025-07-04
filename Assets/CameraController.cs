using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerMvoement playerMovement;

    private float z;
    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMvoement>();
        z = transform.position.z;
    }


    private void LateUpdate()
    {
        Vector2 playerPos = playerMovement.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, z);
    }
}
