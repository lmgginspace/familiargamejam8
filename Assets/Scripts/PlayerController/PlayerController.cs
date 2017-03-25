using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public List<Card> cardsList;

    public Card cardSelected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
         
        switch (Input.inputString)
        {
            case "q":
                selectCard(0);
                break;
            case "w":
                selectCard(1);
                break;
            case "e":
                selectCard(2);
                break;
            case "r":
                selectCard(3);
                break;
            case "t":
                selectCard(4);
                break;
        }
	}

    public void selectCard(int iCard)
    {
        for (int i = 0; i < cardsList.Count; i++)
        {

            if (i != iCard)
            {
                cardsList[i].select = false;
            }
            else
            {
                cardsList[i].select = true;

                cardSelected = cardsList[i];
            }
        }
    }

    public void spawnP(GameObject pos)
    {
        string posName = pos.name;

        if (cardSelected)
        {
            cardSelected.spawnMinion(pos,posName);

        }
    }

}
