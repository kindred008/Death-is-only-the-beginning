using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] string enemyName;
    [SerializeField] float enemySpeed;
    [SerializeField] Transform[] waypoints;

    private PlayerController playerScript;
    private LevelManager levelManager;

    private Rigidbody2D enemyRb;
    private SpriteRenderer enemyRenderer;

    private int waypointIndex = 0;
    private Transform destination;
    private int direction; // 1 is right, -1 is left

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        levelManager.AddDeathToDictionary(enemyName);
    }

    private void Update()
    {
        if (destination == null)
        {
            destination = waypoints[waypointIndex];
            SetDirection();
        }

        if ((direction == -1 && transform.position.x <= destination.position.x) || (direction == 1 && transform.position.x >= destination.position.x))
        {
            waypointIndex++;
            if (waypointIndex == waypoints.Length)
                waypointIndex = 0;
            destination = waypoints[waypointIndex];
            SetDirection();
        }
    }

    private void FixedUpdate()
    {
        enemyRb.velocity = new Vector2(direction * enemySpeed, enemyRb.velocity.y);
    }

    private void SetDirection()
    {
        if (transform.position.x > destination.position.x)
        {
            direction = -1;
            transform.eulerAngles = new Vector2(0, 0);
        }
        else if (transform.position.x < destination.position.x)
        {
            direction = 1;
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyRb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
