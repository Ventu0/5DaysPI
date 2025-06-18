using UnityEngine;

[CreateAssetMenu(fileName = "CuraEffect", menuName = "ScriptableObjects/Effects/CuraEffect", order = 1)]
public class CuraEffect : Effect
{
    public override void ApplyEffect(BasePersonagem alvo, int buffDebuffNumberQuantity)
    {
        int originalLife = alvo.vidaAtual;
        alvo.vidaAtual += buffDebuffNumberQuantity;
        alvo.vidaAtual = Mathf.Clamp(alvo.vidaAtual, 0, alvo.vidaMaxima);
        TextPopup.instance.GerarTexto(Mathf.Abs(originalLife - alvo.vidaAtual).ToString(), alvo.transform.position, Color.green);
        alvo.EndTurn();
    }

}
