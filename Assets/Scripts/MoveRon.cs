using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MoveRon : MonoBehaviour
{
    public float moveDistance;
    public float maxUp, maxDown;
    public float speed;
    bool goDown = true;

    private void Start()
    {
        maxUp = transform.position.y + moveDistance;
        maxDown = transform.position.y - moveDistance;
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < maxDown)
        {
            goDown = false;
        } else if (transform.position.y > maxUp)
        {
            goDown = true;
        }

        if (goDown)
        {
            transform.Translate(0, -moveDistance * speed * Time.deltaTime, 0);
            
        }
        else if (!goDown)
        {
            transform.Translate(0, moveDistance * speed * Time.deltaTime, 0);

        }
    }
}
