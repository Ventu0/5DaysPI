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
    public bool canTalk = true;
    bool isGamePaused;

    [Header("Read-Only")]
    [SerializeField] Vector2 moveInput;
    public Vector2 lastSavedPosition;
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

        canTalk = true;
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
        if (Time.timeScale == 0) return; //evita que o update seja chamado quando for pausado

        if (canTalk)
        {
            Collider2D collider2D = Physics2D.OverlapCircle(transform.position, raioDeInteração, layerMaskInteração);
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                print("Interagindo com NPC");
                InteragirNPC(collider2D); //antes de checar se pode mover, permite o player a falar com npc
            }
        }

        if (isGamePaused || !canMove)
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
            spriteRenderer.flipX = false;
        else if (horizontal < 0)
            spriteRenderer.flipX = true;

        if (moveInput != Vector2.zero)
        {
            moveInput = moveInput.normalized;
        }
        rb.linearVelocity = moveInput * Speed;
    }
    void InteragirNPC(Collider2D overlapCircle)
    {
        if (overlapCircle != null)
        {
            NPC npc = overlapCircle.GetComponent<NPC>();
            if (npc != null)
            {
                npc.Falar();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, raioDeInteração);
    }
}
