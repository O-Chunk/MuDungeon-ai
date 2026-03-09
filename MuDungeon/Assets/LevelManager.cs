using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("프리팹 연결")]
    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject spikePrefab;
    public GameObject crumblingTilePrefab;

    private Box[] boxes;
    private Goal[] goals;

    public static int currentLevel = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int index)
    {
        if (index >= LevelData.Levels.Length)
        {
            Debug.Log("🏆 모든 스테이지 클리어!");
            return;
        }

        LevelData data = LevelData.Levels[index];
        BuildMap(data);

        // 오브젝트 찾기 (생성 후)
        boxes = FindObjectsByType<Box>(FindObjectsSortMode.None);
        goals = FindObjectsByType<Goal>(FindObjectsSortMode.None);

        Debug.Log($"📍 {data.levelName} 시작!");
    }

    void BuildMap(LevelData data)
    {
        // 맵 세로 길이
        int rows = data.map.Length;

        for (int y = 0; y < rows; y++)
        {
            string row = data.map[y];
            for (int x = 0; x < row.Length; x++)
            {
                char tile = row[x];
                // Unity는 Y축이 위가 +라서 rows-y로 뒤집기
                Vector3 pos = new Vector3(x, rows - y, 0);

                switch (tile)
                {
                    case '1': Instantiate(wallPrefab, pos, Quaternion.identity); break;
                    case '2': Instantiate(playerPrefab, pos, Quaternion.identity); break;
                    case '3': Instantiate(boxPrefab, pos, Quaternion.identity); break;
                    case '4': Instantiate(goalPrefab, pos, Quaternion.identity); break;
                    case '5':
                        Instantiate(boxPrefab, pos, Quaternion.identity);
                        Instantiate(goalPrefab, pos, Quaternion.identity);
                        break;
                    case '6': Instantiate(spikePrefab, pos, Quaternion.identity); break;
                    case '7': Instantiate(crumblingTilePrefab, pos, Quaternion.identity); break;
                }
            }
        }
    }

    public void CheckWin()
    {
        boxes = FindObjectsByType<Box>(FindObjectsSortMode.None);
        goals = FindObjectsByType<Goal>(FindObjectsSortMode.None);

        foreach (Goal goal in goals)
        {
            bool covered = false;
            foreach (Box box in boxes)
            {
                if (box.GetGridPos() == goal.GetGridPos())
                {
                    covered = true;
                    break;
                }
            }
            if (!covered) return;
        }

        Debug.Log("🎉 스테이지 클리어!");
        Invoke("NextLevel", 1f);
    }

    void NextLevel()
    {
        // 현재 씬 오브젝트 전부 삭제 후 다음 레벨 로드
        // GameManager랑 Camera 빼고 삭제
        foreach (var obj in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (obj != gameObject && obj.GetComponent<Camera>() == null)
                Destroy(obj);
        }
        
        currentLevel++;
        LoadLevel(currentLevel);
    }
}