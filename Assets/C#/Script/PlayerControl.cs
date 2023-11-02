using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] // �����ڰ� ���� �Է��ϴ� ���� ����
    private float walkSpeed = 10f; // �ȱ� �ӵ�
    [SerializeField]
    private float jumpPower = 6f; // ������ �� ��
    //[SerializeField]
    //private float RunSpeed = 20.0f; // �� �� ��

    [Header("Sensitivity")]
    [SerializeField]
    private float lookSensitivity = 5f; // ���콺 ����

    [Header("CameraRotation")]
    [SerializeField]
    private float cameraRotationLimit = 75f; // Y ȸ���� ���� ����
    private float currentCameraRotationX; // ���� ī�޶��� X ȸ���� ��

    [Header("Camera/Rigidbody")]
    [SerializeField]
    private Camera theCamera; // ī�޶�
    [SerializeField]
    public Rigidbody myRigid; // ������ٵ�

    [Header("Sound")]
    [SerializeField]
    private AudioSource jumpSound;
    [SerializeField]
    private AudioSource walkSound;

/*    [Header("UI")]
    [SerializeField]
    private Image Stamina;*/

    private bool isJumping = false; // ���� ���� ���ΰ�?
    private bool isMoving = false; // ���� �����̰� �ִ°� Ȯ���ϴ� �ڵ�.

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Ŀ�� ����
    }

    void Update()
    {
		CameraRotation();       // ���콺�� ���Ʒ�(Y) �����ӿ� ���� ī�޶� X �� ȸ�� 
		CharacterRotation();    // ���콺 �¿�(X) �����ӿ� ���� ĳ���� Y �� ȸ�� 
        CursorOnOff();
    }

	private void FixedUpdate()
	{
		Move();                 // Ű���� �Է¿� ���� �̵�
		Jump();                 // ����
		
	}


	private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // A/D �Է�
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // W/S �Է�

        // �Ʒ� �ڵ�� ���� �� ���� �ȴ� �Ҹ��� ����ϴ� �ڵ��Դϴ�.
        // ������ false�϶��� �ڵ尡 �۵��ϵ��� �Ͽ����ϴ�.
        if ((_moveDirX != 0 && isJumping == false )|| (_moveDirZ != 0 && isJumping == false))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if(isMoving == true)
        {
            if (!walkSound.isPlaying)
            walkSound.Play();
        }
        else
        {
            walkSound.Stop();
        }
        
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
     /*   if (Input.GetKey(KeyCode.LeftShift) && Stamina.fillAmount > 0)
        {
            Stamina.fillAmount -= 0.2f * Time.deltaTime;
            Vector3 _Runvelocity = (_moveHorizontal + _moveVertical).normalized * RunSpeed;
            myRigid.MovePosition(transform.position + _Runvelocity * Time.deltaTime);
        }
        if (!Input.GetKey(KeyCode.LeftShift) && Stamina.fillAmount < 1)
        {
            Invoke("StaminaUP", 3f);
        }*/


    }

    private void Jump()
    {
        // �����̽��ٸ� �������� ���� ���� �ִ°�?
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping /*&& Stamina.fillAmount >= 0*/)
        {
            //Stamina.fillAmount -= 0.1f;
            myRigid.AddForce(0, jumpPower, 0, ForceMode.Impulse); // ������ٵ� ���� ���� ����
            isJumping = true; // ���� ����
        }
    }

    private void CameraRotation() // ī�޶��� ���Ʒ� ȸ��
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y"); // ���콺 ���Ʒ�
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        // Mathf.Clamp: (��, �ּڰ�, �ִ�) -> ù��° ���� �ּڰ����� �۴ٸ� �ּڰ���,
        // �ִ񰪺��� ũ�ٸ� �ִ���, �ƴ϶�� ù��° �� ��ȯ

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()  // �¿� ĳ���� ȸ��
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); // ���ʹϾ� * ���ʹϾ�
        // Debug.Log(myRigid.rotation);  // ���ʹϾ�
        // Debug.Log(myRigid.rotation.eulerAngles); // ����
    }

    private void OnCollisionEnter(Collision collision) // �ݸ��� �浹 ����
    {
        if (collision.gameObject.CompareTag("Floor"))// �ٴڰ� ��Ҵ°�? (*�ٴڿ� "Floor" �±׸� �ٿ��� ��)
        {
            isJumping = false; // ���� ���� �ƴ�
            jumpSound.Play();
        }
    }

    public void CursorOnOff()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

   /* void StaminaUP()
    {
        Stamina.fillAmount += 0.1f * Time.deltaTime;
    }*/
}
