using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("폭발 설정")]
    public int explosionRange = 0; // Inspector에서 조절 가능

    private Vector2Int gridPos;
    private bool isPlaced = false; // 배치됐는지

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
        transform.position = (Vector2)gridPos;
        GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f); // 검정
    }

    public Vector2Int GetGridPos() => gridPos;
    public bool IsPlaced() => isPlaced;

    public void Place(Vector2Int pos)
    {
        isPlaced = true;
        gridPos = pos;
        transform.position = (Vector2)gridPos;
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 0.1f); // 배치됨 - 주황
        Debug.Log($"💣 폭탄 배치!");
    }

    public void Explode()
    {
        Debug.Log($"💥 폭발! 범위: {explosionRange}");
        // 주변 4방향 체크
        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };

        foreach (var dir in directions)
        {
            for (int i = 1; i <= explosionRange; i++) // 3 → explosionRange 로 변경!
            {
                Vector2Int checkPos = gridPos + dir * i;

                // 폭발 이펙트 (빨간 사각형 잠깐 표시)
                GameObject effect = new GameObject("ExplosionEffect");
                effect.transform.position = (Vector2)checkPos;
                SpriteRenderer sr = effect.AddComponent<SpriteRenderer>();
                sr.sprite = GetComponent<SpriteRenderer>().sprite;
                sr.color = new Color(1f, 0.3f, 0f); // 주황빨간색
                sr.sortingOrder = 10; // 맨 위에 표시
                Destroy(effect, 0.4f); // 0.4초 후 사라짐

                Collider2D hit = Physics2D.OverlapPoint((Vector2)checkPos);
                if (hit != null)
                {
                    // 목표 지점 파괴
                    BombTarget target = hit.GetComponent<BombTarget>();
                    if (target != null) target.Destroy();

                    // 나무 상자 파괴
                    WoodenBox woodenBox = hit.GetComponent<WoodenBox>();
                    if (woodenBox != null) woodenBox.Break();

                    // 벽이나 철 상자면 폭발 막힘
                    if (hit.CompareTag("Wall") || hit.GetComponent<Box>() != null)
                        break;
                }
            }
        }

        Destroy(gameObject);
    }
}