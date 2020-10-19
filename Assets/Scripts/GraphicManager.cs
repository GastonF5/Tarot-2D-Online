using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicManager : MonoBehaviour
{
    public GameObject cardPrefab;

    [SerializeField]
    private PartyManager pm;

    public int cardWidth;
    public float cardHeight;

    private void Start()
    {
        cardWidth = 50;
        cardHeight = 100;
    }

    private void Update()
    {
        foreach (GameObject _cardObject in GameObject.FindGameObjectsWithTag("Card"))
        {
            Card _card = _cardObject.GetComponent<Card>();
            if (_card.selectedCard)
            {
                _card.Highlight();
            }
            else
            {
                _card.Unhighlight();
            }
        }

        switch (pm.state)
        {
            case "partie":
                // affichage des cartes du joueur actif
                ShowHand(pm.activePlayer);
                ShowCardsOnBoard();
                ShowCards(pm.activePlayer.plis, pm.plisObject.transform.position);
                break;
        }
    }

    public Card SpawnCard(int _value, string _color, GameObject _parent)
    {
        // spawn the card
        GameObject _cardObject = Instantiate(cardPrefab, _parent.transform);

        Card _card = _cardObject.GetComponent<Card>();
        // card properties
        _card.value = _value;
        _card.color = _color;
        _card.Rename();

        // text button
        Text _text = _cardObject.GetComponentInChildren<Text>();
        _text.text = _card.ToString();

        return _card;
    }

    public void ChangeParent(GameObject _object, GameObject _newParent)
    {
        _object.transform.SetParent(_newParent.transform);
    }

    public void ShowHand(Player _player)
    {
        bool _selectable;
        int _handWidth = (_player.hand.Count * cardWidth) / 2;
        Vector3 _position = _player.handPosition;
        _position.x -= _handWidth / 2;
        foreach (Card _card in _player.hand)
        {
            _selectable = true;
            if (pm.askedColor == "atout")
            {
                if (_player.nbCardsPerColor[(int)PartyManager.colors.Atout] != 0 && _card.color != "atout") { _selectable = false; }
            }
            else
            {

            }
            _card.Show(_position, !_selectable);
            _position.x += cardWidth / 2;
        }
    }
    public void ShowCardsOnBoard()
    {
        Vector3 _position = pm.boardObject.transform.position;
        int _delta = cardWidth * 2;
        int _width = (pm.cardsOnBoard.Count - 1) * _delta + cardWidth;
        _position.x -= _width / 2;
        foreach (Card _card in pm.cardsOnBoard)
        {
            _card.Show(_position, false);
            _position.x += _delta;
        }
    }

    public void ShowCards(List<Card> _cards, Vector3 _position)
    {
        foreach (Card _card in _cards)
        {
            _card.Show(_position, false);
        }
    }

    public void HideCards()
    {
        foreach (Card _card in pm.fixedCardGame)
        {
            _card.Hide();
        }
    }
}
