using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private bool isWalk;
    private CharacterController controller;
    [Header("Config Player")]
    public float movementSpeed = 3f;
    private Vector3 direction;
    private float horizontal;
    private float vertical;

    [Header("Attack Config")]
    public ParticleSystem fxAttack;
    public Transform hitBox;
    [Range(0.2f,1f)]
    public float hitRange = 0.5f;
    public LayerMask hitMask;
    public bool isAttack;
    // Objetos que possivelmente entraram em minha area de colisao
    public Collider[] hitInfo;
    public int amountDmg;
    // Start is called before the first frame update

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Inputs();

        MoveCharacter();

        UpdateAnimator();
        
    }

    #region MEUS MÉTODOS

    void Inputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Fire1") && isAttack == false)
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttack = true;
        anim.SetTrigger("Attack");
        fxAttack.Emit(10);
        hitInfo = Physics.OverlapSphere(hitBox.position,hitRange,hitMask);
        foreach(Collider c in hitInfo)
        {
            c.gameObject.SendMessage("GetHit",amountDmg,SendMessageOptions.DontRequireReceiver);
        }
    }

    void AttackIsDone()
    {
        isAttack = false;
    }

    void MoveCharacter()
    {
        direction = new Vector3(horizontal,0,vertical).normalized;

        if(direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,targetAngle,0);
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }

        controller.Move(direction * movementSpeed * Time.deltaTime);
    }

    void UpdateAnimator()
    {
        anim.SetBool("isWalk",isWalk);
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        if(hitBox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitBox.position,hitRange);
        }
        
    }

}
