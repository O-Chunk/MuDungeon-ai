using UnityEngine;

public class WoodenBox : MonoBehaviour
{
    private Vector2Int gridPos;

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
        transform.position = (Vector2)gridPos;
        GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.3f, 0.1f); // 갈색
    }

    public bool TryPush(Vector2Int dir, int pushPower)
    {
        Vector2Int nextPos = gridPos + dir;

        Collider2D hit = Physics2D.OverlapPoint((Vector2)nextPos);
        if (hit != null)
        {
            if (hit.CompareTag("Wall")) return false;
            if (hit.GetComponent<Bomb>() != null) return false;

            // 연속 나무 상자 밀기
            WoodenBox nextWoodenBox = hit.GetComponent<WoodenBox>();
            if (nextWoodenBox != null)
            {
                if (pushPower <= 1) return false;
                if (!nextWoodenBox.TryPush(dir, pushPower - 1)) return false;
            }

            // 철 상자 밀기
            Box nextBox = hit.GetComponent<Box>();
            if (nextBox != null)
            {
                if (pushPower <= 1) return false;
                if (!nextBox.TryPush(dir, pushPower - 1)) return false;
            }
        }

        gridPos = nextPos;
        transform.position = (Vector2)gridPos;

        // 퓨즈 제거
        FuseManager.Instance.RemoveFuseAt(gridPos);

        return true;
    }

    public void Break()
    {
        Debug.Log("🪵 나무 상자 파괴!");
        Destroy(gameObject);
    }

    public Vector2Int GetGridPos() => gridPos;
}