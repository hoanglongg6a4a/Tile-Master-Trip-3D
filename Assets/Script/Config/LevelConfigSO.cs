using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardInfo
{
    [SerializeField] private CardType type;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int quantity;
    public CardType Type { get => type; set => type = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public int Quantity { get => quantity; set => quantity = value; }
}
[System.Serializable]
public class levelInfo
{
    [SerializeField] private string name;
    [SerializeField] private string displayName;
    [SerializeField] private int level;
    [SerializeField] private float playTime;
    [SerializeField] private List<CardInfo> cards;

    public levelInfo()
    {
        cards = new();
    }

    public string Name { get => name; set => name = value; }
    public string DisplayName { get => displayName; set => displayName = value; }
    public int Level { get => level; set => level = value; }
    public float PlayTime { get => playTime; set => playTime = value; }
    public List<CardInfo> Cards { get => cards; set => cards = value; }
}
[CreateAssetMenu(fileName = "LevelConfig", menuName = "Levels")]
public class LevelConfigSO : ScriptableObject
{
    [SerializeField] private List<levelInfo> levelInfos;
    public List<levelInfo> LevelInfos { get => levelInfos; set => levelInfos = value; }
}
