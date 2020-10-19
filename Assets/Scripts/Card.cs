using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int value;
    public string color;

    public bool selectedCard;
    public Player owner;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        Select();
    }

    public void Show(Vector3 _position, bool shadowed)
    {
        gameObject.transform.position = _position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public override string ToString()
    {
        if (value == 0 && color == "atout") { return "excuse"; }
        return value + "_" + color;
    }

    public void Rename()
    {
        gameObject.name = ToString();
    }

    public void Unselect()
    {
        if (owner != null)
        {
            owner.selectedCard = null;
            selectedCard = false;
            Debug.Log(ToString() + " unselected");
        }
    }

    public void Select()
    {
        if (selectedCard)
        {
            Unselect();
        }
        else
        {
            if (owner != null && owner.hand.Contains(this))
            {
                if (owner.selectedCard != null)
                {
                    owner.selectedCard.Unselect();
                }
                owner.selectedCard = this;
                selectedCard = true;
                Debug.Log(ToString() + " selected");
            }
        }
    }

    public void Highlight()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void Unhighlight()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
