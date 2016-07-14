using UnityEngine;
using System.Collections;

public class OilBehavior : MonoBehaviour {
    public float growthTime = 0.5f;//seconds
    public float lifeTime = 6f;//seconds
    private float born;

	// Use this for initialization
	void Start () {
        born = Time.time;
        this.gameObject.transform.localScale = new Vector3(0, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - born > lifeTime + 2 * growthTime)
        {
            Destroy(this.gameObject);
            return;
        }
        if (Time.time - born > lifeTime + growthTime)
        {
            float scl = 1-(Time.time - born - lifeTime - growthTime) / growthTime;
            this.gameObject.transform.localScale = new Vector3(scl, 1, scl);
            return;
        }
        if (Time.time - born < growthTime)
        {
            float scl = (Time.time - born) / growthTime;
            this.gameObject.transform.localScale = new Vector3(scl, 1, scl);
            return;
        }
        float scll = 1;
        this.gameObject.transform.localScale = new Vector3(scll, 1, scll);
	}
}
