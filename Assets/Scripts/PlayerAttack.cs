using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [Header("ArrowSpeed")]
    [SerializeField] float defaultArrowSpeed = 0;
    [SerializeField] float maxArrowSpeed = 0;
    [SerializeField] float chargeRate = 0;
    [SerializeField] float DecayArrowSpeed = 0;
    [SerializeField] float DragonStrikeArrowSpeed = 0;
    float arrowSpeed = 0;

    [Header("Arrow Position")]
    [SerializeField] Transform arrowPos = null;
    [SerializeField] Transform pullPos = null;

    [Header("Dragon")]
    [SerializeField] GameObject dragon = null;

    [Header("Post Processing")]
    [SerializeField] Volume postProcessingVolume;
    ColorAdjustments colorAdjustments;
    FilmGrain filmGrain;

    [Header("ZA WARUDO TIME")]
    [SerializeField] Text stopTimeText;
    [SerializeField] float maxStopTime = 5f;
    float stopTime;
    Coroutine lastCoroutine = null;

    GameObject arrow = null;
    ArrowScript arrowAttack = null;

    bool canAttack = true;
    bool timeStopped = false;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiating arrow
        DrawArrow();


        //Initializing PostProcessing Values
        postProcessingVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        postProcessingVolume.profile.TryGet<FilmGrain>(out filmGrain);

        //Initializing Text
        stopTimeText.text = "Time: " + maxStopTime;
        stopTime = maxStopTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Normal Attack
        if (Input.GetKey(KeyCode.Mouse0) && arrowSpeed < maxArrowSpeed && canAttack) 
        {
            arrowSpeed += Time.deltaTime * chargeRate;
            Pull();
        }
        if(Input.GetKeyUp(KeyCode.Mouse0) && canAttack)
        {
            Attack(arrowSpeed);
        }

        //Decay Attack
        if(Input.GetKey(KeyCode.E) && canAttack)
        {
            Pull();
        }
        if(Input.GetKeyUp(KeyCode.E) && canAttack)
        {
            Attack(DecayArrowSpeed);
        }

        //DragonStrike Attack
        if(Input.GetKeyDown(KeyCode.Q) && canAttack)
        {
            StartCoroutine(DragonStrike());
        }

        //ZAWARUDO 
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!timeStopped)
            {
                ZAWARUDO();
                lastCoroutine = StartCoroutine(StopTimeCountdown());
            }
            else
            {
                ResumeTime();
                StopCoroutine(lastCoroutine);
            }
        }

        if(timeStopped)
        {
            stopTimeText.text = "Time: " + stopTime.ToString("0");
            stopTime -= stopTime * Time.unscaledDeltaTime;
        }
    }

    IEnumerator DragonStrike()
    {
        Pull();
        yield return new WaitForSecondsRealtime(0.1f);
        canAttack = false;
        StartCoroutine(FireTime());
        arrowAttack.DragonStrike(DragonStrikeArrowSpeed, dragon);
    }

    IEnumerator FireTime()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        DrawArrow();
        canAttack = true;
    }

    void DrawArrow()
    {
        arrow = ArrowPooling.Instance.GetArrow();
        arrowSpeed = defaultArrowSpeed;
        arrow.transform.SetParent(arrowPos);
        arrow.transform.position = arrowPos.position;
        arrow.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward * -1);
        arrowAttack = arrow.GetComponent<ArrowScript>();
    }

    void Pull()
    {
        arrowAttack.arrowTrail.enabled = true;
        arrow.transform.position = Vector3.Lerp(arrow.transform.position, pullPos.position, .5f);
    }

    void Attack(float attackType)
    {
        canAttack = false;
        arrowAttack.BasicAttack(attackType);

        StartCoroutine(FireTime());
    }

    //ZA WARUDO TOKI WO TOMARE
    void ZAWARUDO()
    {
        colorAdjustments.saturation.value = -100;
        filmGrain.active = true;

        Time.timeScale = 0f;
        timeStopped = true;
    }

    void ResumeTime()
    {
        colorAdjustments.saturation.value = 0;
        filmGrain.active = false;

        stopTime = 5f;
        stopTimeText.text = "Time: " + stopTime.ToString("0");

        timeStopped = false;
        Time.timeScale = 1f;
    }

    IEnumerator StopTimeCountdown()
    {
        yield return new WaitForSecondsRealtime(maxStopTime);
        ResumeTime();
    }
}
