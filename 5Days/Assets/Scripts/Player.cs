using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : CharacterStatus
{
    [Header("Optional")]
    [SerializeField] Light2D luzNatural;
    public bool canMove = true;

    [Header("Interagir Com NPC")]
    [SerializeField] float raioDeInteração = 2;
    [SerializeField] LayerMask layerMaskInteração;
    public Vector2 lastSavedPosition;
    bool isGamePaused;

    [Header("Read-Only")]
    [SerializeField] Vector2 moveInput;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;

    public static Player instance;

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
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //luzNatural = GetComponentInChildren<Light2D>();

        if (SceneTimeController.instance != null)
        SceneTimeController.instance.onPauseGame += PausePlayer;

        if (DiaENoite.instance != null)
            DiaENoite.instance.onNightStart += OnNightStart;
        luzNatural?.gameObject.SetActive(false);
    }
    public void SavePosition()
    {
        lastSavedPosition = new Vector2(transform.position.x, transform.position.y - 0.4f);
    }
    #region delegates
    void PausePlayer()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
        }
        else
        {
            isGamePaused = false;
        }
    }
    void OnNightStart()
    {
            luzNatural?.gameObject.SetActive(true);
    }
    #endregion
    void Update()
    {
        if(isGamePaused || !canMove)
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
        Collider2D collider2D = Physics2D.OverlapCircle(transform.position, raioDeInteração, layerMaskInteração);
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (collider2D != null)
            {
                NPC npc = collider2D.GetComponent<NPC>();
                if(npc != null)
                {
                    npc.Falar();
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, raioDeInteração);
    }
}
