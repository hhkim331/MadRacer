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
        public Transform childObject;    // ���콺�� �÷����� �ڽ� ������Ʈ
        public string animationTrigger;  // �ش� �ڽ� ������Ʈ�� ���콺�� �÷������� �� ����� �ִϸ��̼��� Ʈ���� �̸�
        public Animator childAnimator;         // �ش� �ڽ� ������Ʈ�� ����� �ִϸ�����

    }
    public List<ChildAnimationMapping> childMappings;  // �ڽ� ������Ʈ�� �ִϸ��̼��� ���� ����Ʈ

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
