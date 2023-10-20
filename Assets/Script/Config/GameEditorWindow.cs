using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class GameEditorWindow : EditorWindow
{
    private string[] tabNames = { "Game Config", "Tile Config" };
    private int selectedTab = 0;
    private int selectedLevel = 0;
    private ConfigSO gameConfig;
    private LevelConfigSO levelConfig;
    private List<levelInfo> levels;
    private int widthField = 300;

    private List<string> types;
    [MenuItem("Tools/Game Configs")]
    public static void ShowWindow()
    {
        GetWindow<GameEditorWindow>("Game Configs");
    }
    private void OnEnable()
    {
        ResetGameConfig();
        ResetLevelConfig();
    }
    private void OnGUI()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, tabNames);
        switch (selectedTab)
        {
            case 0:
                GameConfigTab();
                break;
            case 1:
                LevelConfigTab();
                break;
        }
    }


    private void GameConfigTab()
    {
        gameConfig = (ConfigSO)EditorGUILayout.ObjectField("Game Config", gameConfig, typeof(ConfigSO), false);
        LoadGameConfigContent();
    }
    private int timeMatch;
    private float speedFlyCard;
    private int countMatchCard;
    private int scoreBonus;
    private float countDownCombo;
    private float scaleDownRatio;
    private float forceToLipCard;
    private float limitToLipCard;
    private Vector2 scroll;

    private void LoadGameConfigContent()
    {
        timeMatch = EditorGUILayout.IntField("Time Match", timeMatch , GUILayout.Width(widthField));
        speedFlyCard = EditorGUILayout.FloatField("Speed Fly Card", speedFlyCard, GUILayout.Width(widthField));
        countMatchCard = EditorGUILayout.IntField("Count Match Card", countMatchCard, GUILayout.Width(widthField));
        scoreBonus = EditorGUILayout.IntField("Score Bonus", scoreBonus, GUILayout.Width(widthField));
        countDownCombo = EditorGUILayout.FloatField("Count Down Combo", countDownCombo, GUILayout.Width(widthField));
        scaleDownRatio = EditorGUILayout.FloatField("Scale Down Ratio", scaleDownRatio, GUILayout.Width(widthField));
        forceToLipCard = EditorGUILayout.FloatField("Force To Lip Card", forceToLipCard, GUILayout.Width(widthField));
        limitToLipCard = EditorGUILayout.FloatField("limit To Lip Card", limitToLipCard, GUILayout.Width(widthField));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Width(widthField / 2)))
        {
            SaveGameConfig();
        }
        if (GUILayout.Button("Reload", GUILayout.Width(widthField / 2)))
        {
            ResetGameConfig();
        }
        EditorGUILayout.EndHorizontal();
    }
    private void ResetGameConfig()
    {
        gameConfig = AssetDatabase.LoadAssetAtPath<ConfigSO>("Assets/Config/ConfigGame.asset");
        timeMatch      = gameConfig.TimeMatch;
        speedFlyCard   = gameConfig.SpeedFlyCard;
        countMatchCard = gameConfig.CountMatchCard;
        scoreBonus     = gameConfig.ScoreBonus;
        countDownCombo = gameConfig.CountDownCombo;
        scaleDownRatio = gameConfig.ScaleDownRatio;
        forceToLipCard = gameConfig.ForceToLipCard;
        limitToLipCard = gameConfig.LimitToLipCard;
    }
    private void SaveGameConfig()
    {
        gameConfig.TimeMatch = timeMatch; 
        gameConfig.SpeedFlyCard = speedFlyCard; 
        gameConfig.CountMatchCard = countMatchCard; 
        gameConfig.ScoreBonus = scoreBonus; 
        gameConfig.CountDownCombo = countDownCombo; 
        gameConfig.ScaleDownRatio = scaleDownRatio; 
        gameConfig.ForceToLipCard = forceToLipCard;
        gameConfig.LimitToLipCard = limitToLipCard;
        EditorUtility.SetDirty(gameConfig);
        AssetDatabase.SaveAssets();
        ResetGameConfig();
    }
    private void ResetLevelConfig()
    {
        levelConfig = AssetDatabase.LoadAssetAtPath<LevelConfigSO>("Assets/Config/LevelConfig.asset");
        levels = levelConfig.LevelInfos.ToList();
    }
    private void LevelConfigTab()
    {
        levelConfig = (LevelConfigSO)EditorGUILayout.ObjectField("Level Config", levelConfig, typeof(LevelConfigSO), false);
        LoadLevelConfigContent();
    }
    private void SaveLevelConfig()
    {
        levelConfig.LevelInfos = levels;
        EditorUtility.SetDirty(levelConfig);
        AssetDatabase.SaveAssets();
        ResetLevelConfig();
    }

    private void LoadLevelConfigContent()
    {
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < levels.Count; i++)
        {
            if (GUILayout.Button("Map "+i, GUILayout.Width(100)))
            {
                selectedLevel = i;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New Map", GUILayout.Width(200)))
        {
            levels.Add(new levelInfo());
        }
        if (GUILayout.Button("Delete Map", GUILayout.Width(200)))
        {
            levels.RemoveAt(selectedLevel);
            selectedLevel = 0;
        }
        EditorGUILayout.EndHorizontal();

        if (levels == null || levels.Count == 0)
        {
            return;
        }
        EditorGUILayout.LabelField("Level Infomation "+ selectedLevel, GUILayout.Width(widthField));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name", GUILayout.Width(100));
        levels[selectedLevel].Name = EditorGUILayout.TextField(levels[selectedLevel].Name, GUILayout.Width(widthField));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Display Name", GUILayout.Width(100));
        levels[selectedLevel].DisplayName = EditorGUILayout.TextField(levels[selectedLevel].DisplayName, GUILayout.Width(widthField));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level", GUILayout.Width(100));
        levels[selectedLevel].Level = EditorGUILayout.IntField(levels[selectedLevel].Level, GUILayout.Width(widthField));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Play Time(s)", GUILayout.Width(100));
        levels[selectedLevel].PlayTime = EditorGUILayout.IntField((int)levels[selectedLevel].PlayTime, GUILayout.Width(widthField));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Id", GUILayout.Width(widthField/6));
        EditorGUILayout.LabelField("Type", GUILayout.Width(widthField/3));
        EditorGUILayout.LabelField("Image", GUILayout.Width(widthField/3));
        EditorGUILayout.LabelField("Quantity", GUILayout.Width(widthField/3));
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Add new card infomation", GUILayout.Width(widthField)))
        {
            levels[selectedLevel].Cards.Add(new());
        }
        scroll = EditorGUILayout.BeginScrollView(scroll);
        for (int i = 0; i < levels[selectedLevel].Cards.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(widthField/6));
            levels[selectedLevel].Cards[i].Type = (CardType)EditorGUILayout.EnumPopup(levels[selectedLevel].Cards[i].Type, GUILayout.Width(widthField/3));
            levels[selectedLevel].Cards[i].Sprite = (Sprite)EditorGUILayout.ObjectField(levels[selectedLevel].Cards[i].Sprite, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80));
            levels[selectedLevel].Cards[i].Quantity = EditorGUILayout.IntField(levels[selectedLevel].Cards[i].Quantity, GUILayout.Width(widthField/3));
            if (GUILayout.Button("x", GUILayout.Width(30)))
            {
                levels[selectedLevel].Cards.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();


        GUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("SAVE CONFIG", GUILayout.Width(widthField), GUILayout.Height(30)))
        {
            SaveLevelConfig();
        }
        if (GUILayout.Button("RELOAD CONFIG", GUILayout.Width(widthField), GUILayout.Height(30)))
        {
            ResetLevelConfig();
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif