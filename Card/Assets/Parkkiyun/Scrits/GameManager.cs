using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }

    [SerializeField]
    private List<GameObject> arrCard;

    [SerializeField]
    private Transform[] CardSetSpawnPoint;

    [SerializeField]
    private GameObject[] CardPF;

    [SerializeField]
    private Transform CardSpawn;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        CardPF = new GameObject[CardSetSpawnPoint.Length];
        for(int i = 0; i < CardSetSpawnPoint.Length / 2; i++)
        {
            int number = Random.Range(0, arrCard.Count);
            CardPF[i] = arrCard[number];
            arrCard.RemoveAt(number);
        }
        for(int i = 0; i < CardPF.Length / 2; i++)
        {
            while(true)
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

    private void Start()
    {
        for(int i = 0;  i < CardSetSpawnPoint.Length; i++)
        {
            int ran = Random.Range(i, CardPF.Length);
            GameObject temp = CardPF[i];
            CardPF[i] = CardPF[ran];
            CardPF[ran] = temp;
            GameObject obj = Instantiate(CardPF[i], CardSpawn);
            obj.gameObject.transform.position = CardSetSpawnPoint[i].position;
            obj.gameObject.transform.localRotation = CardSetSpawnPoint[i].localRotation;
            obj.gameObject.transform.localScale = CardSetSpawnPoint[i].localScale;
            StartCoroutine(StartGame(obj));
        }
        
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hitInfo))
            {
                Debug.Log(hitInfo.collider.name);
                Transform trs = hitInfo.collider.transform;
            }
        }

        
    }

    private IEnumerator StartGame(GameObject _obj)
    {
        _obj.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        yield return new WaitForSeconds(1.5f);
        _obj.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }



 
}
