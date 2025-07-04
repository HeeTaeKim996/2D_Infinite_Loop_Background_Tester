using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMvoement : MonoBehaviour
{
    private BackgroundMaker backgroundMaker;
    private Rigidbody2D playerRigidbody;

    private bool isMoveInovked = false;
    private float verticalFloat;
    private float horizontalFloat;


    private void Awake()
    {
        backgroundMaker = FindObjectOfType<BackgroundMaker>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.gravityScale = 0;
    }

    private void Update()
    {
        verticalFloat = Input.GetAxis("Vertical");
        horizontalFloat = Input.GetAxis("Horizontal");

        if(verticalFloat != 0 || horizontalFloat != 0)
        {
            isMoveInovked = true;
        }
        else
        {
            isMoveInovked = false;
        }
    }

    private void FixedUpdate()
    {
        if (isMoveInovked)
        {
            Vector2 direction = new Vector2(horizontalFloat, verticalFloat).normalized;
            float power = Mathf.Max(Mathf.Abs(verticalFloat), Mathf.Abs(horizontalFloat));

            playerRigidbody.MovePosition(playerRigidbody.position + direction * power * 17f * Time.fixedDeltaTime);
            backgroundMaker.OnPlayerMove(playerRigidbody.position, verticalFloat >= 0, horizontalFloat >= 0);
        }
    }

}
