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
    public float strengthFactor = 1; //futuramente: fazer stackar com outras coisas como Grito de Guerra
    [Tooltip("A quantidade de força que o golpe será multiplicado, usado apenas em golpes buffados")]
    public float duration = 2;
    public bool turnEnded;
    public List<Effect> efeitosAtivos;
    public StatusEffect statusEffect;
    //variaveis privadas
    Aliados aliado;
    TextMeshProUGUI lifeText;
    void Awake()
    {
        if (shadow != null) shadow.gameObject.SetActive(false);
    }
    void Start()
    {
        aliado = GetComponent<Aliados>();
    }
    #region EffectInvolved
    public void OnTurnStart()
    {
        for(int i = 0; i < efeitosAtivos.Count; i++)
        {
            if(efeitosAtivos[i] != null)
            efeitosAtivos[i].OnTurnStart(this);
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
        Animator animator = GetComponent<Animator>();
        vidaAtual = characterStatus.vidaAtual;
        vidaMaxima = characterStatus.vidaMaxima;
        if (characterStatus.characterSprite != null)
            GetComponent<SpriteRenderer>().sprite = characterStatus.characterSprite;

        if (animator != null) animator.runtimeAnimatorController = characterStatus.animatorController;
        if (enemy != null) enemy.ataques = characterStatus.ataques;

        if (lifeBar != null)
        {
            lifeText = lifeBar.GetComponentInChildren<TextMeshProUGUI>();
            lifeBar.gameObject.SetActive(true);
            lifeBar.maxValue = vidaMaxima;
            AtualizarVida();
        }
    }
        public void TakeDamage(int damage, bool shakeCamera, bool strongMove = false)
        {
        if (ChecarSePossuiVida())
        {
            if (aliado != null && aliado.isDefending)
            {
                damage = damage / 2;
            }
            vidaAtual -= damage;

            string texto = ("-" + damage.ToString() + (strongMove ? "!" : ""));
            TextPopup.instance.GerarTexto(texto, transform.position, Color.red);

            StartCoroutine(ShakeEffect.instance.Shake(gameObject, 0.25f, 0.05f));

            if(shakeCamera)
            StartCoroutine(ShakeEffect.instance.Shake(TurnModeManager.instance.mainCamera.gameObject, 0.25f, 0.09f));

            AtualizarVida();
        }
        if(!ChecarSePossuiVida())
        { 
            AtualizarVida();
            TurnModeManager.instance.aliadosPersonagens.Remove(this);
            TurnModeManager.instance.aliados.Remove(gameObject.GetComponent<Aliados>());
            TurnModeManager.instance.inimigosPersonagens.Remove(this);
            TurnModeManager.instance.inimigos.Remove(gameObject.GetComponent<EnemyAI>());
            Destroy(gameObject);
        }
    }
    public void EndTurn()
    {
        turnEnded = true;
        TurnModeManager.instance.CheckIfAllCharactersAttacked();
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
