using UnityEngine;

public class Slime : MonoBehaviour
{
    #region Vars
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float speed = 0.0f;
    private Transform detectCollider;
    [SerializeField] private float radius = 0.0f;
    [SerializeField] private LayerMask layerHit = default;

    private int health = 2;
    #endregion

    #region Mono Unity
    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.detectCollider = GameObject.Find("PointWall").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        MovementSlime(true);
        OnCollider();
    }
    #endregion

    #region Movement Slime
    private void MovementSlime(bool isMove)
    {
        if (isMove)
        {
            this.rb.velocity = new Vector2(-this.speed, this.rb.velocity.y);
        }
        else
        {
            this.speed = 0.0f;
        }
    }
    #endregion

    #region On Collider
    private void OnCollider()
    {
        var hitCollider = Physics2D.OverlapCircle(this.detectCollider.position, this.radius, this.layerHit);

        if (hitCollider != null)
        {
            this.speed = -this.speed;

            if (this.transform.eulerAngles.y == 0)
            {
                this.transform.eulerAngles = new Vector2(0, 180);
            }
            else
            {
                this.transform.eulerAngles = new Vector2(0, 0);
            }
        }
    }
    #endregion

    #region On Hit
    public void OnHit()
    {
        this.anim.SetTrigger("hit");
        this.health--;

        if (this.health <= 0)
        {
            MovementSlime(false);
            this.anim.SetTrigger("dead");
            Destroy(this.gameObject, 0.500f);
        }
    }
    #endregion
}
