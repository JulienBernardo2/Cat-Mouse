using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class CatEnemyScript : MonoBehaviour
{
    public GameObject player;
    public float maxAngle = 45;
    public float maxDistance = 2;
    public float timer = 1.0f;
    public float visionCheckRate = 1.0f;
    private NavMeshAgent agent;

    public Transform[] point;
    private int destPoint;
    public int walkSpeed = 4;
    public int runSpeed = 9;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.speed = walkSpeed;
        GoToNextPoint();
    }

    void Update()
    {
        if (SeePlayer())
        {
            agent.speed = runSpeed;
            Vector3 position = player.transform.position;
            agent.destination = position;
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPoint();
            }
        }
    }

    public bool SeePlayer()
    {
        Vector3 vecPlayerTurret = player.transform.position - transform.position;
        if (vecPlayerTurret.magnitude > maxDistance)
        {
            return false;
        }
        Vector3 normVecPlayerTurret = Vector3.Normalize(vecPlayerTurret);
        float dotProduct = Vector3.Dot(transform.forward, normVecPlayerTurret);
        var angle = Mathf.Acos(dotProduct);
        float deg = angle * Mathf.Rad2Deg;
        if (deg < maxAngle)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, normVecPlayerTurret);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }

            }
        }
        return false;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void GoToNextPoint()
    {
        if (point.Length == 0)
        {
            return;
        }

        agent.destination = point[destPoint].position;
        destPoint = (destPoint + 1) % point.Length;
    }
}
