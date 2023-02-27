using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    [System.NonSerialized]
    public GameObject go = null;
    [System.NonSerialized]
    public bool isHandBallOn = false;

    [SerializeField]
    private GameObject ballPoint = null;
    [SerializeField]
    private GameObject flyingBall = null;

    private Transform flyingBallOnHand;

    private void Start()
    {
        isHandBallOn = false;
    }

    private void Update()
    {
        if (SettingManager.Instance.isSettingMenuOn == false)
        {
            flyingBallOnHand = ballPoint.transform;
            EquipBall();
            Debug.Log(isHandBallOn);
        }
    }

    void EquipBall()
    {
        if ((Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)) && Slot.Instance.text_Count.text != "0") // ���� ����
        {
            go = Instantiate(flyingBall, flyingBallOnHand.transform); // �� ����, ��ġ�� �������ִ� ��ġ. �÷��̾��� �� ��
            go.GetComponent<Rigidbody>().useGravity = false;
            isHandBallOn = true;
        }
    }
}