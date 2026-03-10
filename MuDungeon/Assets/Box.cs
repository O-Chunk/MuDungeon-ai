using UnityEngine;

public class Box : MonoBehaviour
{
    private Vector2Int gridPos;

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
        transform.position = (Vector2)gridPos;
    }

    public bool TryPush(Vector2Int dir, int pushPower)
    {
        Vector2Int nextPos = gridPos + dir;

        // 밀려날 자리에 벽이나 다른 상자 있는지 체크
        Collider2D hit = Physics2D.OverlapPoint((Vector2)nextPos);
        if (hit != null)
        {
            if (hit.CompareTag("Wall")) return false;

            // 폭탄 위로 상자 못 밀게
            if (hit.GetComponent<Bomb>() != null) return false;

            // 연속 상자 밀기
            Box nextBox = hit.GetComponent<Box>();
            if (nextBox != null)
            {
                if (pushPower <= 1) return false; // 밀 힘이 없으면 못 밀어
                if (!nextBox.TryPush(dir, pushPower - 1)) return false;
            }
        }

        // 이전 위치 가시 체크 - 상자 치우면 다시 위험
        UpdateSpikeAt(gridPos, false);

        // 이동
        gridPos = nextPos;
        transform.position = (Vector2)gridPos;

        // 새 위치에 퓨즈 있으면 제거
        FuseManager.Instance.RemoveFuseAt(gridPos);

        // 목표 지점 위에 있으면 색 변경
        UpdateColor();

        // 새 위치 가시 체크 - 상자가 덮으면 안전
        UpdateSpikeAt(gridPos, true);

        UpdateColor();

        // 승리 판정 요청
        LevelManager.Instance.CheckWin();

        return true;
    }

    void UpdateColor()
    {
        bool onGoal = false;
        Goal[] goals = FindObjectsByType<Goal>(FindObjectsSortMode.None);
        foreach (Goal goal in goals)
        {
            if (goal.GetGridPos() == gridPos)
            {
                onGoal = true;
                break;
            }
        }

        GetComponent<SpriteRenderer>().color = onGoal
            ? new Color(0.2f, 1f, 0.4f)    // 목표 위 - 초록색
            : new Color(1f, 0.6f, 0.1f);   // 기본 - 주황색
    }

    public Vector2Int GetGridPos() => gridPos;

    void UpdateSpikeAt(Vector2Int pos, bool covered)
    {
        Spike[] spikes = FindObjectsByType<Spike>(FindObjectsSortMode.None);
        foreach (Spike spike in spikes)
        {
            if (spike.GetGridPos() == pos)
                spike.SetCovered(covered);
        }
    }
}