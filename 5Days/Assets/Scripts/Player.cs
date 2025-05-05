using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : CharacterStatus
{
    [SerializeField] Vector2 moveInput;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    public static Player instance;
    bool isGamePaused;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (SceneTimeController.instance != null)
        SceneTimeController.instance.onPauseGame += PausePlayer;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void PausePlayer()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
        }
    }
    void Update()
    {
        if(isGamePaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        anim.SetFloat("Horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("Vertical", vertical);
        moveInput = new Vector2(horizontal, vertical);
        if(horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (moveInput != Vector2.zero)
        {
            moveInput = moveInput.normalized;
        }
        rb.linearVelocity = moveInput * Speed;
    }
}
