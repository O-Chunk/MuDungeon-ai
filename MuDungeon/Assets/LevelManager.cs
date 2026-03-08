using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private Box[] boxes;
    private Goal[] goals;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        boxes = FindObjectsByType<Box>(FindObjectsSortMode.None);
        goals = FindObjectsByType<Goal>(FindObjectsSortMode.None);
    }

    public void CheckWin()
    {
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
            if (!covered) return; // 아직 안 끝남
        }

        // 모든 목표 달성!
        Debug.Log("🎉 스테이지 클리어!");
        Invoke("LoadNextLevel", 1f);
    }

    void LoadNextLevel()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            Debug.Log("🏆 모든 스테이지 클리어!");
    }
}