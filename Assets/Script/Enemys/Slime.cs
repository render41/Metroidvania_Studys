using UnityEngine;

public class Slime : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 0.0f;
    private Transform detectWall;
    [SerializeField] private float radius = 0.0f;
    [SerializeField] private LayerMask layerWall = default;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.detectWall = GameObject.Find("PointWall").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        MovementSlime();
        OnWall();
    }

    private void MovementSlime()
    {
        this.rb.velocity = new Vector2(-this.speed, this.rb.velocity.y);
    }

    private void OnWall()
    {
        var hitWall = Physics2D.OverlapCircle(this.detectWall.position, this.radius, this.layerWall);

        if (hitWall != null)
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
}
