using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using TMPro;

public class GameManager1 : MonoBehaviour
{
    private static GameManager1 instance;
    public static GameManager1 Instance { get { return instance; } }

    [SerializeField]
    private List<GameObject> arrCard;

    [SerializeField]
    private Transform[] CardSetSpawnPoint;

    [SerializeField]
    private GameObject[] CardPF;

    [SerializeField]
    private Transform CardSpawnParent;

    [SerializeField]
    private TextMeshProUGUI PlayClickCountScore;

    [SerializeField]
    private TextMeshProUGUI PlayTimeScore;

    [SerializeField]
    private TextMeshProUGUI EndClickCountScore;

    [SerializeField]
    private TextMeshProUGUI EndTimeScore;

    [SerializeField]
    private GameObject ResultCanvas;

    private List<Card> cards = new List<Card>();

    private int ClickCountScore = 0;
    private float TimeScore = 0;

    private List<Card> selectedCards = new List<Card>();
    private bool isProcessing = false; // 연산 중 클릭 방지

    private int ClickCount = 0;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        SelectCard();
        CardSpawnParent.gameObject.SetActive(true);
        ResultCanvas.SetActive(false);
    }

    private void Start()
    {
        DrowCard();
    }
    private void Update()
    {
        PlayingScore();
        if (EndGame())
        {
            ResultGame();
        }
    }

    public void ProcessCardClick(Card clickedCard)
    {
        if (isProcessing || clickedCard.GetMatch() || selectedCards.Contains(clickedCard)) return;

        selectedCards.Add(clickedCard);
        StartCoroutine(ReverseCard(clickedCard.transform));

        if (selectedCards.Count == 2)
        {
            StartCoroutine(CheckMatchRoutine());
        }
    }

    private IEnumerator CheckMatchRoutine()
    {
        isProcessing = true;
        yield return new WaitForSeconds(0.5f);

        if (selectedCards[0].GetCardNumber() == selectedCards[1].GetCardNumber())
        {
            ParticleManager.Instance.PlayMatchEffect(selectedCards[0].transform.position);
            ParticleManager.Instance.PlayMatchEffect(selectedCards[1].transform.position);

            selectedCards[0].SetMatch(true);
            selectedCards[1].SetMatch(true);
        }
        else
        {
            StartCoroutine(ReverseCard(selectedCards[0].transform));
            StartCoroutine(ReverseCard(selectedCards[1].transform));
        }
        ClickCount++;
        selectedCards.Clear();
        isProcessing = false;
    }

    public IEnumerator ReverseCard(Transform _trs)
    {
        Quaternion startRot = _trs.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0f, 180f, 0f);
        float duration = 0.3f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            _trs.rotation = Quaternion.Slerp(startRot, targetRot, time / duration);
            yield return null;
        }
        _trs.rotation = targetRot;
    }
    private void SelectCard()
    {
        CardPF = new GameObject[CardSetSpawnPoint.Length];
        for (int i = 0; i < CardSetSpawnPoint.Length / 2; i++)
        {
            int number = Random.Range(0, arrCard.Count);
            CardPF[i] = arrCard[number];
            arrCard.RemoveAt(number);
        }
        for (int i = 0; i < CardPF.Length / 2; i++)
        {
            while (true)
            {
                int number = Random.Range(0, arrCard.Count);
                if (CardPF[i].GetComponent<Card>().GetCardNumber() == arrCard[number].GetComponent<Card>().GetCardNumber())
                {
                    CardPF[i + 8] = arrCard[number];
                    arrCard.RemoveAt(number);
                    break;
                }
            }
        }
    }

    private void DrowCard()
    {
        for (int i = 0; i < CardSetSpawnPoint.Length; i++)
        {
            int ran = Random.Range(i, CardPF.Length);
            GameObject temp = CardPF[i];
            CardPF[i] = CardPF[ran];
            CardPF[ran] = temp;
            GameObject obj = Instantiate(CardPF[i], CardSpawnParent);
            cards.Add(obj.GetComponent<Card>());
            obj.gameObject.transform.position = CardSetSpawnPoint[i].position;
            obj.gameObject.transform.localRotation = CardSetSpawnPoint[i].localRotation;
            obj.gameObject.transform.localScale = CardSetSpawnPoint[i].localScale;
            StartCoroutine(StartGame(obj));
        }
    }
    private IEnumerator StartGame(GameObject _obj)
    {
        _obj.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);

        yield return new WaitForSeconds(1.5f);

        Quaternion startRot = _obj.transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0f, 180f, 0f);

        float duration = 0.6f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            _obj.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        _obj.transform.rotation = targetRot;

    }

    private void PlayingScore()
    {
        if(!EndGame())
            TimeScore += Time.deltaTime;
        ClickCountScore = ClickCount;
        PlayClickCountScore.text = ClickCountScore.ToString();
        PlayTimeScore.text = ((int)TimeScore).ToString();
    }

    private bool EndGame()
    {
        bool AllMatched = true;
        foreach (Card card in cards)
        {
            if (card.GetMatch() == false)
            {
                AllMatched = false;
                return AllMatched;
            }
        }
        return AllMatched;
    }
    private void ResultGame()
    {
        cards.Clear();
        CardSpawnParent.gameObject.SetActive(false);
        ResultCanvas.SetActive(true);
        EndClickCountScore.text = ClickCountScore.ToString();
        EndTimeScore.text = ((int)TimeScore).ToString();
    }
}
