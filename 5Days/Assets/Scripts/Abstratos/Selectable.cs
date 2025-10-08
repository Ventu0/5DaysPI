using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    public abstract void AbrirMenu(int whichButton);
    public abstract void Close();
}
