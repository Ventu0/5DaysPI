using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
public class MovesVisualEffect : MonoBehaviour
{
    [SerializeField] float espaçamento = 0.75f;
    [SerializeField] GameObject attackEffect;
    [SerializeField] Animator animator;
    SpriteRenderer spriteRenderer;

    Sprite effectSprite;
    Vector2 effectPosition;
    RuntimeAnimatorController controllerAnimation;
    float effectDuration;
    public static MovesVisualEffect instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        spriteRenderer = attackEffect.GetComponent<SpriteRenderer>();
        animator = attackEffect.GetComponent<Animator>();
    }
    public void AttackEffect(Sprite spriteEffect, Vector2 positionEffect, RuntimeAnimatorController animatorController, bool playInFront, float duration = 0.7f)
    {
        controllerAnimation = animatorController;
        effectSprite = spriteEffect;
        effectDuration = duration;
        effectPosition = new Vector2(positionEffect.x, positionEffect.y - 0.5f);
        if(playInFront)
        StartCoroutine(PlayAttackEffectInFront());
        else StartCoroutine(PlayAttackEffectInPosition());
    }
    public IEnumerator PlayAttackEffectInFront()
    {
        animator.runtimeAnimatorController = controllerAnimation;
        Turnos turno = TurnModeManager.instance.turno;
        if (turno == Turnos.EnemyTurn)
        {
                effectPosition.x += espaçamento;
                spriteRenderer.flipX = true;
        }
        else if (turno == Turnos.PlayerTurn)
        {           
                effectPosition.x -= espaçamento;
                spriteRenderer.flipX = false;
        }

        attackEffect.SetActive(true);
        spriteRenderer.sprite = effectSprite;
        attackEffect.transform.position = effectPosition;

        yield return new WaitForSeconds(effectDuration);
        attackEffect.SetActive(false);
    }
    IEnumerator PlayAttackEffectInPosition()
    {
        GameObject effect = Instantiate(attackEffect, effectPosition, transform.rotation);
        effect.GetComponent<Animator>().runtimeAnimatorController = controllerAnimation;
        effect.SetActive(true);
        effect.transform.position = effectPosition;
        yield return new WaitForSeconds(effectDuration);
        Destroy(effect);
    }
}