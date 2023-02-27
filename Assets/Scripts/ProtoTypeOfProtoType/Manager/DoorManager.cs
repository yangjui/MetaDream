//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DoorManager : Singleton<DoorManager>
//{
//    [SerializeField]
//    private Animator animDoor = null;
//    [SerializeField]
//    private GameObject openText = null;

//    private bool isCanOpen = false;

//    private void Awake()
//    {
//        openText.SetActive(false);
//    }

//    private void Update()
//    {
//        PlayerRayForDoor();

//        if (isCanOpen)
//        {
//            DoorOpen();
//        }
//    }

//    private void DoorOpen()
//    {
//        if (Input.GetKey(KeyCode.X))
//        {
//            animDoor.SetBool("isOpen", true);
//        }
//        else
//        {
//            animDoor.SetBool("isOpen", false);
//        }
//    }

//    private void PlayerRayForDoor()
//    {
//        if (Player.Instance.rayString == "DoorAnimate")
//        {
//            openText.SetActive(true);
//            isCanOpen = true;
//        }

//        else
//        {
//            openText.SetActive(false);
//            isCanOpen = false;
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : Singleton<DoorManager>
{
    [SerializeField]
    private GameObject openText = null;

    private bool isCanOpen = false;

    [SerializeField]
    private List<GameObject> doorList = new List<GameObject>();
    private GameObject currentDoor = null;

    private void Awake()
    {
        openText.SetActive(false);

        foreach (var door in doorList)
        {
            Animator animator = door.GetComponent<Animator>();

            if (animator == null)
            {
                animator = door.AddComponent<Animator>();
            }
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Door");
        }
    }

    private void Update()
    {
        PlayerRayForDoor();

        if (isCanOpen && currentDoor != null)
        {
            DoorOpen();
        }
    }

    private void DoorOpen()
    {
        if (Input.GetKey(KeyCode.X))
        {
            currentDoor.GetComponent<Animator>().SetBool("isOpen", true);
        }

        else
        {
            currentDoor.GetComponent<Animator>().SetBool("isOpen", false);
        }
    }

    private void PlayerRayForDoor()
    {
        for (int i = 0; i < doorList.Count; ++i)
        {
            if (Player.Instance.rayString == doorList[i].name)
            {
                openText.SetActive(true);
                isCanOpen = true;

                currentDoor = doorList[i];
            }
        }

        if (!isCanOpen)
        {
            openText.SetActive(false);
            currentDoor = null;
        }
    }
}