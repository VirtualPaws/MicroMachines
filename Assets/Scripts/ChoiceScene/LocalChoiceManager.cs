using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LocalChoiceManager : MonoBehaviour {

    public List<GameObject> carPrefabs;
    public GameObject carHolder;
    public GameObject carHolder2;

    public KeyCode enterKey = KeyCode.Return;
    public KeyCode enterKey2 = KeyCode.Space;
    private string enterButton = "Vertical";

    private List<GameObject> cars;
    private List<GameObject> cars2;
    private int indexP1 = 0;
    private int indexP2 = 1;

    public bool player1HasControls = false;
    public bool player2HasControls = false;

    public string player1Controls;
    public string player2Controls;

    private GameObject picked;
    private GameObject picked2;

    private bool classicCameraMode = false;

	// Use this for initialization
    void Start()
    {
        Object.DontDestroyOnLoad(this);
        cars = new List<GameObject>();
        cars2 = new List<GameObject>();
        foreach (GameObject go in carPrefabs)
        {
            GameObject copy = Instantiate(go);
            copy.transform.parent = carHolder.transform;
            copy.transform.localPosition = new Vector3(0, 0, 0);
            copy.transform.localRotation = new Quaternion();
            copy.GetComponent<Rigidbody>().isKinematic = true;
            cars.Add(copy);
            GameObject copy2 = Instantiate(go);
            copy2.transform.parent = carHolder2.transform;
            copy2.transform.localPosition = new Vector3(0, 0, 0);
            copy2.transform.localRotation = new Quaternion();
            copy2.GetComponent<Rigidbody>().isKinematic = true;
            cars2.Add(copy2);
        }
        displayRightCar();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (!player1HasControls)
        {
            if (Input.GetKeyDown(enterKey))
            {
                player1Controls = "Keyboard";
                player1HasControls = true;
            }
            if (Input.GetKeyDown(enterKey2))
            {
                player1Controls = "Keyboard2";
                player1HasControls = true;
            }
            else if (Input.GetButtonDown(enterButton + "2"))
            {
                player1Controls = "Controller1";
                player1HasControls = true;
            }
            else if (Input.GetButtonDown(enterButton + "3"))
            {
                player1Controls = "Controller2";
                player1HasControls = true;
            }
        }
        else if(!player2HasControls)
        {
            if (Input.GetKeyDown(enterKey) && !player1Controls.Equals("Keyboard"))
            {
                player2Controls = "Keyboard";
                player2HasControls = true;
            }
            if (Input.GetKeyDown(enterKey2))
            {
                player2Controls = "Keyboard2";
                player2HasControls = true;
            }
            else if (Input.GetButtonDown(enterButton+"2") && !player1Controls.Equals("Controller1"))
            {
                player2Controls = "Controller1";
                player2HasControls = true;
            }
            else if (Input.GetButtonDown(enterButton + "3") && !player1Controls.Equals("Controller2"))
            {
                player2Controls = "Controller2";
                player2HasControls = true;
            }
        }
        if (player1HasControls && player2HasControls)
        {
            begin();
        }
    }

    public void switchCarP1Right()
    {
        indexP1 = (indexP1 + 1) % cars.Count;
        displayRightCar();
    }

    public void switchCarP1Left()
    {
        indexP1 = (indexP1 + cars.Count - 1) % cars.Count;
        displayRightCar();
    }

    public void switchCarP2Right()
    {
        indexP2 = (indexP2 + 1) % cars2.Count;
        displayRightCar();
    }

    public void switchCarP2Left()
    {
        indexP2 = (indexP2 + cars2.Count - 1) % cars2.Count;
        displayRightCar();
    }

    public void setCameraToClassic()
    {
        classicCameraMode = true;
    }

    public void setCameraTo3rdPerson()
    {
        classicCameraMode = false;
    }

    private void displayRightCar()
    {
        foreach (GameObject go in cars)
        {
            go.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        }
        cars[indexP1].transform.GetChild(0).GetComponent<Renderer>().enabled = true;

        foreach (GameObject go in cars2)
        {
            go.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        }
        cars2[indexP2].transform.GetChild(0).GetComponent<Renderer>().enabled = true;
    }

    public List<GameObject> getPicks()
    {
        List<GameObject> ret = new List<GameObject>();
        ret.Add(carPrefabs[indexP1]);
        ret.Add(carPrefabs[indexP2]);
        return ret;
    }

    public void begin()
    {
        GameObject picked = carPrefabs[indexP1];
        GameObject picked2 = carPrefabs[indexP2];

        SceneManager.LoadScene("textureScene");
    }

    public bool isCameraModeClassic()
    {
        return classicCameraMode;
    }

    public bool isCameraMode3rdPerson()
    {
        return !classicCameraMode;
    }

    public List<string> getControls()
    {
        List<string> controls = new List<string>();
        controls.Add(player1Controls);
        controls.Add(player2Controls);
        return controls;
    }
}
