using UnityEngine;

public class BombTarget : MonoBehaviour
{
    private Vector2Int gridPos;
    private bool isDestroyed = false;

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
        transform.position = (Vector2)gridPos;
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0.5f); // 분홍
    }

    public Vector2Int GetGridPos() => gridPos;
    public bool IsDestroyed() => isDestroyed;

    public void Destroy()
    {
        isDestroyed = true;
        GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
        Debug.Log("🎯 목표 파괴!");
        FuseManager.Instance.CheckWin();
    }
}