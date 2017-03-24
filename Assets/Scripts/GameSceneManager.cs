/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour {

    #region Variables
    #endregion
    public bool gameover;

    public GameObject hero;
    public GameObject villain;

	#region Unity Functions

	void Start () {
        gameover = false;
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

	#endregion
}
