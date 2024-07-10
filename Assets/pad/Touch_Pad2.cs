using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Pad2 : MonoBehaviour
{
    private RectTransform _touchPad;
    private Vector3 _StartPos = Vector3.zero;
    public float _dragRadius = 122f;
    private int _touchPadId = -1; //원안에서 터치 인지 아닌지 판단
    private bool IsBtnPressed = false; //버튼을 누른 상태인지
    public Vector3 diff;
    [SerializeField] private RocketCtrl2 _rocketCtrl;
    void Start()
    {
        _rocketCtrl = GameObject.FindWithTag("Player").GetComponent<RocketCtrl2>();
        _touchPad = GetComponent<RectTransform>();
        _StartPos = _touchPad.position;
    }
    public void ButtonDown()
    {

        IsBtnPressed = true;
    }
    public void ButtonUp()
    {
        IsBtnPressed = false;
    }
    //정확한 물리량에 따른 것을 구현 하거나 원하는 시간타임에
    // 반드시 구현 할 것이라면 FixedUpdate 를 사용 해야 한다.
    //고정프레임 
    private void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            HandleTouchInput();
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            HandleInput(Input.mousePosition);
        }
    }
    void HandleTouchInput()//모바일용 패드  원안에서 터치가 되는 지 아닌지 판단
    {
        int i = 0;
         //한번이라도 터치가 되었다면 
        if(Input.touchCount >0)
        {                           //터치한 좌표들을 가지고 있는 배열
            foreach( Touch touch in Input.touches )
            {
                i++;
                Vector2 touchPos = new Vector2(touch.position.x, touch.position.y);
                  // 터치유형  == 터치가 이제 막 시작 되었다면 
                if(touch.phase == TouchPhase.Began )
                {    //터치한 마우스커서나 손가락이 x축 원 범위 안에 든다면 
                    if(touch.position.x <=(_StartPos.x + _dragRadius))
                    {
                        _touchPadId = i;
                    }
                    //터치한 마우스커서나 손가락이 y축 원 범위 안에 든다면 
                    if (touch.position.y <= (_StartPos.y + _dragRadius))
                    {
                        _touchPadId = i;
                    }
                }
                // 터치상태가 움직이고 있거나  멈춤 상태라면  
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (_touchPadId == i) // 원안에 있다면 
                    {
                        HandleInput(touchPos); //여기 이 함수에서 실제로 움직인다.
                    }

                }
                if(touch.phase == TouchPhase.Ended) //터치가 끝났다면 
                {
                    if (_touchPadId == i)
                    {
                        _touchPadId = -1;
                    }

                }

            }



        }


    }
    void HandleInput(Vector3 input)//pc용 패드 이동 함수
    {
        if (IsBtnPressed) //눌렀다면 
        {                         //터치위치 - 스타트 위치 = 방향과 거리 
            Vector3 diffVector = (input - _StartPos);

            //전체 거리를 비교해서   원밖을 벗어났다면 
            if (diffVector.sqrMagnitude > _dragRadius * _dragRadius)
            {
                diffVector.Normalize();//정규화
                _touchPad.position = _StartPos + diffVector * _dragRadius;
                // 터치한 마우스커서나 손가락이 원밖을 나가도
                //터치 패드는 원안에서 방향을 유지한채 원 끝에 붙어 있다.
            }
            else //원안에 있으면  그대로 입력값에 따라 움직인다.
            {
                _touchPad.position = input;
            }

        }
        else  //띄었다면 
        {
            _touchPad.position = _StartPos;
        }
                        
         diff = _touchPad.position - _StartPos;
                                      //거리에서 반지름으로 나누면 방향이 만들어진다.
        Vector2 normalDiff = new Vector2(diff.x/_dragRadius, diff.y/_dragRadius);
         if(_rocketCtrl != null)
        {
            _rocketCtrl.OnStickPos(normalDiff);
        }

    }
   
}
