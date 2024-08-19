using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Variables
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    // Set the initial waypoint
    private void Start()
    {
        target = LevelManager.main.path[pathIndex];
    }

    // Find where the enemy needs to move towards
    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position)<= 0.1f )
        {
            pathIndex++;

            // END GAME if enemy makes it to the end
            if(pathIndex >= LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                QuitGame(); // END THE GAME HERE
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    // Actually move the enemy.
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;
    }
    void QuitGame()
    {
        #if UNITY_EDITOR
        // Simulate quitting in the Unity Editor
        EditorApplication.isPlaying = false;
        #else
        // Quit the application in a build
        Application.Quit();
        #endif
    }
}
