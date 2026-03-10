using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    public float smoothSpeed = 0.1f;

    void LateUpdate()
    {
        if (target == null)
        {
            // 플레이어 찾기
            PlayerController player = FindFirstObjectByType<PlayerController>();
            if (player != null) target = player.transform;
            return;
        }

        // 플레이어 위치로 부드럽게 이동
        Vector3 targetPos = new Vector3(
            Mathf.Round(target.position.x),
            Mathf.Round(target.position.y),
            -10f
        );

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
    }
}