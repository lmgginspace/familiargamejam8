/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions.Tuple;

public class Hero : MonoBehaviour {

    #region Variables
    #endregion
    public float health = 300;
    public float attack = 20;
    public float attackRate = 1;

    public float magicAttack = 100;
    public float mana = 0;
    public float manaMax = 100;
    public float manaAcum = 10;

    public bool fury = false;

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
                if (mana >= manaMax)
                {
                    MagicAttack(gameSceneManager.ChooseObjective(GameSceneManager.MAGIC_ATTACK));
                } else
                {
                    Attack(gameSceneManager.ChooseObjective(GameSceneManager.NORMAL_ATTACK));
                }
            }
        }
    }
    void Attack(Tuple<int, GameObject> objective)
    {
        
        if (objective.Item2.tag == "Villain")
        {
            objective.Item2.GetComponent<Villain>().health -= attack; //TODO
        }
        else if (objective.Item2.tag == "Minion")
        {
            //TODO objective.Item2.GetComponent<Minion>().health -= attack; 
        }
        mana = Mathf.Min(mana + manaAcum, manaMax);
        anim.SetTrigger("attack"+objective.Item1);
    }

    void MagicAttack(Tuple<int, GameObject> objective)
    {
        objective.Item2.GetComponent<Villain>().health -= magicAttack; //TODO
        anim.SetTrigger("magic" + objective.Item1);
    }

    #endregion


}
