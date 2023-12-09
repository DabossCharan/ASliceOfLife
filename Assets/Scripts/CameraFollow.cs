using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float maxLeft;
    private float maxRight;
    private float maxTop;
    public float maxBottom; // just to create a buffer if we decide to make deaths a thing
    public float moveThreshold;
    private GameObject player;
    private Transform playerPos;
    private PlayerMovement playerMovement;
    private Camera cam;
    public Vector3 playerScreenPos;
    public Vector3 playerWorldPos;
    public bool moveCam = false;
    public bool canMoveCam = true;
    private Vector3 currentOffset = new Vector3(0, 0, 0);
    private float midpoint;
    private float verticalMidpoint;
    public float acceleration;
    public float deceleration;
    private float speed;
    private float verticalSpeed;
    private bool isAtMid;
    private bool isAtVerticalMid;
    public float timeSpeed; // MUST BE BETWEEN 0 and 1

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.GetComponent<Transform>();
        playerMovement = player.GetComponent<PlayerMovement>();
        

        maxTop = cam.pixelHeight; // just to create a buffer if we decide to make deaths a thing
        maxLeft = 0;
        maxRight = cam.pixelWidth;

        midpoint = maxRight / 2;
        verticalMidpoint = maxTop / 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerMovement.horizontalInput != 0 || !playerMovement.grounded)
        {
            canMoveCam = true;
        }

        CapturePlayerPos();
        CheckSide();
    }

    private void CapturePlayerPos()
    {
        if (!moveCam && canMoveCam)
        {
            playerWorldPos = player.transform.position;
        }
    }

    private void CheckSide()
    {
        playerScreenPos = cam.WorldToScreenPoint(playerWorldPos); // Somewhat working - playerPos.position

        if (canMoveCam)
        {
            bool goingLeft = playerScreenPos.x < maxLeft + moveThreshold;
            bool goingRight = playerScreenPos.x > maxRight - moveThreshold;
            bool goingUp = playerScreenPos.y > maxTop - moveThreshold;
            bool goingDown = playerScreenPos.y < maxBottom + moveThreshold;
            if (goingLeft || goingRight)
            {
                canMoveCam = false;
                moveCam = true;
                Time.timeScale = timeSpeed;
                currentOffset = new Vector3(playerPos.position.x - transform.position.x, currentOffset.y, 0);
            }

            if (goingUp || goingDown)
            {
                canMoveCam = false;
                moveCam = true;
                Time.timeScale = timeSpeed;
                currentOffset = new Vector3(currentOffset.x, playerPos.position.y - transform.position.y, 0);
            }
        }

        // Horizontal
        if (moveCam && currentOffset.x < 0)
        {
            if (playerScreenPos.x < maxRight - moveThreshold && playerScreenPos.x > maxRight - 2 * moveThreshold)
            {
                moveCam = false;
                canMoveCam = true;
                currentOffset = new Vector3(0, 0, 0);
                Time.timeScale = 1;
            }
            isAtMid = (playerScreenPos.x > midpoint);
        } 
        else if (moveCam && currentOffset.x > 0)
        {
            if (playerScreenPos.x > maxLeft + moveThreshold && playerScreenPos.x < maxLeft + 2 * moveThreshold)
            {
                moveCam = false;
                canMoveCam = false;
                currentOffset = new Vector3(0, 0, 0);
                Time.timeScale = 1;
            }
            isAtMid = (playerScreenPos.x < midpoint);
        }

        // Vertical
        if (moveCam && currentOffset.y < 0)
        {
            if (playerScreenPos.y < maxTop - moveThreshold && playerScreenPos.y > maxTop - 2 * moveThreshold)
            {
                moveCam = false;
                canMoveCam = true;
                currentOffset = new Vector3(0, 0, 0);
                Time.timeScale = 1;
            }
            isAtVerticalMid = (playerScreenPos.y > verticalMidpoint);
        }
        else if (moveCam && currentOffset.y > 0)
        {
            if (playerScreenPos.y > maxBottom + moveThreshold && playerScreenPos.y < maxBottom + 2 * moveThreshold)
            {
                moveCam = false;
                canMoveCam = false;
                currentOffset = new Vector3(0, 0, 0);
                Time.timeScale = 1;
            }
            isAtVerticalMid = (playerScreenPos.y < verticalMidpoint);
        }

        if (isAtMid)
        {
            speed = deceleration;
        }
        else
        {
            speed = acceleration;
        }

        if (isAtVerticalMid)
        {
            verticalSpeed = deceleration;
        }
        else
        {
            verticalSpeed = acceleration;
        }

        transform.Translate(currentOffset.x * Time.unscaledDeltaTime * speed, currentOffset.y * Time.unscaledDeltaTime * verticalSpeed, 0);
    }
}
