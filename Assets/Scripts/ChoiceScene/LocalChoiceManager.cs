using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LocalChoiceManager : MonoBehaviour {

    public List<GameObject> carPrefabs;
    public GameObject carHolder;
    public GameObject carHolder2;
    private List<GameObject> cars;
    private List<GameObject> cars2;
    private int indexP1 = 0;
    private int indexP2 = 1;

    private GameObject picked;
    private GameObject picked2;

    private bool classicCameraMode = true;

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
	void Update () {

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
}
