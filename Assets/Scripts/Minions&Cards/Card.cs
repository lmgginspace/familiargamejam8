using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public bool select;

    [HideInInspector]
    public bool usable = true;

    public GameObject minionS;

    public float cooldown;

    private float timeCooldown;

    public ColorType cardColor;
    

    public enum ColorType {
        RED,
        BLUE,
        GREEN
    };

    [HideInInspector]
    public string position;

	// Use this for initialization
	void Start () {
       timeCooldown = cooldown;
	}
	
	// Update is called once per frame
	void Update () {
        if (!usable)
        {
            timeCooldown -= Time.deltaTime;
            if(timeCooldown<=0.0f)
            {
                usable = true;
                timeCooldown = cooldown;
            }
        }
	}

    public void spawnMinion(GameObject spawnPoint,string pos)
    {
        if (usable)
        {
            GameObject tempMinion;
            usable = false;
            tempMinion = Instantiate(minionS, spawnPoint.transform.position, spawnPoint.transform.rotation);
            tempMinion.GetComponent<Minion>().position = pos;
        }
        
    }
}
