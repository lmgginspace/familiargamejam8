/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    public float originalAttackRate = 1;
    public bool attacking = false;

    public float magicAttack = 100;
    public float mana = 0;
    public float manaMax = 100;
    public float manaAcum = 10;
    public bool magicAttacking = false;

    
    public bool fury = false;
    public float furyValue = 0.5f;
    public float furyRate = 30;
    public float furyChance = 0.2f;

    public float blockingRate = 20;
    public float blockingTime = 10;
    public bool blocking = false;

    public float stun = 0;

    public GameObject villain;

    public SpriteRenderer[] part_bodies;
    public ParticleSystem spell;
    public ParticleSystem spell2;

    public Text damagePrefab;
    public float lastDamage = -1;
    public float lastDamageMagic = -1;

    public AudioClip heroAttack;
    public AudioClip heroMagic;



    [SerializeField] private float timeToAttack = 0;
    [SerializeField] private float timeToFury = 0;
    [SerializeField]
    private float timeToBlocking = 0;

    private Animator anim;
    private GameSceneManager gameSceneManager;

    private float originalAnimationSpeed;

	#region Unity Functions

	void Start () {
        attacking = false;
        blocking = false;
        fury = false;
        health = healthMax;
        anim = GetComponent<Animator>();
        originalAnimationSpeed = anim.speed;
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

                    if (timeToBlocking > blockingRate) {
                        blocking = true;
                        timeToBlocking = 0;
                        timeToFury = 0;
                        timeToAttack = 0;
                    } else {
                        timeToBlocking += Time.deltaTime;
                        // no bloquea
                        // Sólo ocurre si el nivel es 3 o mas
                        if (GameManager.Instance.Level >= 1 && timeToFury > furyRate) {
                            Fury();
                            if (fury) {
                                gameSceneManager.furies++;
                                timeToFury = 0;
                                timeToAttack = 0;
                            }
                        } else {
                            timeToFury += Time.deltaTime;
                            // no furia

                            if (timeToAttack > attackRate) {
                                timeToAttack = 0;
                                attacking = true;
                            } else {
                                timeToAttack += Time.deltaTime;
                            }
                        }
                    }

                    if (blocking) {
                        Blocks();
                    }

                    if (attacking) {
                        if (mana >= manaMax) {
                            MagicAttack(gameSceneManager.ChooseObjective(GameSceneManager.MAGIC_ATTACK));
                            mana = 0;
                            lastDamage = magicAttack;
                            timeToAttack = 0;
                        } else {
                            Attack(gameSceneManager.ChooseObjective(GameSceneManager.NORMAL_ATTACK));
                            lastDamage = attack;
                        }
                    }
                    
                }

            }
        }
        blocking = attacking = false;          
            
    }
    void Attack(Tuple<int, GameObject> objective)
    {
        StartCoroutine(DelayandAttack(attackRate / 4, objective.Item1, objective.Item2));
    }

    IEnumerator DelayandAttack(float seconds, int index, GameObject objective) {

        bool flipX = transform.localScale.x > 0;
        bool objective_orient = index == 0;
        if (flipX != objective_orient) {
            // No coinciden las orientaciones. Debe girar: tiempo de espera
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y);
            yield return new WaitForSeconds(seconds);
        }
        if (index == 0) {
            objective.GetComponent<Villain>().health -= attack;
            mana = Mathf.Min(mana + manaAcum, manaMax);
            lastDamage = attack;   
        } else {
            objective.GetComponent<Minion>().health -= attack;
        }
        anim.SetTrigger("attack");
        
    }

    void Damage() {
        if (lastDamage >= 0) {
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            Text d = Instantiate(damagePrefab) as Text;
            d.transform.SetParent(canvas.transform, false);
            d.text = lastDamage.ToString();
            AudioManager.Instance.PlaySoundEffect(heroAttack);
            StartCoroutine(gameSceneManager.destroyDamage(d));
        }
    }

    void DamageMagic() {
        if (lastDamageMagic >= 0) {
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            Text d = Instantiate(damagePrefab) as Text;
            d.transform.SetParent(canvas.transform, false);
            d.text = lastDamageMagic.ToString();
            AudioManager.Instance.PlaySoundEffect(heroMagic);
            StartCoroutine(gameSceneManager.destroyDamage(d));
        }
    }

    void Fury() {
        if (fury) {
            fury = false;
            attackRate = originalAttackRate;
            anim.speed = originalAnimationSpeed;
            timeToFury = 0;
            foreach (SpriteRenderer s in part_bodies) {
                s.color = Color.white;
            }
        } else {
            fury = true;
            foreach (SpriteRenderer s in part_bodies) {
                s.color = new Color(50, 0, 0);
            }
            attackRate = attackRate - attackRate * furyValue;
            anim.speed = anim.speed + anim.speed * furyValue;
        }
    }

    void Blocks() {
        anim.SetTrigger("blocks");
        int index = gameSceneManager.RandomAvailableLane();
        gameSceneManager.closeLane(index, blockingTime);
    }

    void ParticlesAttack() {
        spell.Play();
    }

    void ParticlesMagic() {
        spell2.Play();
    }

    void MagicAttack(Tuple<int, GameObject> objective)
    {
        lastDamageMagic = magicAttack;
        Debug.Log(lastDamage);
        objective.Item2.GetComponent<Villain>().health -= magicAttack;
        anim.SetTrigger("magic");
    }

    #endregion


}
