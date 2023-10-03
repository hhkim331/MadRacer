//using UnityEngine;
//using UnityEngine.EventSystems;

//public class Weapon_Ex : MonoBehaviour
//{
//    public GameObject descriptionUIPrefab; // 설명 UI 프리팹
//    private GameObject spawnedUI; // 생성된 UI 인스턴스
//    public Transform controllerTransform; // 컨트롤러의 Transform
//    public float rayLength = 100f; // 레이의 길이

//    private bool isUIActive = false; // UI 활성화 상태

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
