using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Slider HP;
    [SerializeField]
    private Slider Stamina;

    [Header("PlayerControl")]
    [SerializeField] // 강제 직렬화, 인스펙터창에서 값을 변경할 수 있다.
    private float walkSpeed = 10f; // 걷기 속도
    [SerializeField]
    private float RunSpeed = 20f; // 뛰는 속도
    [SerializeField]
    private float jumpPower = 6f; // 점프할 때 힘

    [SerializeField]
    private float lookSensitivity = 5f; // 마우스 감도

    [SerializeField]
    private float cameraRotationLimit = 75f; // Y 회전축 각도 제한
    private float currentCameraRotationX; // 현재 카메라의 X 회전축 값

    public static bool CamRotateEnable = true;
    [SerializeField]
    private Camera theCamera; // 카메라
    [SerializeField]
    public Rigidbody myRigid; // 리지드바디

    private bool isJumping = false; // 현재 점프 중인가?


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();                 // 키보드 입력에 따라 이동
        Jump();                 // 점프
        if (CamRotateEnable == true)
        {
            CameraRotation();       // 마우스를 위아래(Y) 움직임에 따라 카메라 X 축 회전 
            CharacterRotation();    // 마우스 좌우(X) 움직임에 따라 캐릭터 Y 축 회전 
        }
        MouseLocked();          // 마우스 커서를 안보이게 하고 중간 고정
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // A/D 입력
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // W/S 입력
        Vector3 _moveHorizontal = transform.right * _moveDirX; // 좌우 이동 벡터
        Vector3 _moveVertical = transform.forward * _moveDirZ; // 상하 이동 벡터

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;
        // normalized: 대각선으로 가도 2가 아닌 1로 만들어줌 (대각선으로 가는 속도 = 일자로 가는 속도)
        // transform.right(Vector3(1,0,0)) + transform.forward(Vector3(0,0,1))
        // 그냥 더하면 Vector3(1,0,1) / normalized하면 Vector3(0.5f, 0, 0.5f)

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        
        // deltaTime -> 1.0f ÷ (기기의 현재 1초당 프레임 수)
        // 프레임 높은 기기에서 더 빨리 이동하는 현상 / 프레임 낮은 기기에서 더 느리게 이동하는 현상을 막아준다

        //달리기
        if (Input.GetKey(KeyCode.LeftShift) && Stamina.value > 0)
        {
            Stamina.value -= 0.2f * Time.deltaTime;
            Vector3 _Runvelocity = (_moveHorizontal + _moveVertical).normalized * RunSpeed;
            myRigid.MovePosition(transform.position + _Runvelocity * Time.deltaTime);
        }
        if(!Input.GetKey(KeyCode.LeftShift) && Stamina.value < 1 && !isJumping)
        {
            Invoke("StaminaUP", 3f);
        }
    }

    private void Jump()
    {
        // 스페이스바를 눌렀으며 현재 땅에 있는가?
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && Stamina.value > 0)
        {
            Stamina.value -= 0.1f;
            myRigid.AddForce(0, jumpPower, 0, ForceMode.Impulse); // 리지드바디에 위로 힘을 가함
            isJumping = true; // 점프 중임
        }
    }

    public void CameraRotation() // 카메라의 위아래 회전
    {
       
            float _xRotation = Input.GetAxisRaw("Mouse Y"); // 마우스 위아래
            float _cameraRotationX = _xRotation * lookSensitivity;

            currentCameraRotationX -= _cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            // Mathf.Clamp: (값, 최솟값, 최댓값) -> 첫번째 값이 최솟값보다 작다면 최솟값을,
            // 최댓값보다 크다면 최댓값을, 아니라면 첫번째 값 반환

           theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    
    }

    public void CharacterRotation()  // 좌우 캐릭터 회전
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); // 쿼터니언 * 쿼터니언
        // Debug.Log(myRigid.rotation);  // 쿼터니언
        // Debug.Log(myRigid.rotation.eulerAngles); // 벡터
    }

    void MouseLocked()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void StaminaUP()
    {
        Stamina.value += 0.1f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) // 콜리전 충돌 시작
    {
        if (collision.gameObject.CompareTag("Floor")) // 바닥과 닿았는가? (*바닥에 "Floor" 태그를 붙여야 함)
            isJumping = false; // 점프 중이 아님
    }
}