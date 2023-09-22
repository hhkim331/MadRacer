using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModelChanger : MonoBehaviour
{
    public KHHModel carModel; // ����: KHHModel ������Ʈ
    private int currentModelTypeIndex = 0; // ���� ����� ModelType�� �ε���
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float rotationSpeed = 20f; // �ʴ� 20�� ȸ��
    // Update is called once per frame
    void Update()
    {
        // ���콺 ������ ��ư�� ������ ����
        if (Input.GetMouseButton(1))
        {
            // ���� �ð����� (��: �� �����Ӹ���) �� ����
            ChangeToNextModel();
        
        }
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

    }
    void ChangeToNextModel()
    {
        // ModelType enum�� ���̸� ���
        int modelTypeCount = System.Enum.GetValues(typeof(KHHModel.ModelType)).Length;

        // ���� �ε����� �̵� (������ �ε��������� ó������ ���ư�)
        currentModelTypeIndex = (currentModelTypeIndex + 1) % modelTypeCount;

        // �ش� �ε����� ModelType�� �����Ͽ� ���� �� ����
        carModel.Set((KHHModel.ModelType)currentModelTypeIndex);
    }
    

}
