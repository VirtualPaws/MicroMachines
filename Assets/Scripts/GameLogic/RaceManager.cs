using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour {

    private PlayerGameModel player1;
    private PlayerGameModel player2;

    private PlayerGameModel leadingPlayer;
    private PlayerGameModel chasingPlayer;

	public GameObject knockoutManager_1;
	public GameObject knockoutManager_2;

    private RacingCheckpoint racingFor;
    private RacingCheckpoint lastCheckPoint;
    private bool postRace = false;

    private float timeStarted = 0;
    private int pointsForKO = 100;
    private int pointsToWin = 300;

    private PlayerGameModel winner;
    private bool gameOver = false;

    public float timeLimit = 5; //seconds


	// Use this for initialization
    void Start()
    {
        Object.DontDestroyOnLoad(this);

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        player1 = new PlayerGameModel();
        player1.name = "Player1";
        player1.playerNumber = 1;
        player1.car = cars[0];

        player2 = new PlayerGameModel();
        player2.name = "Player2";
        player2.playerNumber = 2;
        player2.car = cars[1];

        Debug.Log("set player variables");

        lastCheckPoint = GameObject.FindGameObjectsWithTag("Checkpoint")[0].GetComponent<RacingCheckpoint>();

		knockoutManager_1 = GameObject.Find ("KnockoutManager_1");
		knockoutManager_2 = GameObject.Find ("KnockoutManager_2");

	}
	
	// Update is called once per frame
	void Update () {
        if (gameOver || player1 == null || player2 == null)
        {
            return;
        }
        if (player1.score >= pointsToWin || player2.score >= pointsToWin)
        {
                endGame();
        }
        if (racingFor != null)
        {
            if (Time.time > timeStarted + timeLimit)
            {
                racingFor = null;
                postRace = true;
                //leadingPlayer.score += pointsForKO;
                KOplayer(chasingPlayer, leadingPlayer);
            }
			knockoutManager_1.GetComponent<GUIKnockout> ().tickCounter (getTimeLeft()+1);
        }

        if (player1.KO && player2.KO)
        {
            reSpawnKOdPlayersAt(lastCheckPoint);
        }
	}

    public int getPlayerScore(int playerIndex)
    {
        if (playerIndex > 1)
        {
            return 0;
        }
        if (playerIndex == 0)
        {
            return player1.score;
        }
        return player2.score;
    }

    private PlayerGameModel[] getModels(GameObject player)
    {
        PlayerGameModel selectedPlayer = null;
        PlayerGameModel otherPlayer = null;
        if (player.name.Equals(player1.name))
        {
            selectedPlayer = player1;
            otherPlayer = player2;
        }
        else
        {
            selectedPlayer = player2;
            otherPlayer = player1;
        }
        PlayerGameModel[] ret = new PlayerGameModel[2];
        ret[0] = selectedPlayer;
        ret[1] = otherPlayer;
        return ret;
    }

    public void hitCheckpoint(RacingCheckpoint checkPoint, GameObject player)
    {
        PlayerGameModel[] models = getModels(player);
        PlayerGameModel selectedPlayer = models[0];
        PlayerGameModel otherPlayer = models[1];
        if (selectedPlayer.KO)
        {
            return;
        }
        lastCheckPoint = checkPoint;
        if (postRace)
        {
            reSpawnKOdPlayersAt(checkPoint);
			knockoutManager_1.GetComponent<GUIKnockout> ().endCounter();
            return;
        }
        if (racingFor == null)
        {
            //Begin a race for the checkpoint
            racingFor = checkPoint;
            leadingPlayer = selectedPlayer;
            chasingPlayer = otherPlayer;
            timeStarted = Time.time;

			knockoutManager_1.GetComponent<GUIKnockout>().startCounter ();
        }
        else if (racingFor == checkPoint && selectedPlayer == chasingPlayer)
        {
            //handle other players reaching the racing checkpoint
            racingFor = null;
            postRace = true;
        }
    }

    public void KOplayer(GameObject player)
    {
        PlayerGameModel[] models = getModels(player);
        PlayerGameModel selectedPlayer = models[0];
        PlayerGameModel otherPlayer = models[1];
        KOplayer(selectedPlayer, otherPlayer);
    }

    public void KOplayer(PlayerGameModel selectedPlayer, PlayerGameModel otherPlayer)
    {
        selectedPlayer.KO = true;
        selectedPlayer.car.GetComponent<PointIndicatorParticles>().firePenaltyParticles();
        //player.GetComponent<Driving>().enabled = false;
        otherPlayer.car.GetComponent<PointIndicatorParticles>().fireAwardParticles();
        otherPlayer.score += pointsForKO;
    }

    public void reSpawnKOdPlayersAt(RacingCheckpoint checkpoint)
    {
        Transform respawn1 = checkpoint.transform.Find("Respawn1");
        Transform respawn2 = checkpoint.transform.Find("Respawn2");
        if (player1.KO)
        {
            player1.car.transform.position = respawn1.position;
            player1.car.transform.rotation = respawn1.rotation;
            player1.KO = false;
        }
        if (player2.KO)
        {
            player2.car.transform.position = respawn2.position;
            player2.car.transform.rotation = respawn2.rotation;
            player2.KO = false;
        }
        postRace = false;
        racingFor = null;
    }

    public int getTimeLeft()
    {
        if (racingFor == null)
        {
            return -1;
        }
        return (int)(timeStarted - Time.time + timeLimit);
    }

    public void endGame()
    {
        if (player1.score > player2.score)
        {
            winner = player1;
        }
        else
        {
            winner = player2;
        }
        gameOver = true;
        SceneManager.LoadScene("EndScene");
    }

    public int getNumberOfPlayerLeading()
    {
        return leadingPlayer.playerNumber;
    }

    public int getNumberOfPlayerChasing()
    {
        return chasingPlayer.playerNumber;
    }

    public bool isPlayer1Winner()
    {
        return (winner.playerNumber == 1);
    }
}
