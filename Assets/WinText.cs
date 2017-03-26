/*
* author: Daniel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions.UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour {

    #region Variables
    private string[] phrases;
    public AudioClip winSound;
	#endregion

	#region Unity Functions

	void Start () {
        phrases = new string[] { "I told you those eggs would work", "The end is near", "I swear master did wink at me once"};
        GetComponent<Text>().text = RandomUtil.NextInRange(phrases);
        AudioManager.Instance.PlaySoundEffect(winSound);
	}

	#endregion
}
