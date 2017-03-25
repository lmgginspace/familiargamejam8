using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions.System;
using System;

public class Minion : MonoBehaviour {

    public bool atackable;

    public float health;

    public KindMinion kindMinionValue;

    public KindDisrepair kindDisrepairValue;

    [HideInInspector]
    public enum KindMinion{
        Decoy,
        Disrepair
    }

    [HideInInspector]
    public enum KindDisrepair
    {
        None,
        Mana,
        DPS,
        Damage,
        Stun
    }

    public float disrepairTop;

    public float disrepairMid;

    public float disrepairBot;

    public GameObject throwDisrepairObject;

    public Transform SpawnDisrepairObject;

    [HideInInspector]
    public string position;

    [HideInInspector]
    public GameObject heroSht;

	// Use this for initialization
	void Start () {

        heroSht = GameObject.FindGameObjectWithTag("Hero").gameObject;

        if (kindMinionValue == KindMinion.Decoy)
        {

        }else
        {

        }


    }
	
	// Update is called once per frame
	void Update () {
       
        {
            if (KindDisrepair.Stun==kindDisrepairValue)
            {
               
            }else
            {

            }
        }

	}

    public void throwDisrepair(GameObject tDisrepair,Transform spawnD)
    {
        int tempPos=0;
        Instantiate(tDisrepair, spawnD.position, this.transform.rotation);
        switch (position)
        {
            case "top":
                tempPos = 0;
                break;
            case "mid":
                tempPos = 1;
                break;
            case "bot":
                tempPos = 2;
                break;
        }
        tDisrepair.GetComponent<Animator>().SetInteger("position",tempPos);
    }

    public void disrepairHero(float disrepairH, KindDisrepair kDisrepair)
    {
        switch (kDisrepair)
        {
            case KindDisrepair.DPS:
                heroSht.GetComponent<Hero>().attackRate *= disrepairH; 
                    break;
            case KindDisrepair.Mana:
                heroSht.GetComponent<Hero>().mana *= disrepairH;
                break;
            case KindDisrepair.Damage:
                heroSht.GetComponent<Hero>().attack *= disrepairH;
                break;
            case KindDisrepair.Stun:
               // heroSht.GetComponent<Hero>().stun += disrepairH;
                break;
        }
       
    }

}
