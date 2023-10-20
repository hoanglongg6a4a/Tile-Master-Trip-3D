using UnityEngine;
[CreateAssetMenu(fileName = "New Config", menuName = "Config")]
public class ConfigSO : ScriptableObject
{
    [Header("Game Play")]
    [SerializeField] private int timeMatch;
    [SerializeField] private float speedFlyCard;
    [SerializeField] private int countMatchCard;
    [SerializeField] private int scoreBonus;
    [SerializeField] private float countDownCombo;
    [Header("Card")]
    [SerializeField] private float scaleDownRatio;
    [SerializeField] private float forceToLipCard;
    [SerializeField] private float limitToLipCard;
    public int TimeMatch { get => timeMatch; set => timeMatch = value; }
    public float SpeedFlyCard { get => speedFlyCard; set => speedFlyCard = value; }
    public int CountMatchCard { get => countMatchCard; set => countMatchCard = value; }
    public int ScoreBonus { get => scoreBonus; set => scoreBonus = value; }
    public float CountDownCombo { get => countDownCombo; set => countDownCombo = value; }
    public float ForceToLipCard { get => forceToLipCard; set => forceToLipCard = value; }
    public float LimitToLipCard { get => limitToLipCard; set => limitToLipCard = value; }
    public float ScaleDownRatio { get => scaleDownRatio; set => scaleDownRatio = value; }
}
