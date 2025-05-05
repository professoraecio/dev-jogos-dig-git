using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIA : MonoBehaviour
{
    private Animator anim;
    public ParticleSystem hitEffect;
    public int HP = 3;
    public bool isDie = false;
    public enemyState state;
    public const float idleWaitTime = 3f;
    public const float patrolWaitTime = 5f;
    private int rand;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
                StartCoroutine("IDLE");
            break;
            case enemyState.ALERT:
                StartCoroutine("PATROL");
            break;
        }
    }

    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(idleWaitTime);

        if(Rand() <= 50)
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
