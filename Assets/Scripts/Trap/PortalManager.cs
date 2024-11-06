using System.Collections;
using System;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
	[SerializeField] Transform exitPltal; // 이동할 포탈 위치
	[SerializeField] LayerMask playerLayer; // 이동을 허용할 레이어
	[SerializeField] float teleportCooldown = 1f; // 쿨다운 시간
	private bool canTeleport = true; // 중복 전송 방지 플래그

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			StartCoroutine(Teleport(other));
		}
	}
	private IEnumerator Teleport(Collider player)
	{
		canTeleport = false; // 현재 포탈 중복 텔레포트 방지
							 // 플레이어를 목적지 포탈로 이동
		player.transform.position = exitPltal.position;
		// 플레이어에 쿨다운 상태 부여 (포탈 충돌을 일시적으로 무시)
		PortalManager portalScript = exitPltal.GetComponent<PortalManager>();
		portalScript.canTeleport = false;
		// 지정된 쿨다운 시간 대기
		yield return new WaitForSeconds(teleportCooldown);
		// 두 포탈의 텔레포트 기능 다시 활성화
		canTeleport = true;
		portalScript.canTeleport = true;
	}
}
