using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour {
    private PlayerGameModel player1;
    private PlayerGameModel player2;

    private PlayerGameModel leadingPlayer;
    private PlayerGameModel chasingPlayer;

	public GameObject knockoutManager;

    private RacingCheckpoint racingFor;
    private RacingCheckpoint lastCheckPoint;
    private bool postRace = false;

    private float timeStarted = 0;
    private int pointsForKO = 100;
    private int pointsToWin = 300;

    private PlayerGameModel winner;
    private bool gameOver = false;

    public float timeLimit = 5; //seconds

    private AudioSource coinSfx;
    private AudioSource koSfx;

	// Use this for initialization
    void Start()
    {
        Object.DontDestroyOnLoad(this);
	}

    private void initialise()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        if (cars.Length < 2)
        {
            return;
        }

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

        knockoutManager = GameObject.Find("KnockoutManager");

        coinSfx = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameOver)
        {
            return;
        }
        if (player1 == null || player2 == null)
        {
            initialise();
        }
        if (player1.score >= pointsToWin || player2.score >= pointsToWin)
        {
                endGame();
        }
        if (racingFor != null)
        {
            if (Time.time > timeStarted + timeLimit)
            {
                KOplayer(chasingPlayer, leadingPlayer);
            }
			knockoutManager.GetComponent<GUIKnockout> ().tickCounter (getTimeLeft()+1);
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
        Debug.Log(player.name + " Hitting Checkpoint");
        PlayerGameModel[] models = getModels(player);
        PlayerGameModel selectedPlayer = models[0];
        PlayerGameModel otherPlayer = models[1];
        if (selectedPlayer.KO)
        {
            Debug.Log(">>> But " + selectedPlayer.name + " is KO");
            return;
        }
        if (postRace)
        {
            Debug.Log(">>> Respawning " + otherPlayer.name);
            reSpawnKOdPlayersAt(checkPoint);
			knockoutManager.GetComponent<GUIKnockout> ().endCounter();
        }
        else if (racingFor == null && checkPoint != lastCheckPoint)
        {
            Debug.Log(">>> A Race Begins with " + selectedPlayer.name + " leading and " + otherPlayer.name + " chasing.");
            //Begin a race for the checkpoint
            racingFor = checkPoint;
            leadingPlayer = selectedPlayer;
            chasingPlayer = otherPlayer;
            timeStarted = Time.time;

			knockoutManager.GetComponent<GUIKnockout>().startCounter (getNumberOfPlayerChasing());
        }
        else if (racingFor == checkPoint && selectedPlayer == chasingPlayer)
        {
            Debug.Log(">>> " + selectedPlayer.name + " reached Checkpoint in time");
            //handle other players reaching the racing checkpoint
            racingFor = null;
            postRace = true;
        }
        lastCheckPoint = checkPoint;
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
        if (selectedPlayer.KO)
        {
            return;
        }
        selectedPlayer.KO = true;
        selectedPlayer.car.GetComponent<PointIndicatorParticles>().firePenaltyParticles();
        selectedPlayer.car.GetComponent<Driving>().canDrive = false;
        //player.GetComponent<Driving>().enabled = false;
        otherPlayer.car.GetComponent<PointIndicatorParticles>().fireAwardParticles();
        otherPlayer.score += pointsForKO;
        coinSfx.Play();
        GameObject.Find("Canvas").transform.Find(otherPlayer.name).GetComponent<GUIMultiplayer>().setScore(otherPlayer.score);
        racingFor = null;
        postRace = true;

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
            player1.car.GetComponent<Driving>().canDrive = true;
        }
        if (player2.KO)
        {
            player2.car.transform.position = respawn2.position;
            player2.car.transform.rotation = respawn2.rotation;
            player2.KO = false;
            player2.car.GetComponent<Driving>().canDrive = true;
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
