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

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {        Debug.Log("Raycast hit: " + hit.transform.name);

            foreach (ChildAnimationMapping mapping in childMappings)
            {
                if (hit.transform == mapping.childObject)
                {
                    mapping.childAnimator.SetTrigger(mapping.animationTrigger);
                    break;
                }
            }
        }
    }

    public void PlayAnimation()
    {
        //animator.SetTrigger("Active");
    }
}
