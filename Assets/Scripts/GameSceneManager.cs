/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions.Tuple;
using Extensions.System.Colections;

public class GameSceneManager : MonoBehaviour {

    #region Variables
    #endregion
    public bool gameover;

    public GameObject hero;
    public GameObject villain;

    public GameObject minion_up;
    public GameObject minion_middle;
    public GameObject minion_down;

    public int furies;

	#region Unity Functions

	void Start () {
        gameover = false;
        furies = 0;
	}
	
	void Update () {
		if (!gameover)
        {
            if (hero.GetComponent<Hero>().health <= 0 || villain.GetComponent<Villain>().health <= 0)
            {
                gameover = true;
                Debug.Log("GAME OVER");
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
                List<Tuple<int, GameObject>> minions = CurrentMinions();
                r = minions.RandomItem<Tuple<int, GameObject>>();
            }
            else if (attackMode==MAGIC_ATTACK)
            {
                List<Tuple<int, GameObject>> decoys = MagicMinions();
                r = decoys.RandomItem<Tuple<int, GameObject>>();
            }
        }
        return r;
    }

    List<Tuple<int, GameObject>> CurrentMinions()
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

    List<Tuple<int, GameObject>> MagicMinions()
    {
        List<Tuple<int, GameObject>> minions = CurrentMinions();
        List<Tuple<int, GameObject>> r = new List<Tuple<int, GameObject>>();
        foreach (Tuple<int, GameObject> m in r)
        {
            /*if (m.tipo == Tipo.DECOY)
            {
                r.Add(m);
            }*/
        }
        return r;
    }

	#endregion
}
