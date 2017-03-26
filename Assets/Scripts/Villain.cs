/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain : MonoBehaviour {

    #region Variables
    #endregion
    public float healthMax = 1000;
    public float health;
    public float attack = 10;
    public float attackRate = 1;

    public GameObject hero;

    private float timeToAttack = 0;

    private Animator anim;
    private GameSceneManager gameSceneManager;


    #region Unity Functions

    void Start () {
        health = healthMax;
        anim = GetComponent<Animator>();
        gameSceneManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSceneManager>();
    }

    void Update() {


        if (!gameSceneManager.gameover) {

            if (health <= 0) {
                anim.SetTrigger("death");
            } else {

                if (timeToAttack > attackRate) {
                    timeToAttack = 0;
                    Attack(MyObjective());
                } else {
                    timeToAttack += Time.deltaTime;
                }

            }

        }
    }


    void Attack(GameObject objective)
    {
        objective.GetComponent<Hero>().health -= attack;
        anim.SetTrigger("attack");
    }

    GameObject MyObjective()
    {
        return hero;
    }


    #endregion

}
