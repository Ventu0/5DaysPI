using UnityEngine;
[CreateAssetMenu(fileName = "BuffEffect", menuName = "EfeitoSecundario/BuffEffect", order = 1)]
public class BuffEffect : Effect
{
    BasePersonagem character;
    public override void ApplyEffect(BasePersonagem alvo)
    {
        character = alvo;
        InteractButtonsController.instance.menu.SetActive(false);
        var resultado = alvo.ChecarSeJaPossuiEfeito(this);
        if (!resultado.jaTem) //se o alvo não tiver o efeito tiver o efeito
        {
            alvo.strengthFactor *= 2;
            remainingTurns = durationInTurn;
            TextPopup.instance.GerarTexto("Força: " + alvo.strengthFactor.ToString(), alvo.transform.position, Color.red, 26f);
            alvo.efeitosAtivos.Add(Instantiate(this));
        }
        else
        {
            TextPopup.instance.GerarTexto("Falhou!", alvo.transform.position, Color.white, 26f);
        }
        alvo.isBuffed = true;
        character = alvo;
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
        character.strengthFactor /= 2; 
        TextPopup.instance.GerarTexto("Força: " + character.strengthFactor.ToString(), character.transform.position, Color.red, 26f);
        character.isBuffed = false;
        character.efeitosAtivos.Remove(this);
    }
}
