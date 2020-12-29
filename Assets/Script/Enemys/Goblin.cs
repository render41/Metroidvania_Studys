using UnityEngine;

public class Goblin : MonoBehaviour
{
    #region Vars
    private Rigidbody2D rb = null;
    private Transform pointVision;
    private bool isFront = false;
    private bool isFlip = true;
    private Vector2 direction;

    [SerializeField] private float speed = 0.0f;
    [SerializeField] private float maxVision = 0.0f;
    #endregion

    #region Mono Unity
    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.pointVision = GameObject.Find("PointVision").GetComponent<Transform>();
    }

    private void Start()
    {
        Flip();
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        GetPlayer();
    }
    #endregion

    #region Move
    private void Move()
    {
        if (this.isFront)
        {
            if (this.isFlip)
            {
                this.transform.eulerAngles = new Vector2(0, 0);
                this.direction = Vector2.right;
                this.rb.velocity = new Vector2(this.speed, this.rb.velocity.y);
            }
            else
            {
                this.transform.eulerAngles = new Vector2(0, 180);
                this.direction = Vector2.left;
                this.rb.velocity = new Vector2(-this.speed, this.rb.velocity.y);
            }
        }

    }
    #endregion

    #region Flip
    private void Flip()
    {
        if (this.isFlip)
        {
            this.transform.eulerAngles = new Vector2(0, 0);
            this.direction = Vector2.right;
        }
        else
        {
            this.transform.eulerAngles = new Vector2(0, 180);
            this.direction = Vector2.left;
        }
    }
    #endregion

    #region Get Player
    private void GetPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.pointVision.position, this.direction, this.maxVision);

        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Player"))
            {
                this.isFront = true;

                float distance = Vector2.Distance(this.transform.position, hit.transform.position);

                if (distance <= 0.5f)
                {
                    this.isFront = false;
                    this.rb.velocity = Vector2.zero;
                    
                    hit.transform.GetComponent<Player>().OnHit();
                }
            }
        }
    }
    #endregion

    #region On Draw Gizmos
    private void OnDrawGizmos()
    {
        if (this.pointVision != null)
        {
            Gizmos.DrawRay(this.pointVision.position, Vector2.right * this.maxVision);
        }
    }
    #endregion
}
