using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCtrl2 : MonoBehaviour
{
    public GameObject Star_Effect;
    public AudioSource source;
    public AudioClip hitClip;
    public float Speed = 4f;
    [SerializeField] private Transform tr;
    float h = 0f, v = 0f;
    private string enemyTag = "ASTEROID";
    private float halfHeight = 0f;
    private float halfWidth = 0f;
    public Transform FirePos;
    public GameObject coinBullet;
    private Vector3 moveVector;
    [SerializeField] private Touch_Pad2 pad;
    void Start()
    {
        pad = GameObject.Find("Joystick_Pad").GetComponent<Touch_Pad2>();
        source = GetComponent<AudioSource>();
        tr = GetComponent<Transform>();
        halfHeight = Screen.height * 0.5f;
        halfWidth = Screen.width * 0.5f;
    }
    public void OnStickPos(Vector3 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }
    void Update()
    {
           //���� ��Ÿ�� �÷����� �ȵ���̵� �ΰ�
        if(Application.platform == RuntimePlatform.Android)
        {    //����Ͽ��� ��ġ�� �Ǿ��ٸ� ī��Ʈ���� ���� �ϰ� �ִ�.
            //if(Input.touchCount >0) //��ġ�� �Ǿ��ٸ� 
            //{
            //    ////float deltaPosX = Input.GetTouch(0).position.x-halfWidth;
            //    ////float deltaPosY = Input.GetTouch(0).position.y-halfHeight;
            //    ////   //���� ��ġ�� 
            //    ////float XPos = deltaPosX-tr.position.x;
            //    ////float YPos = deltaPosY-tr.position.y;

            //    ////tr.Translate(Speed * Time.deltaTime * XPos * 0.01f,
            //    ////    Speed * Time.deltaTime * YPos * 0.01f, 0f);
               
            //}
            JoyStickControl();

        }
          //���� ��Ÿ�� �÷�����  ������ �ü������ ����Ƽ�� ���� �ִ� ��?
        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            #region pc���� ����
            //h = Input.GetAxis("Horizontal");
            //v = Input.GetAxis("Vertical");
            //Vector2 moveDir = (h * Vector2.right) + (v * Vector2.up);
            //tr.Translate(moveDir.normalized * Speed * Time.deltaTime);
            #endregion

            JoyStickControl();

        }
        //���� ��Ÿ�� �÷����� ������ �ΰ�
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            JoyStickControl();
        }

        CameraOutLimit();
        QuitApp();
    }

    private void JoyStickControl()
    {
        if (GetComponent<Rigidbody2D>())
        {
            Vector2 speed = GetComponent<Rigidbody2D>().velocity; //���� ����
            speed.x = 4 * h;
            speed.y = 4 * v;
            GetComponent<Rigidbody2D>().velocity = speed;
        }
    }

    private void CameraOutLimit()
    {
        #region ī�޶� ���� �������� ���� �ϴ� ���� ù��°
        //if (tr.position.x >= 8.0f)
        //    tr.position = new Vector3(8.0f, tr.position.y, tr.position.z);
        //else if (tr.position.x <= -8.0f)
        //    tr.position = new Vector3(-8f, tr.position.y, tr.position.z);
        //else if (tr.position.y >= 4.0f)
        //    tr.position = new Vector3(tr.position.x, 4.0f, tr.position.z);
        //else if (tr.position.y <= -2.0f)
        //    tr.position = new Vector3(tr.position.x, -2.0f, tr.position.z);
        #endregion
        #region ī�޶� ���� �������� ���� �ϴ� ���� �ι�°
        tr.position  = new Vector3(Mathf.Clamp(tr.position.x,-8.0f,8.0f),
            Mathf.Clamp(tr.position.y ,-2.5f,4.0f), tr.position.z);

        #endregion
    }
    void QuitApp()
    {
        // ����Ͽ��� �ڷΰ��� �� ������ ������ ����ǵ��� �Ѵ�
        if(Input.GetKeyDown(KeyCode.Escape))
           Application.Quit();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {

            Destroy(other.gameObject);
            GameObject eff = Instantiate(Star_Effect,new Vector3(tr.position.x,tr.position.y,-3f),
                Quaternion.identity);
            Destroy(eff, 0.5f);
            source.PlayOneShot(hitClip, 1.0f);
            GameManager.instance.TurnOn();
        }

    }
    public void Fire()
    {
        Instantiate(coinBullet,FirePos.position, Quaternion.identity);
    }
   
    
   
}
