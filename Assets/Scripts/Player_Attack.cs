﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private CapsuleCollider col;
    private Rigidbody rb;
    private float orgColHight;
    private Vector3 orgVectColCenter;
    private Animator anim;
    private AnimatorStateInfo currentBaseState;
    public string characterName;
    public string playerTag;
    public Player_Manager playerManager;
    public GameObject[] hitboxes;
    public Collider_Behavior colliderBehavior;
    public bool checker;

    Dictionary<string, int> attackValueTable = new Dictionary<string, int>();

    [FMODUnity.EventRef]
    public string PlayerStateEvent = "";
    FMOD.Studio.EventInstance playerState;

    [FMODUnity.EventRef]
    FMOD.Studio.EventInstance attack;

    [FMODUnity.EventRef]
    public string PreAttackEvent = "";
    [FMODUnity.EventRef]
    public string AttackEvent = "";
    
    void Start()
    {
        playerManager = gameObject.GetComponent<Player_Manager>();
        colliderBehavior = gameObject.GetComponent<Collider_Behavior>();

        playerState = FMODUnity.RuntimeManager.CreateInstance(PlayerStateEvent);
        playerState.start();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        orgColHight = col.height;
        orgVectColCenter = col.center;
        attack = FMODUnity.RuntimeManager.CreateInstance(AttackEvent);
        foreach (GameObject box in hitboxes)
        {
            box.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            launchAttack("LightPunch");
            StartCoroutine(StartAttack(1.40f, hitboxes[0]));
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            launchAttack("HeavyPunch");
            StartCoroutine(StartAttack(0.70f, hitboxes[0]));
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            launchAttack("LightKick");
            StartCoroutine(StartAttack(0.70f, hitboxes[1]));
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            launchAttack("HeavyKick");
            StartCoroutine(StartAttack(1.05f, hitboxes[2]));
        }
    }
    // co routine to activate the hitboxes only for the duration of the attack.
    IEnumerator StartAttack(float attacktime, GameObject hit)
    {
        hit.SetActive(true);
        yield return new WaitForSeconds(attacktime);
        hit.SetActive(false);
    }


    void initAttackValueTable()
    {
        //attackValueTable.Add("animation", 0);
        attackValueTable.Add("LightPunch", 2);
        attackValueTable.Add("LightKick", 2);
        attackValueTable.Add("HeavyPunch", 5);
        attackValueTable.Add("HeavyKick", 5);
    }

    public int getAttackValue(string attackName)
    {
        if (!attackValueTable.ContainsKey(attackName))
        {
            return 0;
        }
        return attackValueTable[attackName];
    }

    public void launchAttack(string attackName)
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Debug.Log("In attack mode, attack not counted");
            return;

        }
        else {
            anim.SetTrigger(attackName);
            attackSound(attackName);
            int damage = getAttackValue(attackName);
            //playerManager.GiveDamage(damage);
            //colliderBehavior.Hello();
        }

    }

    void attackSound(string attackName)
    {
        //if (!IsPlaying(attack)){
            //FMODUnity.RuntimeManager.PlayOneShot(AttackEvent, transform.position);
            attack.start();
            attack.release();
       // }
        // heal.setParameterByID(fullHealthParameterId, restoreAll ? 1.0f : 0.0f);
        //heal.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }

    bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }
}