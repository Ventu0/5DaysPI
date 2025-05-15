using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class BasePersonagem : MonoBehaviour, IDamageable
{
    [SerializeField]public Slider lifeBar;
    public int força;
    public int vidaAtual;
    public int vidaMaxima;
    public int defesa;
    public float duration = 2;
    Vector2 initialPos;
    public bool turnEnded;
    Aliados aliado;
    public CharacterStatusGeneric characterStatus;
    TextMeshProUGUI lifeText;
    void Start()
    {
        aliado = gameObject.GetComponent<Aliados>();
        initialPos = transform.position;
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
    public void MovePlayerToPos(Vector2 newPos)
    {
        StartCoroutine(MovePlayer(newPos));
    }
    public IEnumerator MovePlayer(Vector2 newPos)
    {
        float iterador = 0;
        Turnos turno = TurnModeManager.instance.turno;
        if (turno == Turnos.EnemyTurn)
        {
            newPos.x += 2;
        }else if(turno == Turnos.PlayerTurn)
        {
            newPos.x -= 2;
        }
        while (iterador < duration)
        {
            float playerNewY = Mathf.Lerp(initialPos.y, newPos.y, iterador) + 0.5f * Mathf.Sin(Mathf.PI * Mathf.Clamp01(iterador));
            float playerNewX = Mathf.Lerp(initialPos.x, newPos.x, iterador);
            transform.position = new Vector2(playerNewX, playerNewY);
            iterador += Time.deltaTime * duration;
            yield return null;
        }
        print("Estou pulando agora: " + gameObject.name);
        yield return new WaitForSeconds(0.1f);
        iterador = 0;
        while (iterador < duration)
        {
            float playerNewY = Mathf.Lerp(newPos.y, initialPos.y, iterador) + 0.5f * Mathf.Sin(Mathf.PI * Mathf.Clamp01(iterador));
            float playerNewX = Mathf.Lerp(newPos.x, initialPos.x, iterador);
            transform.position = new Vector2(playerNewX, playerNewY);
            iterador += Time.deltaTime * duration;
            yield return null;
        }
        turnEnded = true;
        print("atacou");
        TurnModeManager.instance.CheckIfAllCharactersAttacked();
    }
}
