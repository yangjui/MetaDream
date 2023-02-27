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
            //mouseY = Mathf.Clamp(mouseY, -40.0f, 70.0f); // 변수 제한최소 제한최대
            //transform.localEulerAngles = new Vector3(-mouseY, 0, 0); //위아래 조정 -
        }
    }

    private void OnDisable() // 첫 시작에 마우스 위치 고정
    {
        transform.rotation = initialRotation;
    }
}