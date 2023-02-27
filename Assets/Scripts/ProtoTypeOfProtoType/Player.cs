using UnityEngine;

[RequireComponent(typeof(CharacterController))] // ������Ʈ �ڵ� �߰�!

public class Player : Singleton<Player>
{
    [Header("Speed")]

    [SerializeField]
    private float normalSpeed = 5.0f;  // �⺻ ���ǵ�
    [SerializeField]
    public float walkSpeed = 5.0f;  // �ȱ�
    [SerializeField]
    public float runSpeed = 10.0f; // �޸���
    [SerializeField]
    private float jump = 10.0f; // ����
    [SerializeField]
    private float gravity = 20.0f;

    private float runCoolTime = 0f;
    private float canRumTime = 0f;


    [Space(10f)]

    private CharacterController player = null; // ���� ĳ���Ͱ� �������ִ� ĳ���� ��Ʈ�ѷ�
    private Vector3 MoveDir = Vector3.zero; // ĳ������ �����̴� ����.
    private Animator anim;
    private float mouseX = 0.0f;
    private float mouseXSpeed = 3.0f;


    [SerializeField]
    private GameObject enemy = null;


    public bool isSoundOn = false;
    [SerializeField]
    private GameObject foot = null;


    [Space(10f)]
    [Header("Ray")]

    [SerializeField]
    private Camera playerCamera = null;
    private Ray ray;
    private RaycastHit rayHit;
    [SerializeField]
    private float maxRay = 5f;
    [System.NonSerialized]
    public string rayString = null;

    //[SerializeField]
    //[Range(0.01f, 0.1f)] float shakeRange = 0.05f;
    //[SerializeField]
    //[Range(0.1f, 1f)] float duration = 0.5f;

    // private float runTimeForSec = 0f;


    private void Awake()
    {
        player = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 ��ġ ���� �� Ŀ�� ��Ȱ��ȭ
        Cursor.visible = false;
    }

    private void Update()
    {
        isSoundOn = false;

        if (!isSoundOn)
            foot.SetActive(false);

        if (isSoundOn)
            foot.SetActive(true);

        if (SettingManager.Instance.isSettingMenuOn == false)
        {
            PlayerRay();
            PlayerMove();
            PlayerRotate();
        }

        if (runCoolTime > 0f) // ������Ʈ
        {
            RunCoolTime();
        }
    }

    private void PlayerMove()
    {
        if (player.isGrounded)
        {
            // ��, �Ʒ� ������ ����. 
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // ���͸� ���� ��ǥ�� ���ؿ��� ���� ��ǥ�� �������� ��ȯ
            MoveDir = transform.TransformDirection(MoveDir.normalized);

            if (Input.GetKey(KeyCode.LeftShift) && runCoolTime == 0) // ����ƮŰ �޸���
            {
                normalSpeed = runSpeed;
                Stoprun();
            }

            else
            {
                normalSpeed = walkSpeed;
            }

            MoveDir *= normalSpeed;

            // ĳ���� ����
            if (Input.GetButton("Jump"))
            {
                MoveDir.y = jump;
            }

            if (MoveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftControl)) // �ɱ� - �Ҹ� �ȳ�
            {
                if (player.isGrounded)
                {
                    isSoundOn = true;
                }
            }
        }
        MoveDir.y -= gravity * Time.deltaTime;
        // ĳ���� ������.
        player.Move(MoveDir * Time.deltaTime);
    }

    private void Stoprun()
    {
        canRumTime += Time.deltaTime;

        if (canRumTime >= 5f)
        {
            runCoolTime = 10f;
        }
    }

    private void RunCoolTime()
    {
        runCoolTime -= Time.deltaTime;
        if (runCoolTime <= 0)
        {
            runCoolTime = 0;
            canRumTime = 0;
        }
    }

    private void PlayerRotate()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseXSpeed;
        transform.eulerAngles = new Vector3(0, mouseX, 0);
    }

    private void AnimationMoving() // �ִϸ��̼� ȿ�� - ���� �� ���� - ���� ���߿� �ٲ߽ô�
    {
        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            anim.SetInteger("IsWalkingMotion", 1);
        }

        // ���� �ִϸ��̼� ����
        // ��ư ������ �Ķ���Ͱ� ���������� 2�� ��
        // �ٽ� else�� -> �Ķ���� 0 : ����, �������� ����
        else if (Input.GetButtonDown("Jump"))
        {
            anim.SetInteger("JumpUpMotion", 2);
        }

        else
        {
            anim.SetInteger("JumpDownMotion", 0);
        }
    }

    private void PlayerRay()
    {
        ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out rayHit, maxRay))
        {
            Debug.DrawLine(ray.origin, rayHit.point, Color.green);
            rayString = rayHit.transform.name;
            Debug.Log(rayString);
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.direction * 100, Color.red);
            rayString = null;
        }
    }

    //private void OnTriggerEnter(Collider _other) // ���ʹ� �����̵�
    //{

    //    if (_other.CompareTag("DoSomething") && Vector3.Distance(transform.position, enemy.transform.position) >= 20f)
    //    {
    //        enemy.transform.position = transform.position + new Vector3(Random.Range(-20, 20), transform.position.y, Random.Range(-20, 20));
    //    }
    //}
}