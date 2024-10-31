 
using System;
using UnityEngine;

// 종료 지점을 생성하기 위한 추상 클래스
// EndPoint를 상속받아 HandleLevelComplete 메서드를 구현하여 
// 특정 종료 로직을 정의할 수 있습니다.
[RequireComponent(typeof(BoxCollider))]
public abstract class EndPoint : MonoBehaviour
{
    private Collider collider;
    
    // 기본적으로 Trigger 이벤트로 설정합니다.
    protected void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    // 플레이어와 충돌했을 때 종료 로직을 처리합니다.
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandleLevelComplete();
        }
    }
    
    // 종료 로직을 구현하기 위한 추상 메서드
    // 상속받은 클래스에서 종료 동작을 정의해야 합니다.
    protected abstract void HandleLevelComplete();
}