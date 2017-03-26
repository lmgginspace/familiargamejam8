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
    private Dictionary<string, float> map;

    public AudioClip attackSound;
    public AudioClip dieSound;

    #region Unity Functions

    void Start () {
        gameSceneManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSceneManager>();
        map = GameManager.Instance.getMapForLevel(GameManager.Instance.Level);
        healthMax = map["villain_health"];
        attack = map["villain_attack"];
        health = healthMax;
        anim = GetComponent<Animator>();
        
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

    void Sound() {
        AudioManager.Instance.PlaySoundEffect(attackSound);
    }

    void SoundDie() {
        AudioManager.Instance.PlaySoundEffect(dieSound);
    }

    GameObject MyObjective()
    {
        return hero;
    }


    #endregion

}
