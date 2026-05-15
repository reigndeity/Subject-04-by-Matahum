using UnityEngine;

public interface IInteractable
{
    public static IInteractable currentInteractable;
    void Interact();
}