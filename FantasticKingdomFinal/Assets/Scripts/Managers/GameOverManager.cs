using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour {

    public GoldPile goldPile;
    public Text gameStats;

    float totalTime = 0;
    int totalKills = 0;
    int maxGold = 0;
    bool END = false;
    Animator anim;
    

	void Awake () {
        anim = GetComponent<Animator>();
	}

    void FixedUpdate()
    {
        totalTime += Time.deltaTime;
    }
	
	// Update is called once per frame
	void Update () {
	
        if(goldPile)
            if(goldPile.getGold() == 0 && !END)
            {
                GameOver();
            }

	}

    void GameOver()
    {
        END = true;
        maxGold = goldPile.getMaxGold();
        totalKills = goldPile.getKills();
        gameStats.text = "You lasted " + (int)totalTime + " seconds \n Kills: " + totalKills + "\n Max Gold: " + maxGold;
        if (anim)
            anim.SetTrigger("GameOver");
    }


}
