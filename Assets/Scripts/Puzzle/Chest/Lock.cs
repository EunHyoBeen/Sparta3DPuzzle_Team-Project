using UnityEngine;

public class Lock : MonoBehaviour
{
    public int lockID; // 자물쇠의 고유 ID
    private Chest chest; // Chest 참조

    private void Awake()
    {
        chest = GetComponentInParent<Chest>(); // 부모 오브젝트에서 Chest 찾기
    }

    public void TryUnlock(Key key)
    {
        if (key != null && key.keyID == lockID)
        {
            Debug.Log("Correct key! Unlocking the chest.");
            chest.Open(); // 상자 열기
            Destroy(this);
        }
        else
        {
            Debug.Log("Wrong key.");
        }
    }
}
