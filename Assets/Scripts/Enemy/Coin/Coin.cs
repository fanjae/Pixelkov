using UnityEngine;

public class Coin : MonoBehaviour
{
    private Rigidbody2D rb;  

    void Start()
    {
        Jump();
        Destroy(gameObject, 1.0f);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Jump()
    {

        //위로 랜덤하게 점프
        float randomJumpForce = Random.Range(4f, 8f);
        Vector2 jumpVelocity = Vector2.up * randomJumpForce;

        //좌우 랜덤하게 
        jumpVelocity.x = Random.Range(-2f, 2f);


        //코인 AddForce
        rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
    }
    
    
}
