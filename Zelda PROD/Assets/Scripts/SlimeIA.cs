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
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        switch(newState)
        {
            case enemyState.IDLE:
            break;
            case enemyState.ALERT:
            break;
        }
    }
    #endregion
}
