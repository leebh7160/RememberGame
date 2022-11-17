using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] RandomImage;

    [SerializeField]
    private GameObject CardObject;

    [SerializeField]
    private Transform CardSortted;

    [SerializeField]
    private CardUI CardUI;

    private List<Card> CardList;

    private int matchA = -1;
    private int matchALocation = -1;
    private int matchB = -1;
    private int matchBLocation = -1;
    private int cardMatchEnd = 0;


    void Start()
    {
        CardUI.GameManager = this.GetComponent<GameManager>();
        CardList = new List<Card>();
    }

    public void GameStart()
    {
        RandomCardMaking();
    }

    #region 카드 제작 과정

    private void RandomCardMaking()
    {
        int CardMaximun = 1;
        int c_number = 2;

        while (CardMaximun < 13)
        {
            float nameNumber = Mathf.Floor(c_number * 0.5f);
            CardMake((int)nameNumber);
            c_number += 1;
            CardMaximun += 1;
        }
        StartCoroutine(CardSuffle());
    }

    private void CardMake(int number)
    {
        GameObject instantObj   = Instantiate(CardObject, CardSortted);

        Card instantCard        = instantObj.AddComponent<Card>();
        instantCard.GameManager = this.gameObject.GetComponent<GameManager>();
        instantCard.Card_Number = number;
        instantCard.Card_Sprite = RandomImage[Random.Range(0, 4)];
        CardList.Add(instantCard);
    }

    private IEnumerator CardSuffle()
    {
        float current = 0;
        float percent = 0;
        float time = 1.5f;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            int index = Random.Range(0, 12);
            CardList[index].transform.SetAsLastSibling();

            yield return null;
        }

        if(percent >= 1)
        {
            for(int i = 0; i < CardList.Count; i++)
            {
                CardList[i].GetLocateNumber(i);
            }
        }
    }

    private void CardReSuffle()
    {
        float current = 0;
        float percent = 0;
        float time = 1.5f;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            int index = Random.Range(0, 12);
            CardList[index].transform.SetAsLastSibling();
        }

        if (percent >= 1)
        {
            for (int i = 0; i < CardList.Count; i++)
            {
                CardList[i].GetLocateNumber(i);
            }
        }
    }

    #endregion
    #region 카드 재 셔플 과정
    public void Card_ReSuffle()
    {
        CardReSuffle();
    }

    public void Card_Refrash()
    {
        for (int i = 0; i < CardList.Count; i++)
        {
            CardList[i].RefreshCard();
            CardList[i].Card_Sprite = RandomImage[Random.Range(0, 4)];
        }
    }

    public void Card_RePlay()
    {
        for (int i = 0; i < CardList.Count; i++)
        {
            Destroy(CardList[i].gameObject);
        }
        CardList.Clear();
        GameStart();
    }

    #endregion
    #region 카드 판별 과정

    public void ClickCardDataCheck(int cardnumber, int card_location_number)
    {
        if(matchA == -1)
        {
            matchA = cardnumber;
            matchALocation = card_location_number;
        }
        else if(matchA != -1)
        {
            matchB = cardnumber;
            matchBLocation = card_location_number;
        }

        if (matchA != -1 && matchB != -1)
            ClickCardMatchCheck();
    }
    private void ClickCardMatchCheck()
    {
        if(matchA == matchB)
        {
            //점수
            CardList[matchALocation].CheckCorrect();
            CardList[matchBLocation].CheckCorrect();
            CardUI.ScorePlus();
            cardMatchEnd += 1;

            if(cardMatchEnd == 6)
            {
                Card_ReSuffle();
                Card_Refrash();
                cardMatchEnd = 0;
            }
        }
        else
        {
            CardList[matchALocation].WrongCardRefresh();
            CardList[matchBLocation].WrongCardRefresh();
        }
        matchA = -1;
        matchALocation = -1;
        matchB = -1;
        matchBLocation = -1;

    }
    #endregion
}

