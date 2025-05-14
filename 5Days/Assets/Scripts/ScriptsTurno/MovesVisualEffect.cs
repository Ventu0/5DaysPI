using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
public class MovesVisualEffect : MonoBehaviour
{
    public static MovesVisualEffect instance;
    [SerializeField] GameObject attackEffect;
    [SerializeField] Animator animatorController;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        spriteRenderer = attackEffect.GetComponent<SpriteRenderer>();
        animatorController = attackEffect.GetComponent<Animator>();
    }
    public void AttackEffect(Sprite effectSprite, Vector2 effectPosition)
    {
        StartCoroutine(PlayAttackEffect(effectSprite, effectPosition));
    }
    public IEnumerator PlayAttackEffect(Sprite effectSprite, Vector2 effectPosition)
    {
        Turnos turno = TurnModeManager.instance.turno;
        if (turno == Turnos.EnemyTurn)
        {
            effectPosition.x += 1;
            spriteRenderer.flipX = true;
        }
        else if (turno == Turnos.PlayerTurn)
        {
            effectPosition.x -= 1;
            spriteRenderer.flipX = false;
        }

        attackEffect.SetActive(true);
        spriteRenderer.sprite = effectSprite;
        attackEffect.transform.position = effectPosition;

        yield return new WaitForSeconds(0.2f);
        attackEffect.SetActive(false);
    }
    void Update()
    {
        
    }
}
