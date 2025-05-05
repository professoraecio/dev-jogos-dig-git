using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeIA : MonoBehaviour
{
    private GameManager _gm;
    private Animator anim;
    public ParticleSystem hitEffect;
    public int HP = 3;
    public bool isDie = false;
    public enemyState state;
    public const float idleWaitTime = 3f;
    public const float patrolWaitTime = 5f;
    private int rand;
    private NavMeshAgent agent;
    private Vector3 destination;
    private int idWaypoint;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = transform.position;
        anim = GetComponent<Animator>();
        _gm = FindObjectOfType(typeof(GameManager)) as GameManager;
        ChangeState(state);
    }

    // Update is called once per frame
    void Update()
    {
        StateManager();
    }

    IEnumerator Died()
    {
        isDie = true;
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    #region 
    void GetHit(int amount)
    {
        if(isDie == true)
        {
            return;
        }
        HP -= amount;
        if(HP > 0)
        {
            anim.SetTrigger("GetHit");
            hitEffect.Emit(50);
        }
        else
        {
            anim.SetTrigger("Die");
            StartCoroutine("Died");
        }
        
    }
    void StateManager()
    {
        switch(state)
        {
            case enemyState.IDLE:
            break;
            case enemyState.ALERT:
            break;
            case enemyState.EXPLORE:
            break;
            case enemyState.FOLLOW:
            break;
            case enemyState.FURY:
            break;
            case enemyState.PATROL:
            break;
        }
    }
    void ChangeState(enemyState newState)
    {
        state = newState;
        print(newState);
        StopAllCoroutines();
        switch(state)
        {
            case enemyState.IDLE:
                destination = transform.position;
                agent.destination = destination;
                StartCoroutine("IDLE");
            break;
            case enemyState.ALERT:
                idWaypoint = Random.Range(0,_gm.slimeWayPoints.Length);
                destination = _gm.slimeWayPoints[idWaypoint].position;
                agent.destination = destination;
                StartCoroutine("PATROL");
            break;
        }
    }

    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(idleWaitTime);
        /*
        if(Rand() <= 50)
        {
            ChangeState(enemyState.IDLE);
        }
        else
        {
            ChangeState(enemyState.PATROL);
        }
        */
        StayStill(50);
    }

    IEnumerator PATROL()
    {
        yield return new WaitForSeconds(patrolWaitTime);
        StayStill(30);
    }

    void StayStill(int yes)
    {
        if(Rand() <= yes)
        {
            ChangeState(enemyState.IDLE);
        }
        else
        {
            ChangeState(enemyState.PATROL);
        }
    }

    int Rand()
    {
        rand = Random.Range(0,100);
        return rand;
    }

    #endregion
}
