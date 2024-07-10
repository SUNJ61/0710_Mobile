using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Pad2 : MonoBehaviour
{
    private RectTransform _touchPad;
    private Vector3 _StartPos = Vector3.zero;
    public float _dragRadius = 122f;
    private int _touchPadId = -1; //���ȿ��� ��ġ ���� �ƴ��� �Ǵ�
    private bool IsBtnPressed = false; //��ư�� ���� ��������
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
    //��Ȯ�� �������� ���� ���� ���� �ϰų� ���ϴ� �ð�Ÿ�ӿ�
    // �ݵ�� ���� �� ���̶�� FixedUpdate �� ��� �ؾ� �Ѵ�.
    //���������� 
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
    void HandleTouchInput()//����Ͽ� �е�  ���ȿ��� ��ġ�� �Ǵ� �� �ƴ��� �Ǵ�
    {
        int i = 0;
         //�ѹ��̶� ��ġ�� �Ǿ��ٸ� 
        if(Input.touchCount >0)
        {                           //��ġ�� ��ǥ���� ������ �ִ� �迭
            foreach( Touch touch in Input.touches )
            {
                i++;
                Vector2 touchPos = new Vector2(touch.position.x, touch.position.y);
                  // ��ġ����  == ��ġ�� ���� �� ���� �Ǿ��ٸ� 
                if(touch.phase == TouchPhase.Began )
                {    //��ġ�� ���콺Ŀ���� �հ����� x�� �� ���� �ȿ� ��ٸ� 
                    if(touch.position.x <=(_StartPos.x + _dragRadius))
                    {
                        _touchPadId = i;
                    }
                    //��ġ�� ���콺Ŀ���� �հ����� y�� �� ���� �ȿ� ��ٸ� 
                    if (touch.position.y <= (_StartPos.y + _dragRadius))
                    {
                        _touchPadId = i;
                    }
                }
                // ��ġ���°� �����̰� �ְų�  ���� ���¶��  
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (_touchPadId == i) // ���ȿ� �ִٸ� 
                    {
                        HandleInput(touchPos); //���� �� �Լ����� ������ �����δ�.
                    }

                }
                if(touch.phase == TouchPhase.Ended) //��ġ�� �����ٸ� 
                {
                    if (_touchPadId == i)
                    {
                        _touchPadId = -1;
                    }

                }

            }



        }


    }
    void HandleInput(Vector3 input)//pc�� �е� �̵� �Լ�
    {
        if (IsBtnPressed) //�����ٸ� 
        {                         //��ġ��ġ - ��ŸƮ ��ġ = ����� �Ÿ� 
            Vector3 diffVector = (input - _StartPos);

            //��ü �Ÿ��� ���ؼ�   ������ ����ٸ� 
            if (diffVector.sqrMagnitude > _dragRadius * _dragRadius)
            {
                diffVector.Normalize();//����ȭ
                _touchPad.position = _StartPos + diffVector * _dragRadius;
                // ��ġ�� ���콺Ŀ���� �հ����� ������ ������
                //��ġ �е�� ���ȿ��� ������ ������ä �� ���� �پ� �ִ�.
            }
            else //���ȿ� ������  �״�� �Է°��� ���� �����δ�.
            {
                _touchPad.position = input;
            }

        }
        else  //����ٸ� 
        {
            _touchPad.position = _StartPos;
        }
                        
         diff = _touchPad.position - _StartPos;
                                      //�Ÿ����� ���������� ������ ������ ���������.
        Vector2 normalDiff = new Vector2(diff.x/_dragRadius, diff.y/_dragRadius);
         if(_rocketCtrl != null)
        {
            _rocketCtrl.OnStickPos(normalDiff);
        }

    }
   
}
