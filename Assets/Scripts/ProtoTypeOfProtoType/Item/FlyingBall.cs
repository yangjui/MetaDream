using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBall : MonoBehaviour
{
    [SerializeField]
    private float throwAngle = 45f;
    [SerializeField]
    private float throwForce = 10f;

    // public bool throw

    private void Update()
    {
        if (WeaponManager.Instance.isHandBallOn && Input.GetMouseButtonDown(0) && Slot.Instance.text_Count.text != "0")
        {
            WeaponManager.Instance.go.transform.parent = null; // 부모에서 해제
            WeaponManager.Instance.go.GetComponent<Rigidbody>().useGravity = true;
            // 그래비티 켜기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 target = hit.point;
                ThrowBall(target);
                WeaponManager.Instance.isHandBallOn = false; // 공 던져서 손에 없음

                Slot.Instance.SetSlotCount(-1);
                Destroy(WeaponManager.Instance.go, 3f);
            }
        }
    }

    private void ThrowBall(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;
        float height = direction.y;

        float angle = throwAngle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(angle);

        distance += height / Mathf.Tan(angle);

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * angle));

        Vector3 launchVelocity = direction.normalized * velocity;
        GetComponent<Rigidbody>().velocity = launchVelocity;
    }
}