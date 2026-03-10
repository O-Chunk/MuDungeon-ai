using UnityEngine;
using UnityEngine.InputSystem;

public class FuseManager : MonoBehaviour
{
    public static FuseManager Instance;

    [Header("프리팹")]
    public GameObject fusePrefab;      // 도화선 프리팹

    [Header("폭탄 설정")]
    public int maxBombs = 3;           // 스테이지당 폭탄 개수
    public int explosionRange = 3; // Inspector에서 조절

    private int bombsLeft;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        bombsLeft = maxBombs;
        Debug.Log($"💣 폭탄 {bombsLeft}개 준비!");
    }

    void OnEnable()
    {
        bombsLeft = maxBombs;
    }

    public void ResetBombs()
    {
        bombsLeft = maxBombs;
        Debug.Log($"💣 폭탄 {bombsLeft}개 초기화!");
    }

    void Update()
    {
        // Space = 폭탄 배치
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            PlaceBomb();
        }

        // E = 도화선 점화 (폭발!)
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Detonate();
        }
    }

    public void RemoveFuseAt(Vector2Int pos)
    {
        Fuse[] fuses = FindObjectsByType<Fuse>(FindObjectsSortMode.None);
        foreach (Fuse fuse in fuses)
        {
            if (fuse.GetGridPos() == pos)
            {
                Destroy(fuse.gameObject); // 퓨즈 완전 제거
                Debug.Log("🔌 퓨즈 끊김!");
                return;
            }
        }
    }

    public void PlaceBomb()
    {
        if (bombsLeft <= 0)
        {
            Debug.Log("💣 폭탄이 없어요!");
            return;
        }

        Vector2Int playerPos = FindFirstObjectByType<PlayerController>()
            .GetGridPos();

        // 이미 폭탄 있는지 체크
        Collider2D hit = Physics2D.OverlapPoint((Vector2)playerPos);
        if (hit != null && hit.GetComponent<Bomb>() != null)
        {
            Debug.Log("이미 폭탄이 있어요!");
            return;
        }

        // 폭탄 배치
        GameObject bombObj = new GameObject("Bomb");
        bombObj.transform.position = (Vector2)playerPos;
        bombObj.AddComponent<SpriteRenderer>().sprite =
            FindFirstObjectByType<PlayerController>()
            .GetComponent<SpriteRenderer>().sprite;
            
        Bomb bomb = bombObj.AddComponent<Bomb>();
        bomb.explosionRange = explosionRange; // 범위 전달
        bombObj.AddComponent<BoxCollider2D>();
        bomb.Place(playerPos);

        bombsLeft--;
        Debug.Log($"💣 남은 폭탄: {bombsLeft}");
    }

    public void LayFuse(Vector2Int pos)
    {
        // 벽이면 퓨즈 깔지 않기
        Collider2D hit = Physics2D.OverlapPoint((Vector2)pos);
    
        if (hit != null && hit.CompareTag("Wall")) return;

        // 이미 도화선 있으면 스킵
        Fuse[] fuses = FindObjectsByType<Fuse>(FindObjectsSortMode.None);
        foreach (Fuse f in fuses)
        {
            if (f.GetGridPos() == pos) return;
        }

        // 도화선 생성
        Instantiate(fusePrefab, (Vector2)pos, Quaternion.identity);
    }

    public void BreakFuseAt(Vector2Int pos)
    {
        Fuse[] fuses = FindObjectsByType<Fuse>(FindObjectsSortMode.None);
        foreach (Fuse fuse in fuses)
        {
            if (fuse.GetGridPos() == pos)
                fuse.Break();
        }
    }

    public void Detonate()
    {
        // 도화선 연결 체크 후 폭발
        Bomb[] bombs = FindObjectsByType<Bomb>(FindObjectsSortMode.None);

        if (bombs.Length == 0)
        {
            Debug.Log("💣 배치된 폭탄이 없어요!");
            return;
        }

        bool anyExploded = false;
        foreach (Bomb bomb in bombs)
        {
            if (IsBombConnected(bomb))
            {
                bomb.Explode();
                anyExploded = true;
            }
            else
            {
                Debug.Log("🔌 연결 안 된 폭탄은 터지지 않아요!");
            }
        }

        if (!anyExploded)
        {
            Debug.Log("💣 연결된 폭탄이 없어요!");
        }
    }

    bool IsBombConnected(Bomb bomb)
    {
        Fuse[] fuses = FindObjectsByType<Fuse>(FindObjectsSortMode.None);
        Vector2Int[] dirs = {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };

        foreach (var dir in dirs)
        {
            Vector2Int checkPos = bomb.GetGridPos() + dir;
            foreach (Fuse fuse in fuses)
            {
                if (fuse.GetGridPos() == checkPos && !fuse.IsBroken())
                    return true;
            }
        }
        return false;
    }

    public void CheckWin()
    {
        BombTarget[] targets = FindObjectsByType<BombTarget>(FindObjectsSortMode.None);
        foreach (BombTarget target in targets)
        {
            if (!target.IsDestroyed()) return;
        }

        Debug.Log("🎉 모든 목표 파괴! 스테이지 클리어!");
        Invoke("NextLevel", 1f);
    }

    void NextLevel()
    {
        LevelManager.Instance.NextLevel();
    }
}