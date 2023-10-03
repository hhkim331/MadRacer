using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }
    public List<ChildAnimationMapping> childMappings;  // 자식 오브젝트와 애니메이션의 매핑 리스트

    public LineRenderer lineRenderer;  // LineRenderer에 대한 참조
    public float rayLength = 100f;     // 레이의 길이
    public Transform controllerTransform;  // 컨트롤러의 Transform


    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);  // 컨트롤러의 위치와 방향으로 레이 생성
        Vector3 rayStart = ray.origin;
        Vector3 rayEnd = ray.origin + ray.direction * rayLength;
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);
            rayEnd = hit.point;
            foreach (ChildAnimationMapping mapping in childMappings)
            {
                if (hit.transform == mapping.childObject)
                {
                    mapping.childAnimator.SetTrigger(mapping.animationTrigger);
                    break;
                }
            }
        }
        lineRenderer.SetPosition(0, rayStart);
        lineRenderer.SetPosition(1, rayEnd);
    }

    public void PlayAnimation()
    {
        //animator.SetTrigger("Active");
    }
}
