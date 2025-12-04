using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Cinemachine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : CharacterStatus
{
    [Header("Optional")]
    [SerializeField] Joystick joystick;
    public CinemachineVirtualCamera mainCam;
    public GameObject luzNatural;
    public bool canMove = true;

    [Header("Interagir Com NPC")]
    [SerializeField] float raioDeInteração = 2;
    [SerializeField] LayerMask layerMaskInteração;
    public bool canTalk = true;
    public bool isGamePaused;
    public bool isTalking;

    [Header("Read-Only")]
    [SerializeField] Vector2 moveInput;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float originalSpeed;
    [SerializeField] IInteractable lastInteractable;
    //[SerializeField] bool isTired; //porque cansado você nao anda rapido

    [Header("Últimos saves de posição")]
    public Vector2 lastSavedPosition;
    public Vector3 lastSavedBedPos{ get; private set; }
    public string lastSavedBedScene { get; private set; }
    public bool resetBedPosOnLoad = false;

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
        luzNatural.SetActive(false);

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
        
        canTalk = true;
        if (SceneTimeController.instance != null)
        SceneTimeController.instance.onPauseGame += PausePlayer;


    }
    #region SaveMethods
    public void SavePosition(Vector2 newPos = default)
    {
        if (newPos == default)
            lastSavedPosition = new Vector2(transform.position.x, transform.position.y - 0.4f);
        else
            lastSavedPosition = newPos;
    }
    public void SaveBedPos(Vector2 bedPos = default)
    {
        if (bedPos != default)
            lastSavedBedPos = bedPos;
        else
            lastSavedBedPos = new Vector2(transform.position.x, transform.position.y);
    }
    public void SaveBedScene(string bedScene = default)
    {
        print("salvando cena da cama: " + bedScene);
        if (bedScene != default)
            lastSavedBedScene = bedScene;
        else
            lastSavedBedScene = SceneManager.GetActiveScene().name;
    }
    #endregion
    public void LoadOnLastBed(bool resetBedPos)
    {
        if (resetBedPos)
        {
            print("resetando Pos");
            lastSavedBedPos = new Vector2(0, 0);
        }

            if (SceneManager.GetActiveScene().name != lastSavedBedScene)
        {
            print("carregando cena");
            print("lastSavedBedScene: " + lastSavedBedScene);
            SceneManager.LoadScene(lastSavedBedScene);
        }

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
        if (isGamePaused)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Horizontal", 0);
            anim.SetFloat("Vertical", 0);
            return;
        }
        if (Time.timeScale == 0 || isGamePaused) return; //evita que o update seja chamado quando for pausado

        if (canTalk)
        {
            Collider2D collider2D = Physics2D.OverlapCircle(transform.position, raioDeInteração, layerMaskInteração);
            InteragirNPC(collider2D); //antes de checar se pode mover, permite o player a falar com npc
        }

        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Horizontal", 0);
            anim.SetFloat("Vertical", 0);
            return;
        }

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

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
        if (lastInteractable != null && overlapCircle == null)
        {
            print("saindo do range");
            lastInteractable.OnExitRange();
            lastInteractable = null;
            return;
        }

        if(overlapCircle == null) return;
        bool hasComponent = overlapCircle.TryGetComponent(out IInteractable interact);

        if (hasComponent && !isTalking)
        {
            interact.OnReachRange();
            lastInteractable = interact;

        }else if(hasComponent && isTalking)
        {
            if (InputHelper.GetPrimaryDown())
            {
                interact.OnReachRange();
            }
        }
    }
    public void OnDesmaiar()
    {
        originalSpeed = Speed;

        float reducedSpeed = Speed * 0.25f; //10%
        Speed -= reducedSpeed;

        //isTired = true;
        DiaENoite.instance.relogioScript.onAfterNoon.AddListener(OnAfterNoon);
    }
    void OnAfterNoon()
    {
        //isTired = false;
        Speed = originalSpeed;
        DiaENoite.instance.relogioScript.onAfterNoon.RemoveListener(OnAfterNoon);
    }
    public void OnEnable()
    {
        if (DiaENoite.instance != null)
        {
            joystick = DiaENoite.instance.joystick;
            DiaENoite.instance.onNightStart += OnNightStart;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, raioDeInteração);
    }
}
