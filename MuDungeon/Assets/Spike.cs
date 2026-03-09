using UnityEngine;

public class Spike : MonoBehaviour
{
    private Vector2Int gridPos;
    private bool isCovered = false; // 상자로 덮였는지

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
        transform.position = (Vector2)gridPos;
    }

    public Vector2Int GetGridPos() => gridPos;

    public bool IsCovered() => isCovered;

    public void SetCovered(bool covered)
    {
        isCovered = covered;
        // 덮이면 색깔 어둡게
        GetComponent<SpriteRenderer>().color = covered
            ? new Color(0.3f, 0.3f, 0.3f)   // 덮임 - 어두운 회색
            : new Color(1f, 0.2f, 0.2f);     // 기본 - 빨간색
    }
}