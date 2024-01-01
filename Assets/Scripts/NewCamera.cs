using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour
{
    private Camera cam;
    private Transform playerTransform;
    public float moveThreshold = 0.15f;
    public float orthographicWidth;
    public float orthographicHeight;
    private bool camJustMoved = false;
    private Vector3 currentOffset;
    public bool goingRight;
    public bool goingLeft;
    public bool goingUp;
    public bool goingDown;
    public Vector3 currentLockedCamPos;
    public Vector3 futureCamPos;
    public Vector3 playerPos;
    public Vector3 currentLockedPlayerPos;
    public float cameraAccel;
    public float cameraDecel;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        orthographicHeight = cam.orthographicSize * cam.aspect;
        orthographicWidth = orthographicHeight * 2;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = cam.WorldToViewportPoint(playerTransform.position);
        CameraMove();

        if (goingLeft || goingRight || goingDown || goingUp)
        {
            CalculateAccel();
        }

        transform.Translate(currentOffset * Time.unscaledDeltaTime * speed);
        StopMove();
    }

    private void CameraMove()
    {
        // Move Right
        if ((!camJustMoved || goingUp || goingDown) && playerPos.x > 1)
        {
            currentOffset = new Vector3(orthographicWidth, currentOffset.y, 0);
            camJustMoved = true;
            goingRight = true;
            currentLockedCamPos.x = transform.position.x;
            currentLockedPlayerPos.x = playerTransform.position.x;
            futureCamPos.x = transform.position.x + currentOffset.x;
        }
        // Move Left
        else if ((!camJustMoved || goingUp || goingDown) && playerPos.x < 0)
        {
            currentOffset = new Vector3(-orthographicWidth, currentOffset.y, 0);
            camJustMoved = true;
            goingLeft = true;
            currentLockedCamPos.x = transform.position.x;
            currentLockedPlayerPos.x = playerTransform.position.x;
            futureCamPos.x = transform.position.x + currentOffset.x;
        }

        // Move Up
        if ((!camJustMoved || goingLeft || goingRight) && playerPos.y > 1)
        {
            currentOffset = new Vector3(currentOffset.x, orthographicHeight, 0);
            camJustMoved = true;
            goingUp = true;
            currentLockedCamPos.y = transform.position.y;
            currentLockedPlayerPos.y = playerTransform.position.y;
            futureCamPos.y = transform.position.y + currentOffset.y;
        }
        // Move Down
        else if ((!camJustMoved || goingLeft || goingRight) && playerPos.y < 0)
        {
            currentOffset = new Vector3(currentOffset.x, -orthographicHeight, 0);
            camJustMoved = true;
            goingDown = true;
            currentLockedCamPos.y = transform.position.y;
            currentLockedPlayerPos.y = playerTransform.position.y;
            futureCamPos.y = transform.position.y + currentOffset.y;
        }
    }

    private void CalculateAccel()
    {
        float journeyCompletedHorizontal = Mathf.InverseLerp(Mathf.Abs(currentLockedCamPos.x), Mathf.Abs(futureCamPos.x), Mathf.Abs(transform.position.x));
        float journeyCompletedVertical = Mathf.InverseLerp(Mathf.Abs(currentLockedCamPos.y), Mathf.Abs(futureCamPos.y), Mathf.Abs(transform.position.y));
        if (journeyCompletedHorizontal >= 0.75f && (goingRight || goingLeft) || journeyCompletedVertical >= 0.75f && (goingUp || goingDown))
        {
            speed = cameraDecel;
        }
        else
        {
            speed = cameraAccel;
        }
    }
    private void StopMove()
    {
        // Don't Move
        // Left and Right
        Vector3 lockedPlayerScreenPos = cam.WorldToViewportPoint(currentLockedPlayerPos);
        if (goingLeft && (lockedPlayerScreenPos.x > 1 - moveThreshold || transform.position.x < futureCamPos.x))
        {
            goingLeft = false;
            camJustMoved = false;
            currentOffset.x = 0;
        }
        else if (goingRight && (lockedPlayerScreenPos.x < 0 + moveThreshold || transform.position.x > futureCamPos.x))
        {
            goingRight = false;
            camJustMoved = false;
            currentOffset.x = 0;
        }
        // Down and Up
        if (goingDown && (transform.position.y < futureCamPos.y || lockedPlayerScreenPos.y > 1 - moveThreshold))
        {
            goingDown = false;
            camJustMoved = false;
            currentOffset.y = 0;
        }
        else if (goingUp && (transform.position.y > futureCamPos.y || lockedPlayerScreenPos.y < 0 + moveThreshold))
        {
            goingUp = false;
            camJustMoved = false;
            currentOffset.y = 0;
        }
    }
}
