using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModelChanger : MonoBehaviour
{
    public KHHModel carModel; // 참조: KHHModel 컴포넌트
    private int currentModelTypeIndex = 0; // 현재 적용된 ModelType의 인덱스
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float rotationSpeed = 20f; // 초당 20도 회전
    // Update is called once per frame
    void Update()
    {
        // 마우스 오른쪽 버튼을 누르는 동안
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            // 일정 시간마다 (예: 매 프레임마다) 모델 변경
            ChangeToNextModel();        
        }
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

    }
    void ChangeToNextModel()
    {
        // ModelType enum의 길이를 얻기
        int modelTypeCount = (int)KHHModel.ModelType.Length;

        // 다음 인덱스로 이동 (마지막 인덱스에서는 처음으로 돌아감)
        currentModelTypeIndex = (currentModelTypeIndex + 1) % modelTypeCount;

        // 해당 인덱스의 ModelType을 참조하여 차량 모델 적용
        carModel.Set((KHHModel.ModelType)currentModelTypeIndex);
    }
    

}
