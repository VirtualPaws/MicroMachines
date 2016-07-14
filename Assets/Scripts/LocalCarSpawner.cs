using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalCarSpawner : MonoBehaviour {

    private List<GameObject> players;

	// Use this for initialization
    void Start()
    {
        GameObject choiceManager = GameObject.Find("ChoiceManager");
        if (choiceManager == null)
        {
            Debug.Log("NO CHOICE MANAGER");
            Destroy(gameObject);
            return;
        }
        players = new List<GameObject>();
        List<GameObject> picks = choiceManager.GetComponent<LocalChoiceManager>().getPicks();
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpunkt");
        List<string> controls = choiceManager.GetComponent<LocalChoiceManager>().getControls();
        int i = -1;
        foreach (GameObject pick in picks)
        {
            i++;
            GameObject spawn = Instantiate(pick);
            spawn.transform.position = spawnPoints[i % spawnPoints.Length].transform.position;
            spawn.transform.rotation = spawnPoints[i % spawnPoints.Length].transform.rotation;
            spawn.GetComponent<Driving>().enabled = true;
            spawn.name = "Player" + (i+1);
            spawn.GetComponent<Driving>().inputDevice = controls[i % controls.Count];
            players.Add(spawn);
            Transform label = spawn.transform.Find("PlayernameLabel");
            if(label == null) {
                continue;
            }
            label.GetComponent<TextMesh>().text = spawn.name;
            label.GetComponent<FaceCamera>().toFace = GameObject.Find("MultipurposeCameraRig_" + (2 - i)).transform.Find("Pivot").Find("MainCamera").gameObject;
        }
	}

    public GameObject getPlayerByNumber(int number)
    {
        return players[number];
    }

    public GameObject getPlayer1()
    {
        return players[0];
    }

    public GameObject getPlayer2()
    {
        return players[1];
    }
	
	// Update is called once per frame
    void Update()
    {
	}
}
