/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseText : MonoBehaviour {

    public AudioClip crySound;

	#region Unity Functions

	void Start () {
        AudioManager.Instance.PlaySoundEffect(crySound);
	}

	#endregion
}
