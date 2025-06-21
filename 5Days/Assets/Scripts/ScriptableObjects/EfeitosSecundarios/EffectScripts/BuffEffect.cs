using UnityEngine;
[CreateAssetMenu(fileName = "BuffEffect", menuName = "ScriptableObjects/Effects/BuffEffect", order = 1)]
public class BuffEffect : Effect
{
    BasePersonagem character;
    public override void ApplyEffect(BasePersonagem alvo)
    {
        character = alvo;
        remainingTurns = durationInTurn;    
        InteractButtonsController.instance.menu.SetActive(false);
        alvo.strengthFactor *= 2;
        var resultado = alvo.ChecarSeJaPossuiEfeito(this);
        TextPopup.instance.GerarTexto("Força: " + alvo.strengthFactor.ToString(), alvo.transform.position, Color.red, 26f);
        if (!resultado.jaTem) //se o alvo não tiver o efeito tiver o efeito
        {
            alvo.efeitosAtivos.Add(Instantiate(this));
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
        character.strengthFactor /= 2; //bug ocorrendo aqui
        TextPopup.instance.GerarTexto("Força: " + character.strengthFactor.ToString(), character.transform.position, Color.red, 26f);
        character.isBuffed = false;
        character.efeitosAtivos.Remove(this);
    }
}
