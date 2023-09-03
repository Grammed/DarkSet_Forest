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
    [SerializeField] // ���� ����ȭ, �ν�����â���� ���� ������ �� �ִ�.
    private float walkSpeed = 10f; // �ȱ� �ӵ�
    [SerializeField]
    private float RunSpeed = 20f; // �ٴ� �ӵ�
    [SerializeField]
    private float jumpPower = 6f; // ������ �� ��

    [SerializeField]
    private float lookSensitivity = 5f; // ���콺 ����

    [SerializeField]
    private float cameraRotationLimit = 75f; // Y ȸ���� ���� ����
    private float currentCameraRotationX; // ���� ī�޶��� X ȸ���� ��

    public static bool CamRotateEnable = true;
    [SerializeField]
    private Camera theCamera; // ī�޶�
    [SerializeField]
    public Rigidbody myRigid; // ������ٵ�

    private bool isJumping = false; // ���� ���� ���ΰ�?


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();                 // Ű���� �Է¿� ���� �̵�
        Jump();                 // ����
        if (CamRotateEnable == true)
        {
            CameraRotation();       // ���콺�� ���Ʒ�(Y) �����ӿ� ���� ī�޶� X �� ȸ�� 
            CharacterRotation();    // ���콺 �¿�(X) �����ӿ� ���� ĳ���� Y �� ȸ�� 
        }
        MouseLocked();          // ���콺 Ŀ���� �Ⱥ��̰� �ϰ� �߰� ����
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // A/D �Է�
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // W/S �Է�
        Vector3 _moveHorizontal = transform.right * _moveDirX; // �¿� �̵� ����
        Vector3 _moveVertical = transform.forward * _moveDirZ; // ���� �̵� ����

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;
        // normalized: �밢������ ���� 2�� �ƴ� 1�� ������� (�밢������ ���� �ӵ� = ���ڷ� ���� �ӵ�)
        // transform.right(Vector3(1,0,0)) + transform.forward(Vector3(0,0,1))
        // �׳� ���ϸ� Vector3(1,0,1) / normalized�ϸ� Vector3(0.5f, 0, 0.5f)

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        
        // deltaTime -> 1.0f �� (����� ���� 1�ʴ� ������ ��)
        // ������ ���� ��⿡�� �� ���� �̵��ϴ� ���� / ������ ���� ��⿡�� �� ������ �̵��ϴ� ������ �����ش�

        //�޸���
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
        // �����̽��ٸ� �������� ���� ���� �ִ°�?
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && Stamina.value > 0)
        {
            Stamina.value -= 0.1f;
            myRigid.AddForce(0, jumpPower, 0, ForceMode.Impulse); // ������ٵ� ���� ���� ����
            isJumping = true; // ���� ����
        }
    }

    public void CameraRotation() // ī�޶��� ���Ʒ� ȸ��
    {
       
            float _xRotation = Input.GetAxisRaw("Mouse Y"); // ���콺 ���Ʒ�
            float _cameraRotationX = _xRotation * lookSensitivity;

            currentCameraRotationX -= _cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            // Mathf.Clamp: (��, �ּڰ�, �ִ�) -> ù��° ���� �ּڰ����� �۴ٸ� �ּڰ���,
            // �ִ񰪺��� ũ�ٸ� �ִ���, �ƴ϶�� ù��° �� ��ȯ

           theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    
    }

    public void CharacterRotation()  // �¿� ĳ���� ȸ��
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); // ���ʹϾ� * ���ʹϾ�
        // Debug.Log(myRigid.rotation);  // ���ʹϾ�
        // Debug.Log(myRigid.rotation.eulerAngles); // ����
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

    private void OnCollisionEnter(Collision collision) // �ݸ��� �浹 ����
    {
        if (collision.gameObject.CompareTag("Floor")) // �ٴڰ� ��Ҵ°�? (*�ٴڿ� "Floor" �±׸� �ٿ��� ��)
            isJumping = false; // ���� ���� �ƴ�
    }
}