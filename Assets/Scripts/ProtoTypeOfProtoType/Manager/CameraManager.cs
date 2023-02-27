using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (SettingManager.Instance.isSettingMenuOn == false)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            transform.Rotate(Vector3.left, mouseY);

            //mouseY += Input.GetAxis("Mouse Y") * mouseYSpeed;
            //mouseY = Mathf.Clamp(mouseY, -40.0f, 70.0f); // ���� �����ּ� �����ִ�
            //transform.localEulerAngles = new Vector3(-mouseY, 0, 0); //���Ʒ� ���� -
        }
    }

    private void OnDisable() // ù ���ۿ� ���콺 ��ġ ����
    {
        transform.rotation = initialRotation;
    }
}