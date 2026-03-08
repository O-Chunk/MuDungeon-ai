using UnityEngine;

public class Goal : MonoBehaviour
{
    private Vector2Int gridPos;

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
    }

    public Vector2Int GetGridPos() => gridPos;
}