using UnityEngine;
[CreateAssetMenu(fileName = "BuffEffect", menuName = "ScriptableObjects/Effects/BuffEffect", order = 1)]
public class BuffEffect : Effect
{
    BasePersonagem character;
    public override void ApplyEffect(BasePersonagem alvo)
    {
        character = alvo;
        InteractButtonsController.instance.menu.SetActive(false);
        character.strengthFactor *= 2;
        var resultado = character.ChecarSeJaPossuiEfeito(this);
        TextPopup.instance.GerarTexto("Força: " + character.strengthFactor.ToString(), character.transform.position, Color.red, 26f);
        if (!resultado.jaTem) //se o alvo não tiver o efeito tiver o efeito
        {
            character.efeitosAtivos.Add(Instantiate(this));
        }
    }
    public override void OnTurnStart()
    {
        base.OnTurnStart();
    }
    public override void RemoveEffect()
    {
        character.strengthFactor /= 2; //bug ocorrendo aqui
        TextPopup.instance.GerarTexto("Força: " + character.strengthFactor.ToString(), character.transform.position, Color.red, 26f);
        character.efeitosAtivos.Remove(this);
    }
}
