using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions.System;
using System;

public class Minion : MonoBehaviour {

    public bool atackable;

    public float health;

    #region enumValues
    public KindMinion kindMinionValue;

    public KindDisrepair kindDisrepairValue;
    #endregion

    #region enumTypes
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
    #endregion

    #region disrepairValues
    public float disrepairTop;

    public float disrepairMid;

    public float disrepairBot;

    private float disrepairValue;
    #endregion

    public GameObject throwDisrepairObject;

    public Transform SpawnDisrepairObject;

    private GameSceneManager gameSceneManager;

    [HideInInspector]
    public string position;

    [HideInInspector]
    public GameObject heroSht;

    // Use this for initialization
    void Start()
    {
        gameSceneManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSceneManager>();
        heroSht = GameObject.FindGameObjectWithTag("Hero").gameObject;

        switch (position)
        {
            case "top":
                disrepairValue = disrepairTop;
                gameSceneManager.minion_up = gameObject;
                break;
            case "mid":
                disrepairValue = disrepairMid;
                gameSceneManager.minion_middle = gameObject;
                break;
            case "bot":
                disrepairValue = disrepairBot;
                gameSceneManager.minion_down = gameObject;
                break;
        }

        if (kindMinionValue != KindMinion.Decoy)
        {
            if (KindDisrepair.Stun == kindDisrepairValue)
            {
                disrepairHero(disrepairValue, kindDisrepairValue);
                Destroy(this.gameObject, 2f);
            }
            else
            {
                disrepairHero(disrepairValue, kindDisrepairValue);
            }

        }
    }
	
	// Update is called once per frame
	void Update () {

        if (health<=0.0f)
        {
            die();
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
                heroSht.GetComponent<Hero>().stun += disrepairH;
                break;
        }
    }

    public void replaceStatHero(float disrepairH, KindDisrepair kDisrepair)
    {
        switch (kDisrepair)
        {
            case KindDisrepair.DPS:
                heroSht.GetComponent<Hero>().attackRate /= disrepairH;
                break;
            case KindDisrepair.Mana:
                heroSht.GetComponent<Hero>().mana /= disrepairH;
                break;
            case KindDisrepair.Damage:
                heroSht.GetComponent<Hero>().attack /= disrepairH;
                break;
            case KindDisrepair.Stun:
                heroSht.GetComponent<Hero>().stun -= disrepairH;
                break;
        }
    }

    public void die()
    {
        if (position=="top") {
            gameSceneManager.minion_up = null;
        }
        if (position == "mid") {
            gameSceneManager.minion_middle = null;
        }
        if (position == "bot") {
            gameSceneManager.minion_down = null;
        }
        replaceStatHero(disrepairValue,kindDisrepairValue);
        Destroy(this.gameObject);
    }

}
