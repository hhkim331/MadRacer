using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MouseOverAnimation : MonoBehaviour
{
    //public Animator animator;
    //  public string animationName;
    // public Transform targetChild; 
    [System.Serializable]
    public class ChildAnimationMapping
    {
        public Transform childObject;    // 마우스를 올려놓을 자식 오브젝트
        public string animationTrigger;  // 해당 자식 오브젝트에 마우스를 올려놓았을 때 재생될 애니메이션의 트리거 이름
        public Animator childAnimator;         // 해당 자식 오브젝트에 연결된 애니메이터
        public GameObject associatedUI; // 추가: 애니메이션에 연결된 UI

    }
    public List<ChildAnimationMapping> childMappings;  // 자식 오브젝트와 애니메이션의 매핑 리스트

    public LineRenderer lineRenderer;  // LineRenderer에 대한 참조
    public float rayLength = 100f;     // 레이의 길이
    public OVRInput.Controller controller = OVRInput.Controller.RTouch; // Oculus 컨트롤러 설정
    private GameObject currentActiveUI = null; // 현재 활성화된 UI를 추적

    public GameObject defaultUI; // 기본으로 활성화된 UI

    void Start()
    {
        // 컨트롤러나 라인 렌더러가 설정되지 않았을 때를 대비한 검사 추가
        if (lineRenderer == null)
        {
            Debug.LogError("Line Renderer is not assigned!");
            enabled = false;
            return;
        }
        if (defaultUI != null)
        {
            defaultUI.SetActive(true); // 시작할 때 기본 UI 활성화
        }
    }
    
    private void Update()
    {
        // 컨트롤러의 위치와 방향을 가져옴
        //Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(controller);
        //Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(controller);


        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Vector3 rayStart = ray.origin;
        Vector3 rayEnd = ray.origin + ray.direction * rayLength;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);
           
            //defaultUI.SetActive(false); 

            rayEnd = hit.point;
            foreach (ChildAnimationMapping mapping in childMappings)
            {
                if (hit.transform == mapping.childObject)
                {
                    mapping.childAnimator.SetTrigger(mapping.animationTrigger);

                    if (currentActiveUI != null)
                    {
                        currentActiveUI.SetActive(false); // 이전 UI를 비활성화
                    }

                    if (mapping.associatedUI != null)
                    {
                        Debug.Log("Activating UI for " + mapping.childObject.name);
                        defaultUI.SetActive(false);
                        mapping.associatedUI.SetActive(true); // 새로운 UI 활성화
                        currentActiveUI = mapping.associatedUI; // 현재 활성화된 UI 업데이트
                    }
                    else
                    {
                        Debug.LogWarning("No associated UI for " + mapping.childObject.name);
                        defaultUI.SetActive(true);
                        currentActiveUI = null;
                    }
                    break;
                }
            }
        }
        else
        {
            if (currentActiveUI != null)
            {
                currentActiveUI.SetActive(false);
                currentActiveUI = null;
            }

            defaultUI.SetActive(true); 
        }
        lineRenderer.SetPosition(0, rayStart);
        lineRenderer.SetPosition(1, rayEnd);
    }

    public void PlayAnimation()
    {
        //animator.SetTrigger("Active");
    }
}
