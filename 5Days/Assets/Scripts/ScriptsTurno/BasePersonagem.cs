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

    //variaveis privadas
    Aliados aliado;
    TextMeshProUGUI lifeText;
    void Start()
    {
        aliado = gameObject.GetComponent<Aliados>();
    }
    public void SetupStatus()
    {
        EnemyAI enemy = GetComponent<EnemyAI>();
        print("setando status: " + gameObject.name);
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
    void UpdateLife()
    {
        lifeBar.value = vidaAtual;
        lifeText.text = vidaAtual.ToString() + " / " + vidaMaxima.ToString();
    }
        public void TakeDamage(int damage)
        {
        //esse codigo ta uma merda quadratica, muda ele depois
        if (CheckIfHasLife())
        {
            if (aliado != null)
            {
                if (aliado.isDefending)
                {
                    vidaAtual -= damage / 2;
                }
            }
            else
            {
                vidaAtual -= damage;
            }
            StartCoroutine(ShakeEffect.instance.Shake(gameObject, 0.25f, 0.05f));
            StartCoroutine(ShakeEffect.instance.Shake(TurnModeManager.instance.mainCamera.gameObject, 0.25f, 0.05f));
            UpdateLife();
        }
        else
        {
            TurnModeManager.instance.aliadosPersonagens.Remove(this);
            TurnModeManager.instance.aliados.Remove(gameObject.GetComponent<Aliados>());
            TurnModeManager.instance.inimigosPersonagens.Remove(this);
            TurnModeManager.instance.inimigos.Remove(gameObject.GetComponent<EnemyAI>());
            Destroy(gameObject);
        }
    }
    bool CheckIfHasLife()
    {
        if (vidaAtual > 0) return true;
        else if (vidaAtual <= 0) return false;
        return false;
    } 
}
