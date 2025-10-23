using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
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
    public bool isGamePaused;

    [Header("Read-Only")]
    [SerializeField] Vector2 moveInput;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Últimos saves de posição")]
    public Vector2 lastSavedPosition;
    public Vector3 lastSavedBedPos { get; private set; }
    public string lastSavedBedScene { get; private set; }

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
    [ContextMenu("Valores de save da cama")]
    void SaberValoresSaveBed()
    {
        print("Posição da cama salva em: " + lastSavedBedPos);
        print("Cena da cama salva em: " + lastSavedBedScene);
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
    #region SaveMethods
    public void SavePosition() => lastSavedPosition = new Vector2(transform.position.x, transform.position.y - 0.4f);
    public void SaveBedPos() => lastSavedBedPos = new Vector2(transform.position.x, transform.position.y);
    public void SaveBedScene() => lastSavedBedScene = SceneManager.GetActiveScene().name;
    #endregion
    public void LoadOnLastBed()
    {
        SceneManager.LoadScene(lastSavedBedScene);
        print("carregando cena");
        isGamePaused = false;
        transform.position = lastSavedBedPos;
    }
    public void SetGamePauseManual(bool active) => isGamePaused = active;
    #region delegates
    void PausePlayer()
    {
        if (!isGamePaused)
            isGamePaused = true;
        else
            isGamePaused = false;
    }
    void OnNightStart()
    {
        luzNatural?.gameObject.SetActive(true);
    }
    #endregion
    void Update()
    {
        if (Time.timeScale == 0 || isGamePaused) return; //evita que o update seja chamado quando for pausado

        if (canTalk)
        {
            Collider2D collider2D = Physics2D.OverlapCircle(transform.position, raioDeInteração, layerMaskInteração);
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                print("Interagindo com NPC");
                InteragirNPC(collider2D); //antes de checar se pode mover, permite o player a falar com npc
            }
        }

        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Horizontal", 0);
            anim.SetFloat("Vertical", 0);
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
