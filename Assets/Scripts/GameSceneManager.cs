﻿/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Extensions.Tuple;
using Extensions.System.Colections;
using Extensions.UnityEngine;

public class GameSceneManager : MonoBehaviour {

    #region Variables
    #endregion
    public bool gameover;

    public GameObject hero;
    public GameObject villain;

    public GameObject minion_up;
    public GameObject minion_middle;
    public GameObject minion_down;

    public float time_to_open_up = 0;
    public float time_to_open_middle = 0;
    public float time_to_open_down = 0;

    public int furies;

	#region Unity Functions

	void Start () {
        gameover = false;
        time_to_open_up = time_to_open_middle = time_to_open_down = 0;
        furies = 0;
	}

    IEnumerator ChangeScene(string scene) {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(scene);
    }
	
	void Update () {
		if (!gameover)
        {
            if (hero.GetComponent<Hero>().health <= 0 || villain.GetComponent<Villain>().health <= 0)
            {
                gameover = true;
                if (hero.GetComponent<Hero>().health <= 0) {
                    //Pierde
                    StartCoroutine(ChangeScene("LoseScene"));
                } else {
                    //Gana
                    StartCoroutine(ChangeLevel(GameManager.Instance.nextLevel()));
                }
            } else {

                // Times Blocks
                if (time_to_open_up > 0) {
                    time_to_open_up -= Time.deltaTime;
                } else {
                    if (time_to_open_up < 0) {
                        time_to_open_up = 0;
                        // abre lane
                    }
                    
                }
                if (time_to_open_middle > 0) {
                    time_to_open_middle -= Time.deltaTime;
                } else {
                    if (time_to_open_middle < 0) {
                        time_to_open_middle = 0;
                        // abre lane
                    }

                }
                if (time_to_open_down > 0) {
                    time_to_open_down -= Time.deltaTime;
                } else {
                    if (time_to_open_down < 0) {
                        time_to_open_down = 0;
                        // abre lane
                    }

                }
            }
        }
	}


    public static int NORMAL_ATTACK = 0;
    public static int MAGIC_ATTACK = 1;
    public Tuple<int, GameObject> ChooseObjective(int attackMode)
    {
        Tuple<int, GameObject> r = new Tuple<int, GameObject>(0, villain);
        if (minion_up || minion_middle || minion_down)
        {
            
            if (attackMode==NORMAL_ATTACK)
            {
                List<Tuple<int, GameObject>> minions = AttackableMinions();
                r = minions.RandomItem<Tuple<int, GameObject>>();
            }
            else if (attackMode==MAGIC_ATTACK)
            {
                
            }
        }
        return r;
    }

    public List<Tuple<int, GameObject>> CurrentMinions()
    {
        List<Tuple<int, GameObject>> r = new List<Tuple<int, GameObject>>();
        if (minion_up)
            r.Add(new Tuple<int, GameObject>(1, minion_up));
        if (minion_middle)
            r.Add(new Tuple<int, GameObject>(2, minion_middle));
        if (minion_down)
            r.Add(new Tuple<int, GameObject>(3, minion_down));
        return r;
    }

    public List<Tuple<int, GameObject>> AttackableMinions() {
        List<Tuple<int, GameObject>> minions = CurrentMinions();
        List<Tuple<int, GameObject>> r = new List<Tuple<int, GameObject>>();
        foreach (Tuple<int, GameObject> m in minions) {
            if (m.Item2.GetComponent<Minion>().atackable) {
                r.Add(m);
            }
        }
        return r;
    }

    public List<Tuple<int, GameObject>> DecoyMinions()
    {
        List<Tuple<int, GameObject>> minions = CurrentMinions();
        List<Tuple<int, GameObject>> r = new List<Tuple<int, GameObject>>();
        foreach (Tuple<int, GameObject> m in minions)
        {
            if (m.Item2.GetComponent<Minion>().kindMinionValue == Minion.KindMinion.Decoy)
            {
                r.Add(m);
            }
        }
        return r;
    }

    public List<int> AvailableLanes() {
        List<int> r = new List<int>();
        if (minion_up == null && time_to_open_up == 0) {
            r.Add(1);
        }
        if (minion_middle == null && time_to_open_middle == 0) {
            r.Add(2);
        }
        if (minion_down == null && time_to_open_down == 0) {
            r.Add(3);
        }
        return r;
    }

    public int RandomAvailableLane() {
        return RandomUtil.NextInRange(AvailableLanes().ToArray());
    }

    public void closeLane(int index, float time) {
        if (index == 1) {
            time_to_open_up += time;
        }
        if (index == 2) {
            time_to_open_middle += time;
        }
        if (index == 3) {
            time_to_open_down += time;
        }
    }

    public IEnumerator destroyDamage(Text t) {
        yield return new WaitForSeconds(0.5f);
        Destroy(t.gameObject);
    }

    public IEnumerator ChangeLevel(int nextLevel) {
        yield return new WaitForSeconds(3);
        if (nextLevel == 4) {
            SceneManager.LoadScene("WinScene");
        } else {
            GameManager.Instance.Level++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

	#endregion
}
