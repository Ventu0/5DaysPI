using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class BasePersonagem : MonoBehaviour, IDamageable
{
    [Header("Status")]
    public int força;
    public int vidaAtual;
    public int vidaMaxima;
    public int defesa;
    public CharacterStatusGeneric characterStatus;

    [Header("Opcional")]
    [SerializeField] public Slider lifeBar;
    public Transform shadow;

    [Header("Read-Only")]
    public float duration = 2;
    public bool turnEnded;
    public Effect efeitoAtivo;
    //variaveis privadas
    Aliados aliado;
    TextMeshProUGUI lifeText;
    void Awake()
    {
        if (shadow != null) shadow.gameObject.SetActive(false);
    }
    void Start()
    {
        aliado = gameObject.GetComponent<Aliados>();
    }
    public void OnTurnStart()
    {
        if(efeitoAtivo != null)
        efeitoAtivo.OnTurnStart();
    }
    public void SetupStatus()
    {
        EnemyAI enemy = GetComponent<EnemyAI>();
        força = characterStatus.força;
        vidaAtual = characterStatus.vidaAtual;
        vidaMaxima = characterStatus.vidaMaxima;
        defesa = characterStatus.defesa;
        if(enemy != null)
        {
            enemy.ataques = characterStatus.ataques;
        }
        if (lifeBar != null)
        {
            lifeText = lifeBar.GetComponentInChildren<TextMeshProUGUI>();
            lifeBar.gameObject.SetActive(true);
            lifeBar.maxValue = vidaMaxima;
            UpdateLife();
        }
    }
        public void TakeDamage(int damage, bool shakeCamera)
        {
        if (CheckIfHasLife())
        {
            if (aliado != null)
            {
                if (aliado.isDefending)
                {
                    vidaAtual -= damage / 2;
                }
                else
                {
                    vidaAtual -= damage;
                }
            }
            else
            {
                vidaAtual -= damage;
            }
            StartCoroutine(ShakeEffect.instance.Shake(gameObject, 0.25f, 0.05f));
            if(shakeCamera)
            StartCoroutine(ShakeEffect.instance.Shake(TurnModeManager.instance.mainCamera.gameObject, 0.25f, 0.05f));

            TextPopup.instance.GerarTexto("-" + damage.ToString(), 1, transform.position);
            UpdateLife();
        }
        if(!CheckIfHasLife())
        { 
            UpdateLife();
            TurnModeManager.instance.aliadosPersonagens.Remove(this);
            TurnModeManager.instance.aliados.Remove(gameObject.GetComponent<Aliados>());
            TurnModeManager.instance.inimigosPersonagens.Remove(this);
            TurnModeManager.instance.inimigos.Remove(gameObject.GetComponent<EnemyAI>());
            Destroy(gameObject);
        }
    }
    void UpdateLife()
    {
        if (CheckIfHasLife())
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
    bool CheckIfHasLife()
    {
        if (vidaAtual > 0) return true;
        else if (vidaAtual <= 0) return false;
        return false;
    } 
}
