using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : CharacterStatus
{
    [SerializeField] Vector2 moveInput;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical);
        if(moveInput != Vector2.zero)
        {
            moveInput = moveInput.normalized;
        }
        rb.linearVelocity = moveInput * Speed;
    }
}
