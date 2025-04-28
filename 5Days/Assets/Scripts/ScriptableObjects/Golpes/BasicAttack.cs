using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
[CreateAssetMenu(menuName = "Ataque/AtaqueBásico")]
    public class BasicAttack : Attack
    {
        public override async void ExecutarAtaque(BasePersonagem alvo)
        {
            //Debug.Log(TurnModeManager.instance.QuemEstaAtacando() + alvo.name);
            float duração = TurnModeManager.instance.QuemEstaAtacando().duration;
            base.ExecutarAtaque(alvo);
            Debug.Log(TurnModeManager.instance.QuemEstaAtacando());
            TurnModeManager.instance.QuemEstaAtacando().MovePlayerToPos(new Vector2(alvo.transform.position.x, alvo.transform.position.y));
            await Task.Delay(Mathf.CeilToInt(duração) * 250);
            alvo.TakeDamage(dano);
        }
}


