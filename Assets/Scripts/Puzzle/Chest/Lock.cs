using System.Linq;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public ItemData requiredKey; // 자물쇠를 여는 데 필요한 키
    private bool isUnlocked = false;

    private UIInventory inventory;

    private void Start()
    {
        inventory = CharacterManager.Instance.Player.inventory; // 플레이어 인벤토리 찾기
    }

    // 자물쇠가 열려 있는지 확인
    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    public bool CanUnlock()
    {
        if (inventory == null)
            return false;

        foreach (ItemSlot data in inventory.slots)
        {
            if (data.item == null)
            {
                return false;
            }
            if (data.item.displayName.Equals(requiredKey.displayName))
            {
                return true;
            }
        }

        return false;
    }

    // 자물쇠를 여는 시도
    public bool TryUnlock()
    {
        // 자물쇠가 이미 열려 있으면 true 반환
        if (isUnlocked) return true;

        // 자물쇠를 열 수 있는 키가 없으면 false 반환
        if (inventory == null)
            return false;

        foreach (ItemSlot data in inventory.slots)
        {
            if (data.item.displayName.Equals(requiredKey.displayName))
            {
                // 키가 있으면 자물쇠를 열고 true 반환
                isUnlocked = true;
                Debug.Log("자물쇠가 열렸습니다!");
                Destroy(gameObject);
                return true;
            }
        }

        return false;
    }
}