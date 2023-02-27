using UnityEngine;

[RequireComponent(typeof(CharacterController))] // 컴포넌트 자동 추가!

public class Player : Singleton<Player>
{
    [Header("Speed")]

    [SerializeField]
    private float normalSpeed = 5.0f;  // 기본 스피드
    [SerializeField]
    public float walkSpeed = 5.0f;  // 걷기
    [SerializeField]
    public float runSpeed = 10.0f; // 달리기
    [SerializeField]
    private float jump = 10.0f; // 점프
    [SerializeField]
    private float gravity = 20.0f;

    private float runCoolTime = 0f;
    private float canRumTime = 0f;


    [Space(10f)]

    private CharacterController player = null; // 현재 캐릭터가 가지고있는 캐릭터 컨트롤러
    private Vector3 MoveDir = Vector3.zero; // 캐릭터의 움직이는 방향.
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
        Cursor.lockState = CursorLockMode.Locked; // 마우스 위치 고정 및 커서 비활성화
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

        if (runCoolTime > 0f) // 업데이트
        {
            RunCoolTime();
        }
    }

    private void PlayerMove()
    {
        if (player.isGrounded)
        {
            // 위, 아래 움직임 셋팅. 
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환
            MoveDir = transform.TransformDirection(MoveDir.normalized);

            if (Input.GetKey(KeyCode.LeftShift) && runCoolTime == 0) // 시프트키 달리기
            {
                normalSpeed = runSpeed;
                Stoprun();
            }

            else
            {
                normalSpeed = walkSpeed;
            }

            MoveDir *= normalSpeed;

            // 캐릭터 점프
            if (Input.GetButton("Jump"))
            {
                MoveDir.y = jump;
            }

            if (MoveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftControl)) // 앉기 - 소리 안남
            {
                if (player.isGrounded)
                {
                    isSoundOn = true;
                }
            }
        }
        MoveDir.y -= gravity * Time.deltaTime;
        // 캐릭터 움직임.
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

    private void AnimationMoving() // 애니메이션 효과 - 현재 미 구현 - 변수 나중에 바꿉시당
    {
        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            anim.SetInteger("IsWalkingMotion", 1);
        }

        // 점프 애니메이션 시작
        // 버튼 눌릴때 파라미터가 순간적으로 2가 됨
        // 다시 else문 -> 파라미터 0 : 공중, 착지까지 연속
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

    //private void OnTriggerEnter(Collider _other) // 에너미 순간이동
    //{

    //    if (_other.CompareTag("DoSomething") && Vector3.Distance(transform.position, enemy.transform.position) >= 20f)
    //    {
    //        enemy.transform.position = transform.position + new Vector3(Random.Range(-20, 20), transform.position.y, Random.Range(-20, 20));
    //    }
    //}
}