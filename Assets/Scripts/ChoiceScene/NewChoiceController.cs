using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class NewChoiceController : NetworkBehaviour
{
    public int chosenNumber = 0;
    public string[] prefabPaths;
    public List<GameObject> previews;

    void Start()
    {
        foreach (string path in prefabPaths)
        {
            GameObject previewObject = (GameObject)Instantiate(Resources.Load("previews/" + path));
            previewObject.SetActive(false);
            previews.Add(previewObject);
        }
        previews[chosenNumber].SetActive(true);
    }

    //in der variablen car steckt immer nur das ausgewählte auto
    void Update()
    {

        if (Input.GetKeyDown("left"))
        {
            GameObject player = getLocalPlayerRep();
            if (player == null)
            {
                return;
            }
            previews[chosenNumber].SetActive(false);
            chosenNumber = (chosenNumber - 1 + prefabPaths.Length) % prefabPaths.Length;
            previews[chosenNumber].SetActive(true);
            getPlayerModel(player).CmdSetCarPrefabPath(prefabPaths[chosenNumber]);
        }
        if (Input.GetKeyDown("right"))
        {
            GameObject player = getLocalPlayerRep();
            if (player == null)
            {
                return;
            }
            previews[chosenNumber].SetActive(false);
            chosenNumber = (chosenNumber + 1) % prefabPaths.Length;
            previews[chosenNumber].SetActive(true);
            getPlayerModel(player).CmdSetCarPrefabPath(prefabPaths[chosenNumber]);
        }
    }

    GameObject getLocalPlayerRep()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length < 1)
        {
            return null;
        }
        foreach (GameObject pl in players)
        {
            ClientSidePlayer clientSidePlayer = (ClientSidePlayer)pl.GetComponent(typeof(ClientSidePlayer));
            if (clientSidePlayer.isLocalPlayer)
            {
                return pl;
            }
        }
        return null;
    }

    PlayerModel getPlayerModel(GameObject player)
    {
        return (PlayerModel)player.GetComponent(typeof(PlayerModel));
    }

    ClientSidePlayer getClientSidePlayer(GameObject player)
    {
        return (ClientSidePlayer)player.GetComponent(typeof(ClientSidePlayer));
    }

    public void setReady()
    {
        if (getPlayerModel(getLocalPlayerRep()).carPrefabPath == null || getPlayerModel(getLocalPlayerRep()).carPrefabPath == "")
        {
            getPlayerModel(getLocalPlayerRep()).CmdSetCarPrefabPath(prefabPaths[chosenNumber]);
        }
        getClientSidePlayer(getLocalPlayerRep()).CmdSetReady(true);
        //getClientSidePlayer(getLocalPlayerRep()).CmdSpawn();
    }
}
