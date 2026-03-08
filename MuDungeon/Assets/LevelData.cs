using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string levelName;
    public string[] map; // 각 줄을 문자열로 표현

    // 레벨 1~3 데이터
    public static LevelData[] Levels = new LevelData[]
    {
        // 레벨 1 - 아주 간단, 상자 1개
        new LevelData
        {
            levelName = "던전 1층",
            map = new string[]
            {
                "111111111",
                "100000001",
                "100000001",
                "100200001",
                "100030001",
                "100004001",
                "100000001",
                "111111111",
            }
        },

        // 레벨 2 - 상자 2개, 좁은 통로
        new LevelData
        {
            levelName = "던전 2층",
            map = new string[]
            {
                "111111111",
                "100000001",
                "101000101",
                "100200001",
                "100330001",
                "100044001",
                "100000001",
                "111111111",
            }
        },

        // 레벨 3 - 상자 2개, 순서 중요
        new LevelData
        {
            levelName = "던전 3층",
            map = new string[]
            {
                "1111111111",
                "1000000001",
                "1011101001",
                "1000000001",
                "1002330001",
                "1000044001",
                "1000000001",
                "1111111111",
            }
        },
    };
}