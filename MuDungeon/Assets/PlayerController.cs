using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveDelay = 0.15f; // 이동 딜레이 (너무 빠르지 않게)
    private float lastMoveTime;
    private Vector2Int gridPos; // 현재 칸 위치

    void Start()
    {
        // 시작 위치를 그리드에 맞춤
        gridPos = Vector2Int.RoundToInt(transform.position);
        transform.position = (Vector2)gridPos;
    }

    void Update()
    {
        // R키 → 스테이지 재시작
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            GameManager.Instance.ReloadLevel();
            return;
        }

        if (Time.time - lastMoveTime < moveDelay) return;

        Vector2Int dir = Vector2Int.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            dir = Vector2Int.up;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            dir = Vector2Int.down;
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            dir = Vector2Int.left;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            dir = Vector2Int.right;

        if (dir != Vector2Int.zero)
        {
            TryMove(dir);
        }
    }

    void TryMove(Vector2Int dir)
    {
        Vector2Int nextPos = gridPos + dir;

        // 벽 체크
        if (IsWall(nextPos)) return;

        // 상자 체크
        Box box = GetBoxAt(nextPos);
        if (box != null)
        {
            // 상자 밀기 시도
            if (!box.TryPush(dir)) return;
        }

        // 가시 체크 - 상자로 안 덮인 가시면 죽기
        Spike spike = GetSpikeAt(nextPos);
        if (spike != null && !spike.IsCovered())
        {
            // 플레이어 사망 → 재시작
            Debug.Log("💀 가시에 찔렸다! 재시작...");
            GameManager.Instance.ReloadLevel();
            return;
        }

        // 현재 위치 무너지는 타일 체크
        CrumblingTile tile = GetCrumblingTileAt(gridPos);
        if (tile != null) tile.Crumble();

        gridPos = nextPos;
        transform.position = (Vector2)gridPos;
        lastMoveTime = Time.time;

        // 무너진 타일 위에 있으면 추락사
        CrumblingTile nextTile = GetCrumblingTileAt(nextPos);
        if (nextTile != null && nextTile.IsBroken())
        {
            Debug.Log("💀 바닥이 무너졌다! 재시작...");
            GameManager.Instance.ReloadLevel();
            return;
        }

        // 이동
        gridPos = nextPos;
        transform.position = (Vector2)gridPos;
        lastMoveTime = Time.time;
    }

    bool IsWall(Vector2Int pos)
    {
        // Wall 태그가 붙은 오브젝트 감지
        Collider2D hit = Physics2D.OverlapPoint((Vector2)pos);
        return hit != null && hit.CompareTag("Wall");
    }

    Box GetBoxAt(Vector2Int pos)
    {
        Collider2D hit = Physics2D.OverlapPoint((Vector2)pos);
        if (hit != null) return hit.GetComponent<Box>();
        return null;
    }

    Spike GetSpikeAt(Vector2Int pos)
    {
        Spike[] spikes = FindObjectsByType<Spike>(FindObjectsSortMode.None);
        foreach (Spike spike in spikes)
        {
            if (spike.GetGridPos() == pos) return spike;
        }
        return null;
    }

    CrumblingTile GetCrumblingTileAt(Vector2Int pos)
    {
        CrumblingTile[] tiles = FindObjectsByType<CrumblingTile>(FindObjectsSortMode.None);
        foreach (CrumblingTile tile in tiles)
        {
            if (tile.GetGridPos() == pos) return tile;
        }
        return null;
    }
}