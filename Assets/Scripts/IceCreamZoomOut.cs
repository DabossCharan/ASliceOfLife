using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamZoomOut : MonoBehaviour
{
    public float maxCamSize;
    public float speed;
    private Camera cam;
    public Vector2 targetPos;
    public bool startEndScene;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, maxCamSize, Time.deltaTime * speed);
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * speed), Mathf.Lerp(transform.position.y, targetPos.y, Time.deltaTime * speed), -10);
        if (cam.orthographicSize > maxCamSize - 5)
        {
            startEndScene = true;
        }
    }
}