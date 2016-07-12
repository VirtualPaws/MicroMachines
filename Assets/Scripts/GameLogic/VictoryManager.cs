using UnityEngine;
using System.Collections;

public class VictoryManager : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        LocalChoiceManager lcm = GameObject.Find("ChoiceManager").GetComponent<LocalChoiceManager>();
        RaceManager rm = GameObject.Find("RaceManager").GetComponent<RaceManager>();
        if (rm.isPlayer1Winner())
        {
            GameObject firstPosition = GameObject.Find("FirstPosition");
            GameObject winner = GameObject.Instantiate(lcm.getPicks()[0]);
            winner.transform.parent = firstPosition.transform;
            winner.transform.localPosition = new Vector3(0, 0, 0);
            winner.transform.localRotation = new Quaternion();
            winner.GetComponent<Rigidbody>().isKinematic = true;
            GameObject secondPosition = GameObject.Find("SecondPosition");
            GameObject sec = GameObject.Instantiate(lcm.getPicks()[1]);
            sec.transform.parent = secondPosition.transform;
            sec.transform.localPosition = new Vector3(0, 0, 0);
            sec.transform.localRotation = new Quaternion();
            sec.GetComponent<Rigidbody>().isKinematic = true;

            GameObject.Find("Zahl").GetComponent<UnityEngine.UI.Text>().text = "1";
        }
        else
        {
            GameObject firstPosition = GameObject.Find("FirstPosition");
            GameObject winner = GameObject.Instantiate(lcm.getPicks()[1]);
            winner.transform.parent = firstPosition.transform;
            winner.transform.localPosition = new Vector3(0, 0, 0);
            winner.transform.localRotation = new Quaternion();
            winner.GetComponent<Rigidbody>().isKinematic = true;
            GameObject secondPosition = GameObject.Find("SecondPosition");
            GameObject sec = GameObject.Instantiate(lcm.getPicks()[0]);
            sec.transform.parent = secondPosition.transform;
            sec.transform.localPosition = new Vector3(0, 0, 0);
            sec.transform.localRotation = new Quaternion();
            sec.GetComponent<Rigidbody>().isKinematic = true;

            GameObject.Find("Zahl").GetComponent<UnityEngine.UI.Text>().text = "2";
        }
        GameObject.Destroy(lcm.gameObject);
        GameObject.Destroy(rm.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
