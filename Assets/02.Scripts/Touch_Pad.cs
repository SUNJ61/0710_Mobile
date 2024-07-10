using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Pad : MonoBehaviour
{// MonoBehaviour라는 부모 클래스가 기능을 상속시켜 주었다. 
    private RectTransform _touchPad;
    private RocketCtrl RocketCtrl;

    private Vector3 _StartPos = Vector3.zero;
    private float _dragRadius = 122.0f;
    private int _touchPadId = -1; // 원 안에서 터치가 되었는지 판단하는 변수
    private bool IsBtnPress = false; // 버튼을 누른 상태인지 판단
    public Vector3 differ;

    void Start()
    {
        _touchPad = GetComponent<RectTransform>();
        _StartPos = _touchPad.position; // 초기에 터치패드 이미지가 있던 위치를 저장
        RocketCtrl = GameObject.FindWithTag("Player").GetComponent<RocketCtrl>();
    }

    public void BtnDown()
    {
        IsBtnPress = true;
    }
    public void BtnUp()
    {
        IsBtnPress = false;
    }

    private void FixedUpdate() //고정 프레임 (그냥 업데이트함수는 코드가 늘어나면 프레임이 늘어남 이를 방지한 함수)
    {//정확한 물리량에 따른 것을 구현하거나 원하는 시간타임에 반드시 구현할 것이라면 해당함수를 사용한다.
        if (Application.platform == RuntimePlatform.Android)
        {
            HandleTouchInput();
        }
        else if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            HandleInput(Input.mousePosition);
        }
    }
    void HandleTouchInput() //모바일용 터치 패드 이동함수 (원안에서 터치가 되는지 아닌지만 판단, 움직임은 pc용 움직임 함수를 사용)
    {
        int i = 0;
        if(Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches) //Input.touches 라는 배열이 터치한 위치를 저장, 하나씩 꺼낸다.
            {//즉, Input.touches는 터치한 좌표들을 가진 배열이다.
                i++;
                Vector2 touchPos = new Vector2(touch.position.x, touch.position.y); // 하나씩 꺼낸 좌표값을 벡터에 저장
                if(touch.phase == TouchPhase.Began) //조건문 내용 : 터치유형 == 터치가 이제막 시작 되었다면
                {
                    if(touch.position.x <= (_StartPos.x + _dragRadius)) //터치한 위치가 패드 원 안에 있다면
                        _touchPadId = i; // 터치를 판단하는 변수를 양수로 바꿈 (양수 -> 터치중 / 음수 -> 터치 x)
                    
                    if (touch.position.y <= (_StartPos.y + _dragRadius)) //터치한 위치가 패드 원 안에 있다면
                        _touchPadId = i; // 터치를 판단하는 변수를 양수로 바꿈 (양수 -> 터치중 / 음수 -> 터치 x)
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {//터치 상태 == 움직이고 있거나 터치 상태 == 멈춤 상태 둘중 하나라면 작동
                    if(_touchPadId == i) //원안에 있다면 i와 touchPadId가 같아져 함수 작동
                    {
                        HandleInput(touchPos); //여기 이 함수에서 실제로 움직인다.
                    }
                }
                if(touch.phase == TouchPhase.Ended) //터치가 끝났다면
                {
                    if (_touchPadId == i) // 이전까지 원안에서 터치가 되고 있었다면.
                        _touchPadId = -1; // _touchPadId를 -1로 변경하면서 원안에 터치가 없다라고 알림.
                }
            }
        }
    }

    void HandleInput(Vector3 input) //pc용 터치 패드 이동함수
    {
        if(IsBtnPress) //패드 이미지를 눌렀다면
        {
            Vector3 diff = (input - _StartPos); // 조이스틱이 움직인 방향과 거리 값을 구할 수 있다. (터치위치 - 초기위치)
            if(diff.sqrMagnitude > _dragRadius * _dragRadius) //전체 거리를 비교하여 원밖을 벗어났다면
            {
                diff.Normalize(); //방향정보만 뽑아내기 위하여 정규화 시킴
                _touchPad.position = _StartPos + diff * _dragRadius; //터치하는 방향으로 패드이미지를 제일 끝쪽에 위치 시킨다.
            }
            else
            {
                _touchPad.position = input; //원안에 있으면 터치한 방향, 위치를 그대로 사용한다.
            }
        }
        else //패드 이미지를 땟다면
        {
            _touchPad.position = _StartPos; //초기위치로 변경
        }

        differ = _touchPad.position - _StartPos;
        Vector2 normalDiff = new Vector2(differ.x / _dragRadius, differ.y / _dragRadius); //거리에서 반지름으로나누면 방향이된다.

        if(RocketCtrl != null)
        {
            RocketCtrl.OnStickPos(normalDiff);
        }
    }
}
