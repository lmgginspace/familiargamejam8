using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public bool select;

    public GameObject minionS;

    public float coldown;

    public List<GameObject> positionList;
    
    [HideInInspector]
    public string position;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    public void spawnMinion(GameObject spawnPoint,string pos)
    {
        GameObject tempMinion;
        tempMinion = Instantiate(minionS,spawnPoint.transform.position,spawnPoint.transform.rotation);

        tempMinion.GetComponent<Minion>().position= pos;
    }
}
