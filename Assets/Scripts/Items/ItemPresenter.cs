using UnityEngine;
/// <summary>
/// Base Class for different ways of presenting items
/// </summary>
public abstract class ItemPresenter : MonoBehaviour
{
    public abstract void PresentItem(Item item);
}
