using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class BasePersonagem : MonoBehaviour, IDamageable
{
    [Header("Status")]
    public int vidaAtual;
    public int vidaMaxima;
    public CharacterStatusGeneric characterStatus;

    [Header("Opcional")]
    [SerializeField] public Slider lifeBar;
    public GameObject shield;
    public Transform shadow;

    [Header("Read-Only")]
    public bool isBuffed;
    public float strengthFactor = 1; //futuramente: fazer stackar com outras coisas como Grito de Guerra
    [Tooltip("A quantidade de força que o golpe será multiplicado, usado apenas em golpes buffados")]
    public float duration = 2;
    public bool turnEnded;
    public List<Effect> efeitosAtivos;
    public StatusEffect statusEffect;

    //variaveis privadas
    public Animator animator;
    Aliados aliado;
    TextMeshProUGUI lifeText;
    TurnModeManager turnModeManager;
    void Awake()
    {
        if (shadow != null) shadow.gameObject.SetActive(false);
    }
    void Start()
    {
        turnModeManager = TurnModeManager.instance;
        animator = GetComponent<Animator>();
        aliado = GetComponent<Aliados>();
    }
    #region EffectInvolved
    public void OnTurnStart()
    {
        List<Effect> efeitos = new List<Effect>(efeitosAtivos);
        for(int i = 0; i < efeitos.Count; i++)
        {
            if(efeitos[i] != null)
            efeitos[i].OnTurnStart(this);
        }
    }
    public (bool jaTem, Effect efeitoQueJaPossui) ChecarSeJaPossuiEfeito(Effect effectToCheck)
    {
        for(int i = 0; i < efeitosAtivos.Count; i++)
        {
            Effect effect = efeitosAtivos[i];
            
            if (effectToCheck.GetType() == effect.GetType()) //se o tipo do efeito for o mesmo do efeito que tem
            return (true, efeitosAtivos[i]);
        }   
        return (false, null);
    }
    #endregion
    public void SetupStatus()
    {
        EnemyAI enemy = GetComponent<EnemyAI>();
        Animator animato = GetComponent<Animator>();
        vidaAtual = characterStatus.vidaAtual;
        vidaMaxima = characterStatus.vidaMaxima;
        if (characterStatus.characterSprite != null)
            GetComponent<SpriteRenderer>().sprite = characterStatus.characterSprite;
        if(animato!= null) animato.runtimeAnimatorController = characterStatus.animatorController;
        if (enemy != null) enemy.ataques = characterStatus.ataques;

        if (lifeBar != null)
        {
            lifeText = lifeBar.GetComponentInChildren<TextMeshProUGUI>();
            lifeBar.gameObject.SetActive(true);
            lifeBar.maxValue = vidaMaxima;
            AtualizarVida();
        }
    }
        public void TakeDamage(int damage, bool shakeCamera, bool isMoveStrong = false)
        {
        if (ChecarSePossuiVida())
        {
            if (aliado != null && aliado.isDefending)
            {
                damage = damage / 2;
            }
            vidaAtual -= damage;

            string texto = ("-" + damage.ToString() + (isMoveStrong ? "!" : ""));
            TextPopup.instance.GerarTexto(texto, transform.position, Color.red);

            StartCoroutine(ShakeEffect.instance.Shake(gameObject, 0.25f, 0.05f));

            if(shakeCamera)
            StartCoroutine(ShakeEffect.instance.Shake(TurnModeManager.instance.mainCamera.gameObject, 0.25f, 0.09f, true));

            AtualizarVida();
        }
        if(!ChecarSePossuiVida())
        { 
            AtualizarVida();
            characterStatus.isDead = true;  
            if(aliado != null)
            aliado.shield.SetActive(false);
            turnModeManager.aliadosPersonagens.Remove(this);
            turnModeManager.aliados.Remove(gameObject.GetComponent<Aliados>());
            turnModeManager.inimigosPersonagens.Remove(this);
            turnModeManager.inimigos.Remove(gameObject.GetComponent<EnemyAI>());
            gameObject.SetActive(false);
        }
    }
    public void EndTurn()
    {
        turnEnded = true;
        turnModeManager.CheckIfAllCharactersAttacked();
    }
    #region LifeInvolved
    public void AtualizarVida()
    {
        if (ChecarSePossuiVida())
        {
            lifeBar.value = vidaAtual;
            lifeText.text = vidaAtual.ToString() + " / " + vidaMaxima.ToString();
        }
        else
        {
            lifeBar.gameObject.SetActive(false);
            if(shadow != null)
            shadow.gameObject.SetActive(false);
        }
    }
    bool ChecarSePossuiVida()
    {
        if (vidaAtual > 0) return true;
        else if (vidaAtual <= 0) return false;
        return false;
    }
    #endregion
}
