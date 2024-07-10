using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Pad : MonoBehaviour
{// MonoBehaviour��� �θ� Ŭ������ ����� ��ӽ��� �־���. 
    private RectTransform _touchPad;
    private RocketCtrl RocketCtrl;

    private Vector3 _StartPos = Vector3.zero;
    private float _dragRadius = 122.0f;
    private int _touchPadId = -1; // �� �ȿ��� ��ġ�� �Ǿ����� �Ǵ��ϴ� ����
    private bool IsBtnPress = false; // ��ư�� ���� �������� �Ǵ�
    public Vector3 differ;

    void Start()
    {
        _touchPad = GetComponent<RectTransform>();
        _StartPos = _touchPad.position; // �ʱ⿡ ��ġ�е� �̹����� �ִ� ��ġ�� ����
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

    private void FixedUpdate() //���� ������ (�׳� ������Ʈ�Լ��� �ڵ尡 �þ�� �������� �þ �̸� ������ �Լ�)
    {//��Ȯ�� �������� ���� ���� �����ϰų� ���ϴ� �ð�Ÿ�ӿ� �ݵ�� ������ ���̶�� �ش��Լ��� ����Ѵ�.
        if (Application.platform == RuntimePlatform.Android)
        {
            HandleTouchInput();
        }
        else if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            HandleInput(Input.mousePosition);
        }
    }
    void HandleTouchInput() //����Ͽ� ��ġ �е� �̵��Լ� (���ȿ��� ��ġ�� �Ǵ��� �ƴ����� �Ǵ�, �������� pc�� ������ �Լ��� ���)
    {
        int i = 0;
        if(Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches) //Input.touches ��� �迭�� ��ġ�� ��ġ�� ����, �ϳ��� ������.
            {//��, Input.touches�� ��ġ�� ��ǥ���� ���� �迭�̴�.
                i++;
                Vector2 touchPos = new Vector2(touch.position.x, touch.position.y); // �ϳ��� ���� ��ǥ���� ���Ϳ� ����
                if(touch.phase == TouchPhase.Began) //���ǹ� ���� : ��ġ���� == ��ġ�� ������ ���� �Ǿ��ٸ�
                {
                    if(touch.position.x <= (_StartPos.x + _dragRadius)) //��ġ�� ��ġ�� �е� �� �ȿ� �ִٸ�
                        _touchPadId = i; // ��ġ�� �Ǵ��ϴ� ������ ����� �ٲ� (��� -> ��ġ�� / ���� -> ��ġ x)
                    
                    if (touch.position.y <= (_StartPos.y + _dragRadius)) //��ġ�� ��ġ�� �е� �� �ȿ� �ִٸ�
                        _touchPadId = i; // ��ġ�� �Ǵ��ϴ� ������ ����� �ٲ� (��� -> ��ġ�� / ���� -> ��ġ x)
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {//��ġ ���� == �����̰� �ְų� ��ġ ���� == ���� ���� ���� �ϳ���� �۵�
                    if(_touchPadId == i) //���ȿ� �ִٸ� i�� touchPadId�� ������ �Լ� �۵�
                    {
                        HandleInput(touchPos); //���� �� �Լ����� ������ �����δ�.
                    }
                }
                if(touch.phase == TouchPhase.Ended) //��ġ�� �����ٸ�
                {
                    if (_touchPadId == i) // �������� ���ȿ��� ��ġ�� �ǰ� �־��ٸ�.
                        _touchPadId = -1; // _touchPadId�� -1�� �����ϸ鼭 ���ȿ� ��ġ�� ���ٶ�� �˸�.
                }
            }
        }
    }

    void HandleInput(Vector3 input) //pc�� ��ġ �е� �̵��Լ�
    {
        if(IsBtnPress) //�е� �̹����� �����ٸ�
        {
            Vector3 diff = (input - _StartPos); // ���̽�ƽ�� ������ ����� �Ÿ� ���� ���� �� �ִ�. (��ġ��ġ - �ʱ���ġ)
            if(diff.sqrMagnitude > _dragRadius * _dragRadius) //��ü �Ÿ��� ���Ͽ� ������ ����ٸ�
            {
                diff.Normalize(); //���������� �̾Ƴ��� ���Ͽ� ����ȭ ��Ŵ
                _touchPad.position = _StartPos + diff * _dragRadius; //��ġ�ϴ� �������� �е��̹����� ���� ���ʿ� ��ġ ��Ų��.
            }
            else
            {
                _touchPad.position = input; //���ȿ� ������ ��ġ�� ����, ��ġ�� �״�� ����Ѵ�.
            }
        }
        else //�е� �̹����� ���ٸ�
        {
            _touchPad.position = _StartPos; //�ʱ���ġ�� ����
        }

        differ = _touchPad.position - _StartPos;
        Vector2 normalDiff = new Vector2(differ.x / _dragRadius, differ.y / _dragRadius); //�Ÿ����� ���������γ����� �����̵ȴ�.

        if(RocketCtrl != null)
        {
            RocketCtrl.OnStickPos(normalDiff);
        }
    }
}
