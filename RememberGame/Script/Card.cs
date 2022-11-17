using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    private bool isFront            = true;
    private float turnSpeed         = 4f;
    private int cardLocationNumber  = 0;

    private Image Card_Image;
    private RectTransform rectTransform;
    private TextMeshProUGUI Card_NumberTM;

    public void GetLocateNumber(int locatenumber)
    {
        cardLocationNumber = locatenumber;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isFront = true;
        StartCoroutine(rotateCard());
    }

    #region Get, Set
    private Sprite card_Front_Sprite;
    public Sprite Card_Sprite
    {
        set
        {
            Card_Image = this.gameObject.GetComponentInChildren<Image>();
            card_Front_Sprite = value;
            Card_Image.sprite = card_Front_Sprite;
        }
        get => card_Front_Sprite;
    }

    private int card_Number;
    public int Card_Number
    {
        set
        {
            Card_NumberTM = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            card_Number = value;
        }
        get => card_Number;
    }

    private GameManager gameManager;
    public GameManager GameManager
    {
        set
        {
            gameManager = value;
            rectTransform = this.gameObject.GetComponent<RectTransform>();
        }
        get => gameManager;
    }
    #endregion


    #region 카드 리프레쉬 과정
    public void RefreshCard()
    {
        Card_Image.gameObject.SetActive(true);
        Card_NumberTM.gameObject.SetActive(true);
        isFront = false;
        CardSwap(isFront);
    }

    public void WrongCardRefresh()
    {
        isFront = false;
        StartCoroutine(rotateCard());
    }
    #endregion

    #region 카드 체크 과정
    public void CheckCorrect()
    {
        Card_Image.gameObject.SetActive(false);
        Card_NumberTM.gameObject.SetActive(false);
    }
    #endregion

    #region 카드 돌리기 과정
    private void CardSwap(bool isfront)
    {
        if (isfront)
        {
            Card_Image.sprite = null;
            Card_NumberTM.text = card_Number.ToString();
        }
        else
        {
            Card_Image.sprite = card_Front_Sprite;
            Card_NumberTM.text = null;
        }
    }

    //https://intrepidgeeks.com/tutorial/create-an-animation-that-flips-the-unity-2d-card
    private IEnumerator rotateCard()
    {
        float tick = 0f;

        Vector3 startScale = new Vector3(1.0f, 1.0f, 1.0f); 
        Vector3 endScale = new Vector3(0f, 1.0f, 1.0f);

        Vector3 localScale = new Vector3();


        while (tick < 1.0f)
        {
            tick += Time.deltaTime * turnSpeed;

            localScale = Vector3.Lerp(startScale, endScale, tick);

            rectTransform.localScale = localScale;

            yield return null;
        }

        CardSwap(isFront);

        isFront = !isFront;

        tick = 0f;
        while (tick < 1.0f)
        {
            tick += Time.deltaTime * turnSpeed;

            localScale = Vector3.Lerp(endScale, startScale, tick);

            rectTransform.localScale = localScale;

            yield return null;
        }

        if(isFront == false)
            gameManager.ClickCardDataCheck(card_Number, cardLocationNumber);
    }
    #endregion
}
