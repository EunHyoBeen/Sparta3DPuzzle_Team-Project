using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Selected Item")]
    
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    
    public GameObject dropButton;
    public GameObject equipButton;
    public GameObject unEquipButton;

    private PlayerController controller;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipIndex;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        ClearSelectedItemWindow();
    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;

        dropButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool isOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }
    
    public void AddItem(ItemData data)
    {
        ItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        ThrowItem(data);
    }

    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if( slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0;i<slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }

    public void OnEquipButton()
    {
        Debug.Log(curEquipIndex);
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if(selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

}
