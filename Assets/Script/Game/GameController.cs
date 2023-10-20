using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private ConfigSO gameConfig;
    [SerializeField] private ViewController view;
    [SerializeField] private List<Transform> listPosBar;
    [SerializeField] private Spawner spawner;
    [SerializeField] private LayerMask tileLayerMask;
    [SerializeField] private List<Card> listCardInBar;
    [SerializeField] private LevelConfigSO levelConfig;
    private RaycastHit hitInfo;
    private bool isTouch = false;
    private bool isWin = false;
    private Coroutine comboCorou;
    private int combo;
    private int score;

    private void Awake()
    {
        if (!SoundManager.IsExist())
        {
            SceneManager.LoadSceneAsync("Home");
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        int level = PlayerSaveManager.Instance.GetLevel();
        view.Init(levelConfig.LevelInfos[level].PlayTime, level);
        spawner.SpawnCard(levelConfig.LevelInfos[level]);
    }
    // Update is called once per frame
    private void Update()
    {
        if (isTouch) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 100f, tileLayerMask))
            {
                isTouch = true;
                Card cardTemp = hitInfo.collider.GetComponent<Card>();
                MoveCard(cardTemp);
                ResetBarPos();
            }
        }
    }
    private void MoveCard(Card card)
    {
        SoundManager.Instance.PlaySound("Tap");

        int index = listCardInBar.Count;
        for (int i = 0; i < listCardInBar.Count; i++)
        {
            if (card.CardType == listCardInBar[i].CardType)
            {
                index = i;
                break;
            }
        }
        card.FlyToEmptySlot(listPosBar[index].position, gameConfig.SpeedFlyCard, () => { CheckMatch(card); isTouch = false; });
        listCardInBar.Insert(index, card);
        spawner.ListCard.Remove(card);
        spawner.DictCard[card.CardType].Remove(card);
        if (spawner.DictCard[card.CardType].Count == 0)
        {
            spawner.DictCard.Remove(card.CardType);
        }
    }
    private void ResetBarPos()
    {
        for (int i = 0; i < listCardInBar.Count; i++)
        {
            listCardInBar[i].FlyToEmptySlot(listPosBar[i].position, gameConfig.SpeedFlyCard);
        }
    }
    private void CheckMatch(Card card)
    {
        List<Card> listCardMatch = new List<Card>();
        foreach (Card cardMatch in listCardInBar)
        {
            if (cardMatch.CardType == card.CardType)
            {
                listCardMatch.Add(cardMatch);
            }
        }
        if (listCardMatch.Count == gameConfig.CountMatchCard)
        {
            SoundManager.Instance.PlaySound("Match");

            foreach (Card cardMatch in listCardMatch)
            {
                cardMatch.gameObject.SetActive(false);
                listCardInBar.Remove(cardMatch);
            }
            ResetBarPos();
            if (comboCorou != null)
            {
                StopCoroutine(comboCorou);
            }
            comboCorou = StartCoroutine(CheckCombo());
        }
        CheckWinOrLose();
    }
    private void CheckWinOrLose()
    {
        if (spawner.ListCard.Count <= 0)
        {
            ShowResultGame(true);
        }
        if (listCardInBar.Count == listPosBar.Count)
        {
            ShowResultGame(false);
        }
    }
    private void ShowResultGame(bool isWin)
    {
        if (this.isWin)
        {
            return;
        }
        this.isWin = true;
        if (isWin)
        {
            SoundManager.Instance.PlaySound("Win");
            if (PlayerSaveManager.Instance.GetLevel() < levelConfig.LevelInfos.Count)
            {
                PlayerSaveManager.Instance.SetLevel(PlayerSaveManager.Instance.GetLevel()+1);
            }
        }
        else
        {
            SoundManager.Instance.PlaySound("Lose");
        }
        view.ShowResult(isWin);
    }
    private IEnumerator CheckCombo()
    {
        float timeTemp = gameConfig.CountDownCombo;
        view.ComboBar.value = 1f;
        combo++;
        score += combo * gameConfig.ScoreBonus;
        view.ShowScore(score);
        while (timeTemp > 0)
        {
            timeTemp -= Time.deltaTime;
            float valueBar = timeTemp / gameConfig.CountDownCombo;
            view.SetValueComboBar(valueBar, combo);
            yield return null;
        }
        comboCorou = null;
        combo = 0;
    }
    //----------- Booster ---------------//
    public void RollBackBooster()
    {
        SoundManager.Instance.PlaySound("Booster");

        if (listCardInBar.Count == 0) return;
        int index = listCardInBar.Count - 1;
        listCardInBar[index].ReturnToFloor(spawner.randowmPos(), 1f);
        spawner.ListCard.Add(listCardInBar[index]);
        spawner.DictCard[listCardInBar[index].CardType].Add(listCardInBar[index]);
        listCardInBar.RemoveAt(index);
    }
    public void GetCardBooster()
    {
        SoundManager.Instance.PlaySound("Booster");
        if (listCardInBar.Count == 0)
        {
            int randomKeyDic = UnityEngine.Random.Range(0, spawner.DictCard.Count);
            CardType key = spawner.DictCard.Keys.ElementAt(randomKeyDic);
            Debug.Log(spawner.DictCard[key].Count);
            for (int i = 0; i < 3; i++)
            {
                MoveCard(spawner.DictCard[key][0]);
            }
        }
        else
        {
            List<CardType> cardTypes = new List<CardType>();
            CardType cardTmp = listCardInBar[0].CardType;
            foreach (Card card in listCardInBar)
            {
                if (!cardTypes.Contains(card.CardType))
                {
                    cardTypes.Add(card.CardType);
                }
                else
                {
                    cardTmp = card.CardType;
                    break;
                }
            }
            if (cardTypes.Count == listPosBar.Count - 1)
            {
                return;
            }
            int count = 3 - listCardInBar.Where(n => n.CardType == cardTmp).Count();
            for (int i = 0; i < count; i++)
            {
                MoveCard(spawner.DictCard[cardTmp][0]);
            }
        }
    }
    public void ShuffleCards()
    {
        SoundManager.Instance.PlaySound("Booster");
        foreach (CardType key in spawner.DictCard.Keys)
        {
            foreach (Card card in spawner.DictCard[key])
            {
                Debug.Log(card.name);
                float randomNum = Random.Range(2f, -2f);
                card.Rb.AddForce(new Vector3(randomNum, 5f, randomNum), ForceMode.Impulse);
            }
        }
    }
    public void FreeBooster()
    {
        SoundManager.Instance.PlaySound("Booster");
        view.IsFreeze = true;
    }
}
