using UnityEngine;
using UnityEngine.EventSystems;

public class Weapon_Ex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionUIPrefab; // 설명 UI 프리팹
    private GameObject spawnedUI; // 생성된 UI 인스턴스

    public void OnPointerEnter(PointerEventData eventData)
    {
        spawnedUI = Instantiate(descriptionUIPrefab, transform.position, Quaternion.identity);
        spawnedUI.transform.SetParent(transform, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (spawnedUI)
        {
            Destroy(spawnedUI);
        }
    }
}
