/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    #region Variables
    #endregion
    public float health = 300;
    public float attack = 20;
    public float attackRate = 1;

    public GameObject villain;

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
        }
        else
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
        objective.GetComponent<Villain>().health -= 100;
        anim.SetTrigger("attack");
    }

    GameObject MyObjective()
    {
        return villain;
    }

    #endregion


}
