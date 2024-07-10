using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCtrl : MonoBehaviour
{
    /* 필요한 요소
    1. 움직일 오브젝트 위치 값.
    2. 움직일 스피드.
    3. 로켓이 있는 위치.
    */
    public Transform tr;

    private float Speed = 4f;
    private float h = 0f, v = 0f; //로켓의 x,y좌표

    //운석 감지
    public GameObject Star_Effects;
    public AudioSource source;
    public AudioClip hitClip;

    private string asteroidTag = "ASTEROID";

    //모바일 터치 컨트롤
    private float halfHeight = 0f;
    private float halfWidth = 0f;

    //불릿
    public Transform Firepos;
    public GameObject ConinBullet;

    //모바일 터치패드 컨트롤
    public Touch_Pad_ pad;
    private Vector3 moveVector;
    void Start()
    {
        tr = GetComponent<Transform>();
        source = GetComponent<AudioSource>();
        halfHeight = Screen.height * 0.5f; //모바일은 pc해상도에 절반이라 0.5를 곱한다. (곱하기가 나누기보다 연산이 빠르다)
        halfWidth = Screen.width * 0.5f; //모바일은 pc해상도에 절반이라 0.5를 곱한다. (덧셈 -> 뺄셈 -> 곱셈 -> 나눗셈 순)
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
        if (Application.platform == RuntimePlatform.Android) //앱을 실행한 위치가 안드로이드 일때 발동 (현재 런타임 감지)
        { //핸드폰 스크린 좌표는 왼쪽 하단이 0,0이다. 즉, 좌표에 차이가 발생한다.
            if(Input.touchCount > 0) //터치가 된 횟수를 카운팅한다 == 터치가 되었다면
            {
                //Touchdistance(); //오브젝트와 터치 거리를 구하고 거리만큼 이동시키는 함수
            }
            PadCtrl();
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor) //앱을 실행한 위치가 윈도우체제유니티일경우 (현재 런타임 감지)
        {
            //UnityRoketCtrl(); //pc에서 조종
            PadCtrl();
        }
        QuitApp();
    }

    private void PadCtrl()
    {
        if (GetComponent<Rigidbody2D>()) //리깃바디2D가 존재한다면
        {
            Vector2 Speed = GetComponent<Rigidbody2D>().velocity; //힘과 방향
            Speed.x = 4 * h; //h는 패드가 가르키는 x축 방향 
            Speed.y = 4 * v; //v는 패드가 가르키는 y축 방향
            GetComponent<Rigidbody2D>().velocity = Speed; //변한 속도를 다시 넣어준다.
        }
    }

    private void Touchdistance()
    {
        float deltaPosX = Input.GetTouch(0).position.x - halfWidth; //터치한 위치에서 반을 뺀다.
        float deltaPosY = Input.GetTouch(0).position.y - halfHeight; //정확한 터치 위치를 구하기위해서이다.

        float XPos = deltaPosX - tr.position.x; //터치위치에서 현재 오브젝트 위치를 뺀다.
        float YPos = deltaPosY - tr.position.y; //터치한곳과 오브젝트 사이 거리가 나온다.

        tr.Translate(Speed * Time.deltaTime * XPos * 0.005f, Speed * Time.deltaTime * YPos * 0.005f, 0f);
        //오브젝트를 구한 거리만큼 이동시킴
    }

    private void UnityRoketCtrl()
    {
        h = Input.GetAxis("Horizontal"); //x좌표 컨트롤 a d조정
        v = Input.GetAxis("Vertical"); //y좌표 컨트롤 w s조정

        Vector2 moveDir = (h * Vector2.right) + (v * Vector2.up);
        tr.Translate(moveDir.normalized * Speed * Time.deltaTime);
    }

    private void CameraOutLimit() //카메라 범위 밖으로 나가지 못하게하는 로직
    {
        #region 로직 첫번째
        //if (tr.position.x >= 11.0f)
        //    tr.position = new Vector3(11.0f, tr.position.y, tr.position.z);
        //else if (tr.position.x <= -11.0f)
        //    tr.position = new Vector3(-11.0f, tr.position.y, tr.position.z);
        //if (tr.position.y >= 4.5f)
        //    tr.position = new Vector3(tr.position.x, 4.5f, tr.position.z);
        //else if (tr.position.y <= -4.5f)
        //    tr.position = new Vector3(tr.position.x, -4.5f, tr.position.z);
        #endregion
        #region 로직 두번째
        tr.position = new Vector3
            (Mathf.Clamp(tr.position.x, -11.0f, 11.0f), Mathf.Clamp(tr.position.y, -4.5f, 4.5f), tr.position.z);
        #endregion
    }

    void QuitApp()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) //모바일에서 뒤로가기 키를 누르면 게임이 종료되도록 한다.
            Application.Quit();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(asteroidTag))
        {
            Destroy(other.gameObject); //운석 삭제
            GameObject eff = Instantiate
                (Star_Effects, new Vector3(tr.position.x, tr.position.y, -3), Quaternion.identity); //이펙트 생성
            Destroy(eff, 0.5f); //0.5초후 이펙트 삭제
            source.PlayOneShot(hitClip, 1.0f);
            GameManager.instance.TurnOn();
        }
    }

    public void Fire()
    {
        Instantiate(ConinBullet,Firepos.position, Quaternion.identity);
    }

 
}
