using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAI : MonoBehaviour

{
    private enum State
    {
        Roaming,
        ChasePlayer,
    }

    [SerializeField] private float moveSpeed = 1.5f;
    private Transform pointleft; 
    private Transform pointright; 
    private Transform currentTarget; 
    private bool isFacingRight = true;
    private State state;
    private Vector3 initialPosition;
    Transform player;
   

    void Start()
    {   
        
        // get innitial position
        initialPosition = transform.position;
        player = GameObject.FindWithTag("Player").transform;

        // get left and right position 
        pointleft = new GameObject("PointA").transform;
        pointleft.position = new Vector3(initialPosition.x - 3f, initialPosition.y, initialPosition.z);

        pointright = new GameObject("PointB").transform;
        pointright.position = new Vector3(initialPosition.x + 3f, initialPosition.y, initialPosition.z);

        // set first target to left
        currentTarget = pointleft;
    }

    void Update()
    {
        FindPlayer();

        switch (state){

            case State.Roaming:
                if (currentTarget == player.transform)
                {
                    currentTarget = pointleft;
                }

                transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, currentTarget.position) < 0.5f)
                {
                    SwitchTarget();
                }
                Debug.Log("roaming");
                break;
            case State.ChasePlayer:
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed *1.5f* Time.deltaTime);
                Debug.Log("chasing");
                break;
        }

        Vector3 moveDirection = currentTarget.position - transform.position;
        if (moveDirection.x > 0 && !isFacingRight || moveDirection.x < 0 && isFacingRight)
        {
            Flip();
        }

    }

    void SwitchTarget()
    {
       
        if (currentTarget == pointleft || currentTarget == player.transform)
        {
            currentTarget = pointright;
        }
        else
        {
            currentTarget = pointleft;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void FindPlayer()
    {
        float targetrange = 3f;
        if (Vector3.Distance(initialPosition,player.position) < targetrange)
        {
            state = State.ChasePlayer;
            currentTarget = player.transform;

        }
        else if(Vector3.Distance(initialPosition, player.position) > targetrange)
        {
            state = State.Roaming;
        }
    }


}
