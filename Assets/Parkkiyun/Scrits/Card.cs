using UnityEngine;

public enum CardNumber
{
    One = 1,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}

public class Card : MonoBehaviour
{
    [SerializeField]
    private CardNumber cardnumber;

    private bool match;

    public CardNumber GetCardNumber()
    {
        return cardnumber; 
    }

    public bool GetMatch()
    {
        return match;
    }
    public void SetMatch(bool _bool)
    {
        match = _bool;
    }

    public Card(CardNumber _cardnumber)
    {
        cardnumber = _cardnumber;
    }
}
