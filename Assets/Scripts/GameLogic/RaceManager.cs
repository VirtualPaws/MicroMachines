using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceManager : MonoBehaviour {

    public List<int> scoreByPlayerIndex;
    private RacingCheckpoint racingFor;
    private RacingCheckpoint lastCheckPoint;
    private List<GameObject> playersThroughCheckpoint;
    private List<GameObject> playersKO;
    private float timeStarted = 0;
    private int pointsForCheckPointRace = 100;
    private int pointsForKO = 150;

    public float timeLimit = 5; //seconds

	// Use this for initialization
	void Start () {
        playersKO = new List<GameObject>();
        scoreByPlayerIndex = new List<int>();
        scoreByPlayerIndex.Add(0);
        scoreByPlayerIndex.Add(0);
        lastCheckPoint = GameObject.FindGameObjectsWithTag("Checkpoint")[0].GetComponent<RacingCheckpoint>();
	}
	
	// Update is called once per frame
	void Update () {
        if (racingFor != null)
        {
            if (Time.time > timeStarted + timeLimit)
            {
                racingFor = null;
                int multiplyer = GameObject.FindGameObjectsWithTag("Car").Length - playersThroughCheckpoint.Count;
                foreach (GameObject player in playersThroughCheckpoint)
                {
                    if (playersKO.Contains(player))
                    {
                        continue;
                    }
                    if (player.name.Equals("Player1"))
                    {
                        incrementPlayerScore(0, pointsForCheckPointRace * multiplyer);
                        GameObject.Find("Canvas").transform.Find("Player1").GetComponent<GUIMultiplayer>().setScore(getPlayerScore(0));
                    }
                    if (player.name.Equals("Player2"))
                    {
                        incrementPlayerScore(1, pointsForCheckPointRace * multiplyer);
                        GameObject.Find("Canvas").transform.Find("Player2").GetComponent<GUIMultiplayer>().setScore(getPlayerScore(1));
                    }
                    if (player.name.Equals("Player3"))
                    {
                        incrementPlayerScore(2, pointsForCheckPointRace * multiplyer);
                        GameObject.Find("Canvas").transform.Find("Player3").GetComponent<GUIMultiplayer>().setScore(getPlayerScore(2));
                    }
                    if (player.name.Equals("Player4"))
                    {
                        incrementPlayerScore(3, pointsForCheckPointRace * multiplyer);
                        GameObject.Find("Canvas").transform.Find("Player4").GetComponent<GUIMultiplayer>().setScore(getPlayerScore(3));
                    }
                }
            }
        }
        if (GameObject.FindGameObjectsWithTag("Car").Length == playersKO.Count)
        {
            reSpawnKOdPlayersAt(lastCheckPoint);
        }
	}

    public void incrementPlayerScore(int playerIndex, int scoreIncrement)
    {
        while (scoreByPlayerIndex.Count <= playerIndex)
        {
            scoreByPlayerIndex.Add(0);
        }
        scoreByPlayerIndex[playerIndex] = scoreByPlayerIndex[playerIndex] + scoreIncrement;
    }

    public int getPlayerScore(int playerIndex)
    {
        if (playerIndex >= scoreByPlayerIndex.Count)
        {
            return 0;
        }
        return scoreByPlayerIndex[playerIndex];
    }

    public void hitCheckpoint(RacingCheckpoint checkPoint, GameObject player)
    {
        lastCheckPoint = checkPoint;
        if (racingFor == null)
        {
            //Begin a race for the checkpoint
            racingFor = checkPoint;
            playersThroughCheckpoint = new List<GameObject>();
            playersThroughCheckpoint.Add(player);
            timeStarted = Time.time;
        }
        else if (racingFor == checkPoint && !playersThroughCheckpoint.Contains(player))
        {
            //handle other players reaching the racing checkpoint
            playersThroughCheckpoint.Add(player);
            if (playersThroughCheckpoint.Count >= GameObject.FindGameObjectsWithTag("Car").Length)
            {
                //All players have reached the checkpoint in time. Reset it all and wait for the next checkpoint to be hit.
                racingFor = null;
            }
        }
    }

    public void KOplayer(GameObject player)
    {
        playersKO.Add(player);
        //player.GetComponent<Driving>().enabled = false;
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Car"))
        {
            if (p != player) {

                if (playersKO.Contains(p))
                {
                    continue;
                }
                if (p.name.Equals("Player1"))
                {
                    incrementPlayerScore(0, pointsForKO);
                    GameObject.Find("Canvas").transform.Find("Player1").GetComponent<GUIMultiplayer>().setScore(getPlayerScore(0));
                }
                if (p.name.Equals("Player2"))
                {
                    incrementPlayerScore(1, pointsForKO);
                    GameObject.Find("Canvas").transform.Find("Player2").GetComponent<GUIMultiplayer>().setScore(getPlayerScore(1));
                }
                if (p.name.Equals("Player3"))
                {
                    incrementPlayerScore(2, pointsForKO);
                    GameObject.Find("Canvas").transform.Find("Player3").GetComponent<GUIMultiplayer>().setScore(getPlayerScore(2));
                }
                if (p.name.Equals("Player4"))
                {
                    incrementPlayerScore(3, pointsForKO);
                    GameObject.Find("Canvas").transform.Find("Player4").GetComponent<GUIMultiplayer>().setScore(getPlayerScore(3));
                }
            }
        }
    }

    public void reSpawnKOdPlayersAt(RacingCheckpoint checkpoint)
    {
        Transform respawn1 = checkpoint.transform.Find("Respawn1");
        Transform respawn2 = checkpoint.transform.Find("Respawn2");
        if (playersKO.Count > 1)
        {
            playersKO[0].transform.position = respawn1.position;
            playersKO[0].transform.rotation = respawn1.rotation;
            playersKO[1].transform.position = respawn2.position;
            playersKO[1].transform.rotation = respawn2.rotation;
        }
        playersKO = new List<GameObject>();
    }
}
