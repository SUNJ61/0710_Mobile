using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCtrl : MonoBehaviour
{
    /* �ʿ��� ���
    1. ������ ������Ʈ ��ġ ��.
    2. ������ ���ǵ�.
    3. ������ �ִ� ��ġ.
    */
    public Transform tr;

    private float Speed = 4f;
    private float h = 0f, v = 0f; //������ x,y��ǥ

    //� ����
    public GameObject Star_Effects;
    public AudioSource source;
    public AudioClip hitClip;

    private string asteroidTag = "ASTEROID";

    //����� ��ġ ��Ʈ��
    private float halfHeight = 0f;
    private float halfWidth = 0f;

    //�Ҹ�
    public Transform Firepos;
    public GameObject ConinBullet;

    //����� ��ġ�е� ��Ʈ��
    public Touch_Pad_ pad;
    private Vector3 moveVector;
    void Start()
    {
        tr = GetComponent<Transform>();
        source = GetComponent<AudioSource>();
        halfHeight = Screen.height * 0.5f; //������� pc�ػ󵵿� �����̶� 0.5�� ���Ѵ�. (���ϱⰡ �����⺸�� ������ ������)
        halfWidth = Screen.width * 0.5f; //������� pc�ػ󵵿� �����̶� 0.5�� ���Ѵ�. (���� -> ���� -> ���� -> ������ ��)
        pad = GameObject.Find("Joystick_Pad").GetComponent<Touch_Pad_>();
    }
    public void OnStickPos(Vector3 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }

    void Update()
    {
        CameraOutLimit();
        if (Application.platform == RuntimePlatform.Android) //���� ������ ��ġ�� �ȵ���̵� �϶� �ߵ� (���� ��Ÿ�� ����)
        { //�ڵ��� ��ũ�� ��ǥ�� ���� �ϴ��� 0,0�̴�. ��, ��ǥ�� ���̰� �߻��Ѵ�.
            if(Input.touchCount > 0) //��ġ�� �� Ƚ���� ī�����Ѵ� == ��ġ�� �Ǿ��ٸ�
            {
                //Touchdistance(); //������Ʈ�� ��ġ �Ÿ��� ���ϰ� �Ÿ���ŭ �̵���Ű�� �Լ�
            }
            PadCtrl();
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor) //���� ������ ��ġ�� ������ü������Ƽ�ϰ�� (���� ��Ÿ�� ����)
        {
            //UnityRoketCtrl(); //pc���� ����
            PadCtrl();
        }
        QuitApp();
    }

    private void PadCtrl()
    {
        if (GetComponent<Rigidbody2D>()) //����ٵ�2D�� �����Ѵٸ�
        {
            Vector2 Speed = GetComponent<Rigidbody2D>().velocity; //���� ����
            Speed.x = 4 * h; //h�� �е尡 ����Ű�� x�� ���� 
            Speed.y = 4 * v; //v�� �е尡 ����Ű�� y�� ����
            GetComponent<Rigidbody2D>().velocity = Speed; //���� �ӵ��� �ٽ� �־��ش�.
        }
    }

    private void Touchdistance()
    {
        float deltaPosX = Input.GetTouch(0).position.x - halfWidth; //��ġ�� ��ġ���� ���� ����.
        float deltaPosY = Input.GetTouch(0).position.y - halfHeight; //��Ȯ�� ��ġ ��ġ�� ���ϱ����ؼ��̴�.

        float XPos = deltaPosX - tr.position.x; //��ġ��ġ���� ���� ������Ʈ ��ġ�� ����.
        float YPos = deltaPosY - tr.position.y; //��ġ�Ѱ��� ������Ʈ ���� �Ÿ��� ���´�.

        tr.Translate(Speed * Time.deltaTime * XPos * 0.005f, Speed * Time.deltaTime * YPos * 0.005f, 0f);
        //������Ʈ�� ���� �Ÿ���ŭ �̵���Ŵ
    }

    private void UnityRoketCtrl()
    {
        h = Input.GetAxis("Horizontal"); //x��ǥ ��Ʈ�� a d����
        v = Input.GetAxis("Vertical"); //y��ǥ ��Ʈ�� w s����

        Vector2 moveDir = (h * Vector2.right) + (v * Vector2.up);
        tr.Translate(moveDir.normalized * Speed * Time.deltaTime);
    }

    private void CameraOutLimit() //ī�޶� ���� ������ ������ ���ϰ��ϴ� ����
    {
        #region ���� ù��°
        //if (tr.position.x >= 11.0f)
        //    tr.position = new Vector3(11.0f, tr.position.y, tr.position.z);
        //else if (tr.position.x <= -11.0f)
        //    tr.position = new Vector3(-11.0f, tr.position.y, tr.position.z);
        //if (tr.position.y >= 4.5f)
        //    tr.position = new Vector3(tr.position.x, 4.5f, tr.position.z);
        //else if (tr.position.y <= -4.5f)
        //    tr.position = new Vector3(tr.position.x, -4.5f, tr.position.z);
        #endregion
        #region ���� �ι�°
        tr.position = new Vector3
            (Mathf.Clamp(tr.position.x, -11.0f, 11.0f), Mathf.Clamp(tr.position.y, -4.5f, 4.5f), tr.position.z);
        #endregion
    }

    void QuitApp()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) //����Ͽ��� �ڷΰ��� Ű�� ������ ������ ����ǵ��� �Ѵ�.
            Application.Quit();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(asteroidTag))
        {
            Destroy(other.gameObject); //� ����
            GameObject eff = Instantiate
                (Star_Effects, new Vector3(tr.position.x, tr.position.y, -3), Quaternion.identity); //����Ʈ ����
            Destroy(eff, 0.5f); //0.5���� ����Ʈ ����
            source.PlayOneShot(hitClip, 1.0f);
            GameManager.instance.TurnOn();
        }
    }

    public void Fire()
    {
        Instantiate(ConinBullet,Firepos.position, Quaternion.identity);
    }

 
}
