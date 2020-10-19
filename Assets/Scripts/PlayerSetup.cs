using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    private GraphicManager gm;
    private PartyManager pm;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            // boucle pour désactiver les components des autres joueurs sur notre instance
            foreach (Behaviour behaviour in componentsToDisable)
            {
                behaviour.enabled = false;
            }
        }

        gm = GameObject.Find("GraphicManager").GetComponent<GraphicManager>();
        pm = GameObject.Find("PartyManager").GetComponent<PartyManager>();
        gm.ChangeParent(gameObject, pm.playerSpawner);
    }
}
