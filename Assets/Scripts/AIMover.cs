using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMover : MonoBehaviour
{
    private Transform player;
    public float life = 100;
    private NavMeshAgent NavMesh;

    public void Start()
    {
        GameObject goPlayer = GameObject.FindGameObjectWithTag("Player");
        player = goPlayer.transform;
        NavMesh = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (life <= 0)
            Destroy(gameObject);
    }

    private void Update()
    {
        NavMesh.SetDestination(player.position);
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetFloat("speed", NavMesh.velocity.magnitude);
        }
    }
}