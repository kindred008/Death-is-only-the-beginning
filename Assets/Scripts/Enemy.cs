using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] string enemyName;
    [SerializeField] float enemyMoveSpeed;
    [SerializeField] Transform[] waypoints;

    private LevelManager levelManager;

    private Rigidbody2D enemyRb;

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        levelManager.AddDeathToDictionary(enemyName);
    }

	private IEnumerator MovingEnemy()
	{
		Vector2 direction;
		while (true)
		{
			foreach (Transform waypoint in waypoints)
			{
				while ((waypoint.transform.position - transform.position).sqrMagnitude > 0.1)
				{
					direction = (waypoint.transform.position - transform.position).normalized;

					if (direction.x > 0)
						transform.eulerAngles = new Vector2(0, 180);
					else if (direction.x < 0)
						transform.eulerAngles = new Vector2(0, 0);

					transform.Translate(direction * Time.deltaTime * enemyMoveSpeed, Space.World);
					yield return null;
				}
			}
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyRb.bodyType = RigidbodyType2D.Kinematic;
			StartCoroutine(MovingEnemy());
        }
    }
}
