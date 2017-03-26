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

    private CanvasCard canvasC;

    [HideInInspector]
    public string position;

	// Use this for initialization
	void Start () {
       timeCooldown = cooldown;
        canvasC = GetComponent<CanvasCard>();

    }
	
	// Update is called once per frame
	void Update () {
        if (!usable)
        {
            timeCooldown -= Time.deltaTime;
            canvasC.BottleFill += timeCooldown / cooldown; 
            if(timeCooldown<=0.0f)
            {
                canvasC.Active = true;
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
            canvasC.Active = false;
            canvasC.BottleFill = 0.0f;
            tempMinion = Instantiate(minionS, spawnPoint.transform.position, spawnPoint.transform.rotation);
            tempMinion.GetComponent<Minion>().position = pos;
        }
        
    }
}
