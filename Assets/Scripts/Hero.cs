/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions.Tuple;
using Extensions.UnityEngine;

public class Hero : MonoBehaviour {

    #region Variables
    #endregion
    public float health = 10000;
    public float attack = 20;
    public float attackRate = 1;

    public float magicAttack = 100;
    public float mana = 0;
    public float manaMax = 100;
    public float manaAcum = 10;

    public bool fury = false;
    public float furyRate = 1/30;
    public float furyChance = 0.2f;

    public GameObject villain;

    [SerializeField] private float timeToAttack = 0;
    [SerializeField] private float timeToFury = 0;

    private Animator anim;
    private GameSceneManager gameSceneManager;

	#region Unity Functions

	void Start () {
        anim = GetComponent<Animator>();
        gameSceneManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSceneManager>();
	}
	
	/// <summary>
    /// 
    /// </summary>
    void Update () {
        
            if (!gameSceneManager.gameover)
            {

                if (health <= 0) {
                    anim.SetTrigger("death");
                } else {


                    if (timeToFury > furyRate) {
                        Fury();
                        if (!fury) {
                            timeToFury -= 1;
                            Debug.Log("Tiempo alargado");
                        } else {
                            timeToFury = 0;
                        }
                        Debug.Log("FURY: " + fury.ToString());
                    } else {
                        timeToFury += Time.deltaTime;
                    }

                    if (timeToAttack > attackRate) {
                        timeToAttack = 0;
                        if (mana >= manaMax) {
                            MagicAttack(gameSceneManager.ChooseObjective(GameSceneManager.MAGIC_ATTACK));
                            mana = 0;
                        } else {
                            Attack(gameSceneManager.ChooseObjective(GameSceneManager.NORMAL_ATTACK));
                        }
                    } else {
                        timeToAttack += Time.deltaTime;
                    }

                }
            }            
            
    }
    void Attack(Tuple<int, GameObject> objective)
    {
        if (objective.Item2.tag == "Villain")
        {
            Debug.Log(objective.Item2);
            objective.Item2.GetComponent<Villain>().health -= attack; //TODO
        }
        else if (objective.Item2.tag == "Minion")
        {
            //TODO objective.Item2.GetComponent<Minion>().health -= attack; 
        }
        mana = Mathf.Min(mana + manaAcum, manaMax);
        anim.SetTrigger("attack");
    }

    void Fury() {
        if (fury) {
            fury = false;
        } else {
            fury = RandomUtil.Chance(furyChance);
        }
    }

    void MagicAttack(Tuple<int, GameObject> objective)
    {
        objective.Item2.GetComponent<Villain>().health -= magicAttack; //TODO
        anim.SetTrigger("magic" + objective.Item1);
    }

    #endregion


}
