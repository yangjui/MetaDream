using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : Singleton<Chaser>
{
    private NavMeshAgent navAgent;
    private Vector3 lastPostion;

    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private float moveSpeed = 5f;

    [Space(10f)]
    [Header("Ray")]

    private Ray ray;
    private float rayHitTime = 0;

    [SerializeField]
    private float maxRay = 5f;
    [SerializeField]
    private float rayHitTimeForStopMove = 3f;

    [Space(10f)]
    [Header("EyeBeam")]

    [SerializeField]
    private float eyeBeamSpeedDown = 0.8f;
    [SerializeField]
    private float eyeBeamCoolForSetting = 90f;
    [SerializeField]
    private float eyeBeamRangeMin = 8f;

    private float playerNomalWalkSpeed = 0f;
    private float playerNomalRunSpeed = 0f;
    private float eyeBeamCoolForCode = 0f;
    private bool isHitEyeBeam = false;

    private Animator walkAnimation;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        GetComponent<NavMeshAgent>().speed = moveSpeed;
        // walkAnimation = GetComponent<Animator>();
        // walkAnimation.SetBool("isWalk", false) ;

        playerNomalWalkSpeed = player.GetComponent<Player>().walkSpeed;
        playerNomalRunSpeed = player.GetComponent<Player>().runSpeed;
    }

    private void Update()
    {
        ChasePlayerWithSound();
        ChasePlayerWithRay();
        SearchAround();
        UVLightHit();

        // Debug.Log(navAgent.velocity.magnitude);

        //if (Mathf.Abs(navAgent.velocity.magnitude) > 0.2f)
        //{
        //    walkAnimation.SetBool("isWalk", true);
        //}
        //else if(Mathf.Abs(navAgent.velocity.magnitude) == 0)
        //{
        //    walkAnimation.SetBool("isWalk", false);
        //}
    }

    private void UVLightHit()
    {
        if (player.GetComponent<Player>().rayString == transform.name && FlashLightManager.Instance.uvLightOn)
        {
            rayHitTime += Time.deltaTime;

            if (rayHitTime >= rayHitTimeForStopMove)
            {
                navAgent.isStopped = true;
                Invoke("MoveAgain", 3f);
            }
        }
    }

    private void OnCollisionEnter(Collision _other)
    {
        if (_other.collider.CompareTag("FloorBall"))
        {
            navAgent.isStopped = true;
            Invoke("MoveAgain", 5f);
        }
    }

    private void MoveAgain()
    {
        rayHitTime = 0;
        navAgent.isStopped = false;
    }

    private void ChasePlayerWithSound()
    {
        if (player.GetComponent<Player>().isSoundOn)
        {
            navAgent.SetDestination(player.transform.position);
            lastPostion = player.transform.position;
        }
    }

    private void ChasePlayerWithRay()
    {
        float rayLength = 30f;
        float raySpreadAngle = 10f;
        Vector3 rayDirection = transform.forward;
        Vector3 rayEndPoint = transform.position + rayDirection * rayLength;
        Vector3 raySpreadVector = Random.Range(-raySpreadAngle, raySpreadAngle) * transform.right + Random.Range(-raySpreadAngle, raySpreadAngle) * transform.up;
        Vector3 raySpreadEndPoint = rayEndPoint + raySpreadVector;
        Debug.DrawLine(transform.position, raySpreadEndPoint, Color.green);

        ray = new Ray(transform.position, rayDirection);

        if (Physics.Raycast(ray, out RaycastHit rayHit))
        {
            //Debug.Log(rayHit.transform.name);
            if (rayHit.transform.name == "Player")
            {
                transform.LookAt(player.transform.position);
                navAgent.SetDestination(player.transform.position);

                if (Vector3.Distance(transform.position, player.transform.position) > eyeBeamRangeMin && eyeBeamCoolForCode == 0 && !isHitEyeBeam)
                {
                    isHitEyeBeam = true;
                    player.GetComponent<Player>().walkSpeed = player.GetComponent<Player>().walkSpeed * eyeBeamSpeedDown;
                    player.GetComponent<Player>().runSpeed = player.GetComponent<Player>().runSpeed * eyeBeamSpeedDown;
                }
            }
        }

        if (eyeBeamCoolForCode >= eyeBeamCoolForSetting)
        {
            eyeBeamCoolForCode = 0;
            isHitEyeBeam = false;
        }

        if (eyeBeamCoolForCode != 0)
        {
            ReturnNomalPlayerSpeed();
        }

        if (isHitEyeBeam)
        {
            eyeBeamCoolForCode += Time.deltaTime;
        }
    }

    private void ReturnNomalPlayerSpeed()
    {
        if (eyeBeamCoolForCode >= 10)
        {
            player.GetComponent<Player>().walkSpeed = playerNomalWalkSpeed;
            player.GetComponent<Player>().runSpeed = playerNomalRunSpeed;
        }
    }

    private void SearchAround()
    {
        if (Vector3.Distance(transform.position, lastPostion) > 1f)
        {
            transform.Rotate(0f, 30f * Time.deltaTime, 0f);
        }
    }
}