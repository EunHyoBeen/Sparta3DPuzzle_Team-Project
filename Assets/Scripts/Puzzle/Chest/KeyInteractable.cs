using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractable : MonoBehaviour, IInteractable
{
    public ItemData data;
    public string GetInteractPrompt()
    {
        return gameObject.name;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.inventory.AddItem(data);
        Destroy(gameObject);
    }
}
