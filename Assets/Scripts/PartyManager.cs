using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private GraphicManager gm;
    [SerializeField]
    private EventManager em;

    public enum colors { Carreau, Coeur, Pique, Trefle, Atout };

    public string state;

    public List<Player> players;
    public List<Player> takerCamp;
    public List<Player> defenderCamp;
    public int nbPlayers;
    public Player activePlayer;

    public List<Card> cardsOnBoard;
    public List<Card> fixedCardGame;
    public List<Card> cardGame;
    public List<Card> chien;
    private bool chienFait;
    public int nbCardsInChien;
    public string askedColor;

    public GameObject playerPrefab;
    public GameObject playerSpawner;
    public GameObject cardGameObject;
    public GameObject chienObject;
    public GameObject boardObject;
    public GameObject plisObject;
    public GameObject activePlayerText;

    public void Start()
    {
        nbPlayers = 3;
        AddPlayers(nbPlayers);
        activePlayer = players[0];
        askedColor = "";

        if (nbPlayers == 5)
        {
            nbCardsInChien = 3;
        }
        else if (nbPlayers == 3 || nbPlayers == 4)
        {
            nbCardsInChien = 6;
        }

        cardGame = CreateCardGame();
        foreach (Card _card in cardGame)
        {
            fixedCardGame.Add(_card);
        }
    }

    private void Update()
    {
        if (players.Count != nbPlayers)
        {
            state = "waiting";
        }

        switch (state)
        {
            case "donne":
                // si le jeu de carte n'est pas vide, on distribue
                if (cardGame.Count != 0)
                {
                    // on constitue le chien tant qu'il ne contient pas 6 cartes
                    if (chien.Count < nbCardsInChien)
                    {
                        int _nbCardsInGame = cardGame.Count;
                        int _index = Random.Range(0, _nbCardsInGame - 1);
                        Card _card = cardGame[_index];
                        chien.Add(_card);
                        gm.ChangeParent(_card.gameObject, chienObject);
                        _card.Hide();
                        cardGame.Remove(_card);
                    }
                    // lorsque le chien est constitué, on distribue 3 cartes au joueur actif
                    else
                    {
                        DistribuerCartes(activePlayer, 3);
                        NextPlayer();
                    }
                }
                // le jeu est vide : la donne est finie
                else
                {
                    gm.ShowCards(chien, chienObject.transform.position);
                    Debug.Log("Fin donne");
                    em.Wait();
                }
                break;

            case "encheres":
                em.Wait();
                break;

            case "partie":
                activePlayerText.GetComponent<Text>().text = activePlayer.ToString();

                if (cardsOnBoard.Count == 1)
                {
                    askedColor = cardsOnBoard[0].color;
                }
                else if (cardsOnBoard.Count == 0)
                {
                    askedColor = "";
                }

                // tous les joueurs ont joué une carte
                if (cardsOnBoard.Count == nbPlayers)
                {
                    Player _winner = WinningCard().owner;
                    Debug.Log(_winner.ToString() + " gagne le pli");
                    foreach (Card _card in cardsOnBoard)
                    {
                        // on ajoute la carte sur le board aux plis du joueur gagnant
                        _winner.plis.Add(_card);
                        // la carte appartient maintenant au joueur gagnant
                        _card.owner = _winner;
                        gm.ChangeParent(_card.gameObject, plisObject);
                    }
                    cardsOnBoard.RemoveRange(0, nbPlayers);
                    activePlayer.HideEveryCards();
                    activePlayer = _winner;
                }
                break;

            case "waiting":
                break;
        }
    }

    private void DistribuerCartes(Player _player, int _nbCards)
        /*
         * Distribue "nbCards" cartes aléatoires au joueur "player"
         */
    {
        int _nbCardsInGame = cardGame.Count;
        Card _card;
        for (int i = 0; i < _nbCards; i++)
        {
            int _index = Random.Range(0, _nbCardsInGame - 1);
            _card = cardGame[_index];
            _card.Hide();
            cardGame.Remove(_card);
            _player.hand.Add(_card);
            _card.owner = _player;
            gm.ChangeParent(_card.gameObject, activePlayer.gameObject);
            _nbCardsInGame = cardGame.Count;
        }
    }

    private List<Card> CreateCardGame()
        /*
         * Crée un jeu de cartes complet
         */
    {
        Debug.Log("Début création jeu de cartes");
        List<Card> _cardGame = new List<Card>(78);
        int _value;
        string _color = "";

        // on initialise le jeu de carte
        for (int i = 1; i <= 78; i++)
        {
            if (i <= 14) { _value = i; _color = "carreau"; }
            else if (i > 14 && i <= 28) { _value = i - 14; _color = "coeur"; }
            else if (i > 28 && i <= 42) { _value = i - 28; _color = "pique"; }
            else if (i > 42 && i <= 56) { _value = i - 42; _color = "trefle"; }
            // les cartes au-delà de 56 sont les atouts
            else { _value = i - 57; _color = "atout"; }
            
            _cardGame.Add(gm.SpawnCard(_value, _color, cardGameObject));
        }
        Debug.Log("Fin création jeu de cartes");

        return _cardGame;
    }

    private void NextPlayer()
        /*
         * Passe du joueur actif au joueur suivant
         */
    {
        activePlayer.HideEveryCards();
        int _index = players.IndexOf(activePlayer);
        activePlayer = players[(_index + 1) % nbPlayers];
    }

    private void AddPlayers(int _nb)
    {
        Debug.Log("Debut ajout joueurs");
        for (int i = 0; i < _nb; i++)
        {
            GameObject _playerObject = Instantiate(playerPrefab, playerSpawner.transform);
            Player _player = _playerObject.GetComponent<Player>();
            players.Add(_player);
            _player.Rename("Player" + (i + 1));
            Debug.Log(_player.name + " instancié");
        }
        Debug.Log("Fin ajout joueurs");
    }

    public void PlayACard()
    {
        if (activePlayer.selectedCard != null)
        {
            Card _selectedCard = activePlayer.selectedCard;
            _selectedCard.Unselect();
            cardsOnBoard.Add(_selectedCard);
            gm.ChangeParent(_selectedCard.gameObject, boardObject);
            activePlayer.hand.Remove(_selectedCard);
            NextPlayer();
            Debug.Log(_selectedCard.ToString() + " jouée");
            Debug.Log("Au tour de " + activePlayer.ToString());
        }
        else
        {
            Debug.Log("Pas de carte sélectionnée");
        }
    }
    
    public Card WinningCard()
        /*
         * Non fonctionnelle
         */
    {
        Card _wCard = cardsOnBoard[0];
        for (int i = 1; i < nbPlayers; i++)
        {
            int _wVal = _wCard.value;
            string _askedColor = _wCard.color;
            Card _card = cardsOnBoard[i];
            if (_card.color == _askedColor && _card.value > _wVal) { _wCard = _card; }
            // si la couleur demandée n'est pas un atout mais que la carte est un atout (hormis l'excuse)
            if (_askedColor != "atout" && _card.color == "atout" && _card.value != 0) { _wCard = _card; }
        }

        return _wCard;
    }
}
