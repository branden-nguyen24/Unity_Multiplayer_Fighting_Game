﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public int playerHealth;
    //public int winCount;
    public float stunTime = .5f;
    public float pushBack = 200f;
    public string currentState = "idle";
    public float currentPosition;
    public int attackStat;
    public int defenseStat;
    public string characterName;
    public string playerTag;

    private Rigidbody rb;
    private Animator anim;
    private AnimatorStateInfo currentBaseState;

    GameObject opponent;
    public Player_Manager opponentManager;

    public Attack attackRequest;
    


    [FMODUnity.EventRef]
    public string PlayerStateEvent = "";

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 1000;
        playerTag = gameObject.tag;
        Player_Stats playerStats = gameObject.GetComponent<Player_Stats>();
        characterName = gameObject.name;
        Debug.Log(characterName);
        attackStat = playerStats.getPlayerAttack(characterName);
        Debug.Log(attackStat);
        defenseStat = playerStats.getPlayerDefense(characterName);
        Debug.Log(defenseStat);

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        attackRequest = gameObject.GetComponent<Attack>();
        

        if (playerTag == "Player1")
        {
            opponent = GameObject.FindWithTag("Player2");
        }
        else if (playerTag == "Player2")
        {
            opponent = GameObject.FindWithTag("Player1");
        }

        opponentManager = opponent.GetComponent<Player_Manager>();
        Debug.Log(opponentManager.playerTag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetUp")) {
            return;
        }

        rb.AddForce(new Vector3(pushBack, 0f, 0f) * -50);
        anim.SetBool("Stun", true);
        playerHealth = (playerHealth - damage /*+ defenseStat*/);
        Debug.Log("took damage from opponent");
        Debug.Log(playerHealth + playerTag);
        Invoke("stopStun", stunTime);
    }

    public void GiveDamage(int damage)
    {
        opponentManager.TakeDamage(damage + attackStat);
        Debug.Log("gave damage to opponent");
        Debug.Log(playerTag);
        //Network
        attackRequest.sendAttackRequest(attackStat, damage);
    }

    void EndGame()
    {
        if (playerHealth <= 0) { 
        //endgame
        }
    }

    void stopStun() {
        anim.SetBool("Stun", false);
    }
}
