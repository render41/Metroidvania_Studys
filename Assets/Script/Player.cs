using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Vars
    //Get Components
    private Rigidbody2D rb;
    private Animator anim;

    //Movement
    [SerializeField] [Range(1, 5)] private float speed = 1.0f;

    //Jump
    [SerializeField] [Range(2, 10)] private float jumpForce = 0.0f;
    [SerializeField] private LayerMask layerGround = default;
    private bool isJump = false;
    private bool doubleJump = false;
    private float distance = 0.5f;

    //Attack
    private Transform pointAttack = default;
    private bool isAtk = false;
    [SerializeField] [Range(0.1f, 0.3f)] private float radius = 0.0f;
    #endregion

    #region Mono Unity
    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.anim = GetComponentInChildren<Animator>();
        this.pointAttack = GameObject.Find("Point Attack").GetComponent<Transform>();
    }

    private void Update()
    {
        Jump();
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region Move
    private void Move()
    {
        float movement = Input.GetAxis("Horizontal");
        this.rb.velocity = new Vector2(movement * this.speed, this.rb.velocity.y);

        if (movement > 0 && !this.isAtk)
        {
            this.transform.eulerAngles = new Vector2(0, 0);
            if (!this.isJump)
            {
                this.anim.SetInteger("transition", 1);
            }
        }
        else if (movement < 0 && !this.isAtk)
        {
            this.transform.eulerAngles = new Vector2(0, 180);
            if (!this.isJump)
            {
                this.anim.SetInteger("transition", 1);
            }
        }

        else if (movement == 0 && !this.isJump && !this.isAtk)
        {
            this.anim.SetInteger("transition", 0);
        }
    }
    #endregion

    #region Jump
    private void Jump()
    {
        var ray = Physics2D.Raycast(this.transform.position, Vector2.down, this.distance, this.layerGround);

        if (Input.GetButtonDown("Jump"))
        {
            if (ray)
            {
                this.rb.AddForce(Vector2.up * this.jumpForce, ForceMode2D.Impulse);
                this.doubleJump = true;
            }

            else if (this.doubleJump)
            {
                this.rb.AddForce(Vector2.up * this.jumpForce, ForceMode2D.Impulse);
                this.doubleJump = false;
            }
        }

        if (ray)
        {
            this.isJump = false;
        }
        else if (!ray)
        {
            this.isJump = true;
            this.anim.SetInteger("transition", 2);
        }
    }
    #endregion

    #region Attack
    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            this.isAtk = true;
            this.anim.SetInteger("transition", 3);
            var hit = Physics2D.OverlapCircle(this.pointAttack.position, this.radius);

            if (hit != null)
            {
                Debug.Log(hit.name);
            }

            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack(){
        yield  return new WaitForSeconds(0.308f);
        this.isAtk = false;
    }
    #endregion

    #region On Draw Gizmos 
    private void OnDrawGizmos()
    {
        if (this.pointAttack != null)
        {
            Gizmos.DrawWireSphere(this.pointAttack.position, this.radius);
        }

    }
    #endregion
}
