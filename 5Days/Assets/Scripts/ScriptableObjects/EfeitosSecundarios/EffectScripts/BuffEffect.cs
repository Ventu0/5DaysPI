using UnityEngine;
[CreateAssetMenu(fileName = "BuffEffect", menuName = "EfeitoSecundario/BuffEffect", order = 1)]
public class BuffEffect : Effect
{
    [SerializeField] float strengthMultiplier = 2f;
    [Tooltip("o valor é somado")]
    [SerializeField] bool stackAble = false;
    BasePersonagem character;
    public override void ApplyEffect(BasePersonagem alvo)
    {
        character = alvo;
        InteractButtonsController.instance.menu.SetActive(false);
        var resultado = alvo.ChecarSeJaPossuiEfeito(this);
        if (!stackAble)
        {
            if (resultado.jaTem) //se o alvo não tiver o efeito tiver o efeito
            {
                TextPopup.instance.GerarTexto("Falhou!", alvo.transform.position, Color.white, 26f);
            }
            else
                Aplicar();
        }
        else
            Aplicar();
    }
    void Aplicar()
    {
        character.strengthFactor += strengthMultiplier;
        remainingTurns = durationInTurn;
        TextPopup.instance.GerarTexto("Força: " + character.strengthFactor.ToString(), character.transform.position, Color.red, 26f);
        character.efeitosAtivos.Add(Instantiate(this));
        character.isBuffed = true;
    }
    public override void OnTurnStart(BasePersonagem alvo) //por algum motivo desconhecido, o character estava sendo nulo, então teve que ser atribuido forçadamente
    {
        Debug.Log("OnTurnStart ativando: " + remainingTurns);
        alvo.isBuffed = true;
        if (remainingTurns <= durationInTurn)
        {
            remainingTurns--;
        }
        if (remainingTurns <= 0)
        {
            character = alvo;
            RemoveEffect();
        }
    }
    public override void RemoveEffect()
    {
        character.strengthFactor -= strengthMultiplier; 
        character.strengthFactor = Mathf.Clamp(character.strengthFactor, 1, 999);
        TextPopup.instance.GerarTexto("Força: " + character.strengthFactor.ToString(), character.transform.position, Color.red, 26f);
        character.isBuffed = false;
        character.efeitosAtivos.Remove(this);
    }
}
