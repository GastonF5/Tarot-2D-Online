using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private PartyManager pm;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (pm.cardGame.Count != 0)
            {
                StartDonne();
            }
            else { Debug.LogError("La donne a déjà été faite"); }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pm.cardGame.Count != 0) { Debug.LogError("La donne n'a pas été faite"); }
            else { StartEncheres(); }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (Card card in pm.cardGame)
            {
                card.Show(Vector2.zero, false);
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (Card card in pm.cardGame)
            {
                card.Hide();
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartPartie();
        }
    }

    public void Wait()
    {
        pm.state = "waiting";
    }

    public void StartDonne()
    {
        Debug.Log("Début donne");
        pm.state = "donne";
    }

    public void StartEncheres()
    {
        Debug.Log("Début enchères");
        pm.state = "encheres";
    }

    public void StartPartie()
    {
        Debug.Log("Début partie");
        pm.state = "partie";
    }
}
