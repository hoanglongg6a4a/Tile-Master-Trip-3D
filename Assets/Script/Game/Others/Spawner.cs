using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Card prefab;
    [SerializeField] private Material material;
    [SerializeField] private Dictionary<CardType, List<Card>> dictCard;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private List<Card> listCard;
    [SerializeField] private int num;
    [SerializeField] private ConfigSO config;
    private levelInfo levelInfo;
    public List<Card> ListCard { get => listCard; set => listCard = value; }
    public Dictionary<CardType, List<Card>> DictCard { get => dictCard; set => dictCard = value; }

    public void SpawnCard(levelInfo levelInfo)
    {
        this.levelInfo = levelInfo;
        StartCoroutine(DelaySpawn());
    }
    private IEnumerator DelaySpawn()
    {
        DictCard = new();
        List<CardType> cardTypeRandom = new List<CardType>();
        Dictionary<CardType, Material> DictCardType = new Dictionary<CardType, Material>();
        foreach (CardInfo cardInfo in levelInfo.Cards)
        {
            DictCard.Add(cardInfo.Type, new List<Card>());
            Material materialPrefab = Instantiate(material);
            materialPrefab.mainTexture = cardInfo.Sprite.texture;
            for (int i = 0; i < cardInfo.Quantity; i++)
            {
                cardTypeRandom.Add(cardInfo.Type);
            }
            DictCardType.Add(cardInfo.Type, materialPrefab);
        }
        cardTypeRandom = cardTypeRandom.OrderBy(x => Random.Range(0f, 100f)).ToList();
        foreach (CardType type in cardTypeRandom)
        {
            yield return new WaitForSeconds(0.02f);
            Card spawnPrefab = Instantiate(prefab, randowmPos(), Quaternion.Euler(0f,Random.Range(0f,360f),0f));
            dictCard[type].Add(spawnPrefab);
            spawnPrefab.Init(type, DictCardType[type]);
            listCard.Add(spawnPrefab);
        }
    }
    public Vector3 randowmPos()
    {
        float randomX = Random.Range(-5f, 5f);
        float randomZ = Random.Range(7f, -5f);
        return new Vector3(randomX, 3f, randomZ);
    }
}
