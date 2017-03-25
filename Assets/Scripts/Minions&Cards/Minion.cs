using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour {
    public string name;

    public bool atackable;

    public float health;

    public enum kindMinion{
        stun,
        decoy,
        disrepair
    }

    public enum kindDispair
    {
        mana,
        dps,
        damage
    }

    public float dispairTop;

    public float dispairMid;

    public float dispairBot;

    [HideInInspector]
    public string position;

    [HideInInspector]
    public GameObject heroSht;

	// Use this for initialization
	void Start () {

        heroSht = GameObject.FindGameObjectWithTag("Hero").gameObject;

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void dispairHero(float dispairH, string kDispair)
    {
        //heroSht
    }

}
