using UnityEngine;

public class Box : MonoBehaviour
{
    private Vector2Int gridPos;

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
        transform.position = (Vector2)gridPos;
    }

    public bool TryPush(Vector2Int dir)
    {
        Vector2Int nextPos = gridPos + dir;

        // 밀려날 자리에 벽이나 다른 상자 있는지 체크
        Collider2D hit = Physics2D.OverlapPoint((Vector2)nextPos);
        if (hit != null && (hit.CompareTag("Wall") || hit.GetComponent<Box>() != null))
            return false; // 못 밀어

        // 이동
        gridPos = nextPos;
        transform.position = (Vector2)gridPos;

        // 승리 판정 요청
        LevelManager.Instance.CheckWin();

        return true;
    }

    public Vector2Int GetGridPos() => gridPos;
}