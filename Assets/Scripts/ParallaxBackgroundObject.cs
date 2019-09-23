using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundObject : MonoBehaviour
{

    [SerializeField] float moveSpeed = 0.2f;
    [SerializeField] Transform startPos = null;
    [SerializeField] Transform endPos = null;

    Vector3 targetPos;


    void Move()
    {
        targetPos = new Vector2(endPos.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (transform.position == targetPos)
        {
            transform.position = new Vector2(startPos.position.x, transform.position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
