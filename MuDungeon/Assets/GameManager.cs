using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ReloadLevel()
    {
        // 현재 씬 오브젝트 전부 삭제 후 현재 레벨 다시 로드
        foreach (var obj in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (obj != gameObject && obj.GetComponent<Camera>() == null)
                Destroy(obj);
        }
        LevelManager.Instance.LoadLevel(LevelManager.currentLevel);
    }

    private void Update()
    {
        // 개발자 모드 단축키
        // N키 → 다음 스테이지
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            LevelManager.currentLevel++;
            ReloadLevel();
            Debug.Log($"⏭ 스테이지 {LevelManager.currentLevel + 1} 로 이동");
        }

        // P키 → 이전 스테이지
         if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (LevelManager.currentLevel > 0)
            {
                LevelManager.currentLevel--;
                ReloadLevel();
                Debug.Log($"⏮ 스테이지 {LevelManager.currentLevel + 1} 로 이동");
            }
        }
    }
}