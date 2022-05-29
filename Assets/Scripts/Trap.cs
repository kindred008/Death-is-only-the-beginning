using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] string trapName;
    [SerializeField] bool movingTrap;
    [SerializeField] Transform[] waypoints; // only if a moving trap
    [SerializeField] float trapMoveSpeed;

    private PlayerController playerScript;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        levelManager.AddDeathToDictionary(trapName);

        if (waypoints.Length != 0)
        {
            StartCoroutine(MovingTrap());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript = collision.gameObject.GetComponent<PlayerController>();
            playerScript.PlayerDeath();
            levelManager.UpdateDeathDictionary(trapName);
        }
    }

    private IEnumerator MovingTrap()
    {
        while (true)
        {
            foreach (Transform waypoint in waypoints)
            {
                while ((waypoint.transform.position - transform.position).sqrMagnitude > 0.1)
                {
                    Vector2 direction = (waypoint.transform.position - transform.position).normalized;
                    transform.Translate(direction * Time.deltaTime * trapMoveSpeed);
                    yield return null;
                }
            }
        }
    }
}
