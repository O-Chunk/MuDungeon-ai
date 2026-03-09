using UnityEngine;

public class CrumblingTile : MonoBehaviour
{
    private Vector2Int gridPos;
    private bool isBroken = false;

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
        transform.position = (Vector2)gridPos;
        GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.4f, 0.2f); // 갈색
    }

    public Vector2Int GetGridPos() => gridPos;
    public bool IsBroken() => isBroken;

    public void Crumble()
    {
        isBroken = true;
        // 무너진 타일 - 어둡게
        GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.1f, 0.05f);
        // 콜라이더 제거 (이제 벽 역할 안 함)
        Destroy(GetComponent<Collider2D>());
    }
}