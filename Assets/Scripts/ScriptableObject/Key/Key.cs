using UnityEngine;

[CreateAssetMenu(fileName = "NewKey", menuName = "ScriptableObjects/Key")]
public class Key : ScriptableObject
{
    public int keyID;          // 열쇠의 고유 ID
}
