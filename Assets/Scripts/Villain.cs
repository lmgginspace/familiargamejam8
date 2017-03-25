/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain : MonoBehaviour {

    #region Variables
    #endregion
    public float health = 1000;
    public float attack = 10;
    public float attackRate = 1;



    public GameObject hero;

    
    private float timeToAttack = 0;

    private Animator anim;
    private GameSceneManager gameSceneManager;


    #region Unity Functions

    void Start () {
        anim = GetComponent<Animator>();
        gameSceneManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSceneManager>();
    }
	
	void Update () {
        if (health <= 0)
        {
            anim.SetTrigger("death");
        } else
        {
            if (Time.time > timeToAttack && !gameSceneManager.gameover)
            {
                timeToAttack = Time.time + 1 / attackRate;
                Attack(MyObjective());
            }
        }
       
    }

    void Attack(GameObject objective)
    {
        objective.GetComponent<Hero>().health -= 100;
        anim.SetTrigger("attack");
    }

    GameObject MyObjective()
    {
        return hero;
    }


    #endregion

}
