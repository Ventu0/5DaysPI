using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public int dano;
    public string nomeAtaque;
    public Sprite iconeAtaque;
    public RuntimeAnimatorController animatorController;

    public virtual void ExecutarAtaque(BasePersonagem alvo)
    {
        InteractButtonsController.instance.menu.SetActive(false);
    }
}
