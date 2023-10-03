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

    public LineRenderer lineRenderer;  // LineRenderer�� ���� ����
    public float rayLength = 100f;     // ������ ����
    public Transform controllerTransform;  // ��Ʈ�ѷ��� Transform


    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);  // ��Ʈ�ѷ��� ��ġ�� �������� ���� ����
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
