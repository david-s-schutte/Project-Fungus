using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    Rigidbody2D rb;
    [SerializeField] private float freeWill;
    Vector3 mousePosition;
    Vector3 moveDirection;
    public GameObject marker;
    WorldGrid world;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mousePosition = transform.position;
        world = GameObject.FindWithTag("WorldGenerator").GetComponent<WorldGenerator>().world;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (willListenToPlayer())
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                world.SetValue(mousePosition, 37);
                world.DebugGrid();
            }
        }

        if (shouldMovePlayer() && !isPlayerMoving())
        {
            moveDirection = mousePosition - transform.position;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rb.AddForce(moveDirection * moveSpeed);
        }
        else if (!shouldMovePlayer())
        {
            rb.velocity = Vector2.zero;
        }
    }

    private bool willListenToPlayer() {
        float playersWill = Random.Range(0, 100);
        return playersWill > freeWill;
    }

    private bool isPlayerMoving()
    {
        return (rb.velocity.x > 0 && rb.velocity.y > 0);
    }

    private bool shouldMovePlayer()
    {
        return ((transform.position.x < mousePosition.x || transform.position.x > mousePosition.x) && 
            (transform.position.y < mousePosition.y) || (transform.position.y < mousePosition.y));
    }

    IEnumerator MovePlayer(float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        while(time < duration)
        {
            rb.AddForce(moveDirection * time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = moveDirection;
    }
}
