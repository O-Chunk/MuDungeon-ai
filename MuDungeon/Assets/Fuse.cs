using UnityEngine;

public class Fuse : MonoBehaviour
{
    private Vector2Int gridPos;
    private bool isBroken = false;

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.2f); // 노란색

        // 다른 오브젝트보다 뒤에 렌더링
        GetComponent<SpriteRenderer>().sortingOrder = -1;

        // 크기를 작게 해서 바닥에 깔린 느낌
        transform.localScale = new Vector3(0.3f, 0.3f, 1f);
    }

    public Vector2Int GetGridPos() => gridPos;
    public bool IsBroken() => isBroken;

    public void Break()
    {
        isBroken = true;
        GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f); // 끊김 - 회색
    }
}