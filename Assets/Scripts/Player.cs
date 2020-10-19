using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Card> hand;
    public List<Card> plis;

    public Card selectedCard;
    public GameObject handObject;
    public Vector3 handPosition;

    public int[] nbCardsPerColor;
    private string[] colors = { "carreau", "coeur", "pique", "trefle", "atout" };

    private void Start()
    {
        hand = new List<Card>();
        plis = new List<Card>();
        handObject = GameObject.Find("Hand");
        handPosition = handObject.transform.position;
        nbCardsPerColor = new int[5];
    }

    private void Update()
    {
        nbCardsPerColor[(int)PartyManager.colors.Carreau] = GetNbCardsInHandOfColor((int)PartyManager.colors.Carreau);
        nbCardsPerColor[(int)PartyManager.colors.Coeur] = GetNbCardsInHandOfColor((int)PartyManager.colors.Coeur);
        nbCardsPerColor[(int)PartyManager.colors.Pique] = GetNbCardsInHandOfColor((int)PartyManager.colors.Pique);
        nbCardsPerColor[(int)PartyManager.colors.Trefle] = GetNbCardsInHandOfColor((int)PartyManager.colors.Trefle);
        nbCardsPerColor[(int)PartyManager.colors.Atout] = GetNbCardsInHandOfColor((int)PartyManager.colors.Atout);
    }

    public void Rename(string _name)
    {
        gameObject.name = _name;
    }

    public void ShowPlis(Vector3 _position, Card _backCard)
    {
        if (plis.Count != 0)
        {
            _backCard.Show(_position, false);
        }
    }
    
    public void HideEveryCards()
    {
        foreach (Card _card in hand) { _card.Hide(); }
        foreach (Card _card in plis) { _card.Hide(); }
    }

    public int GetNbCardsInHandOfColor(int _colorIndex)
    {
        int _count = 0;
        foreach (Card _card in hand)
        {
            if (_card.color == colors[_colorIndex]) { _count++; }
        }
        return _count;
    }
}
