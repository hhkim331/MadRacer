//using UnityEngine;
//using UnityEngine.EventSystems;

//public class Weapon_Ex : MonoBehaviour
//{
//    public GameObject descriptionUIPrefab; // ���� UI ������
//    private GameObject spawnedUI; // ������ UI �ν��Ͻ�
//    public Transform controllerTransform; // ��Ʈ�ѷ��� Transform
//    public float rayLength = 100f; // ������ ����

//    private bool isUIActive = false; // UI Ȱ��ȭ ����

//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        spawnedUI = Instantiate(descriptionUIPrefab, transform.position, Quaternion.identity);
//        spawnedUI.transform.SetParent(transform, true);
//    }

//    public void OnPointerExit(PointerEventData eventData)
//    {
//        if (spawnedUI)
//        {
//            Destroy(spawnedUI);
//        }
//    }
//}
