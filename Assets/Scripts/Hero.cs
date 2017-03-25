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
    public float healthMax = 10000;
    public float health;
    public float attack = 20;
    public float attackRate = 1;

    public float magicAttack = 100;
    public float mana = 0;
    public float manaMax = 100;
    public float manaAcum = 10;

    public bool fury = false;
    public float furyRate = 30;
    public float furyChance = 0.2f;

    public float stun = 0;

    public GameObject villain;

    [SerializeField] private float timeToAttack = 0;
    [SerializeField] private float timeToFury = 0;

    private Animator anim;
    private GameSceneManager gameSceneManager;

	#region Unity Functions

	void Start () {
        health = healthMax;
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

                if (stun>0) {
                    // Stuneado
                    stun = Mathf.Max(stun - Time.deltaTime, 0);

                } else {
                    // Si no estamos stuneados

                    // Sólo ocurre si el nivel es 3 o mas
                    if (GameManager.Instance.Level >= 3) {
                        if (timeToFury > furyRate) {
                            Fury();
                            if (!fury) {
                                timeToFury -= 1;
                                Debug.Log("Tiempo alargado");
                            } else {
                                gameSceneManager.furies++;
                                timeToFury = 0;
                            }
                            Debug.Log("FURY: " + fury.ToString());
                        } else {
                            timeToFury += Time.deltaTime;
                        }
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
            
    }
    void Attack(Tuple<int, GameObject> objective)
    {
        StartCoroutine(DelayandAttack(attackRate / 4, objective.Item1, objective.Item2));
        
        
    }

    IEnumerator DelayandAttack(float seconds, int index, GameObject objective) {
        
        bool flipX = GetComponent<SpriteRenderer>().flipX;
        bool cond_changeFlip = index == 0;
        if (flipX != cond_changeFlip) {
            // No coinciden las orientaciones. Debe girar: tiempo de espera
            GetComponent<SpriteRenderer>().flipX = cond_changeFlip;
            yield return new WaitForSeconds(seconds);
        }
        if (index == 0) {
            objective.GetComponent<Villain>().health -= attack;
            mana = Mathf.Min(mana + manaAcum, manaMax);
        } else {
            //TODO objective.GetComponent<Minion>().health -= attack; 
        }
        anim.SetTrigger("attack");
        
    }

    void Fury() {
        if (fury) {
            fury = false;
            timeToFury = 0;
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
