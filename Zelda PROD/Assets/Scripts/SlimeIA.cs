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
    //public const float idleWaitTime = 3f;
    //public const float patrolWaitTime = 5f;
    private int rand;
    private NavMeshAgent agent;
    private Vector3 destination;
    private int idWaypoint;
    private bool isWalk;
    private bool isAlert;
    private bool isPlayerVisible;
    private bool isAttack;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = transform.position;
        anim = GetComponent<Animator>();
        _gm = FindObjectOfType(typeof(GameManager)) as GameManager;
        //ChangeState(state);
    }

    // Update is called once per frame
    void Update()
    {
        StateManager();
        if(agent.desiredVelocity.magnitude >= 0.1f)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }
        anim.SetBool("isWalk",isWalk);
        anim.SetBool("isAlert",isAlert);
    }

    IEnumerator Died()
    {
        isDie = true;
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && (state == enemyState.IDLE || state == enemyState.PATROL))
        {
            isPlayerVisible = true;
            ChangeState(enemyState.ALERT);
        }
    }

    private void OnggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerVisible = false;
        }
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
            ChangeState(enemyState.FURY);
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
                destination = _gm.player.position;
                agent.destination = destination;
            break;
            case enemyState.FOLLOW:
                destination = _gm.player.position;
                agent.destination = destination;
                if(agent.remainingDistance <= agent.stoppingDistance)
                {
                    Attack();
                }
            break;
            case enemyState.FURY:
                destination = _gm.player.position;
                agent.destination = destination;
            break;
            case enemyState.PATROL:
            break;
        }
        //state = newState;
    }
    void ChangeState(enemyState newState)
    {
        state = newState;
        print(newState);
        StopAllCoroutines();
        isAlert = false;
        switch(state)
        {
            case enemyState.IDLE:
                agent.stoppingDistance = 0;
                destination = transform.position;
                print(agent.destination);
                agent.destination = destination;
                StartCoroutine("IDLE");
            break;
            case enemyState.ALERT:
                agent.stoppingDistance = 0;
                destination = transform.position;
                agent.destination = destination;
                isAlert = true;
                StartCoroutine("ALERT");
            break;
            case enemyState.PATROL:
                agent.stoppingDistance = 0;
                idWaypoint = Random.Range(0,_gm.slimeWayPoints.Length);
                destination = _gm.slimeWayPoints[idWaypoint].position;
                agent.destination = destination;
                StartCoroutine("PATROL");
                
            break;
            case enemyState.FOLLOW:
                isAttack = true;
                agent.stoppingDistance = _gm.slimeDistanceToAttack;
                StartCoroutine("FOLLOW");
                StartCoroutine("ATTACK");
            break;
            case enemyState.FURY:
                destination = transform.position;
                agent.stoppingDistance = _gm.slimeDistanceToAttack;
                agent.destination = destination;
            break;
        }
    }

    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(_gm.slimeIdleWaitTime);
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
        //yield return new WaitForSeconds(patrolWaitTime);
        yield return new WaitUntil(() => agent.remainingDistance <= 0);
        StayStill(30);
    }

    IEnumerator ALERT()
    {
        yield return new WaitForSeconds(_gm.slimeAlertTime);
        if(isPlayerVisible == true)
        {
            ChangeState(enemyState.FOLLOW);
        }
        else
        {
            StayStill(10);
        }
    }

    IEnumerator ATTACK()
    {
        yield return new WaitForSeconds(_gm.slimeAttackDelay);
        isAttack = false;
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

    void Attack()
    {
        if(isAttack == false)
        {
            isAttack = true;
            anim.SetTrigger("Attack");
        }
    }

    void AttackIsDone()
    {
        //isAttack = false;
        StartCoroutine("ATTACK");
    }

    #endregion
}
