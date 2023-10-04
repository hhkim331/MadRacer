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
        public Transform childObject;    // ���콺�� �÷����� �ڽ� ������Ʈ
        public string animationTrigger;  // �ش� �ڽ� ������Ʈ�� ���콺�� �÷������� �� ����� �ִϸ��̼��� Ʈ���� �̸�
        public Animator childAnimator;         // �ش� �ڽ� ������Ʈ�� ����� �ִϸ�����
        public GameObject associatedUI; // �߰�: �ִϸ��̼ǿ� ����� UI

    }
    public List<ChildAnimationMapping> childMappings;  // �ڽ� ������Ʈ�� �ִϸ��̼��� ���� ����Ʈ

    public LineRenderer lineRenderer;  // LineRenderer�� ���� ����
    public float rayLength = 100f;     // ������ ����
    public OVRInput.Controller controller = OVRInput.Controller.RTouch; // Oculus ��Ʈ�ѷ� ����
    private GameObject currentActiveUI = null; // ���� Ȱ��ȭ�� UI�� ����

    public GameObject defaultUI; // �⺻���� Ȱ��ȭ�� UI

    void Start()
    {
        // ��Ʈ�ѷ��� ���� �������� �������� �ʾ��� ���� ����� �˻� �߰�
        if (lineRenderer == null)
        {
            Debug.LogError("Line Renderer is not assigned!");
            enabled = false;
            return;
        }
        if (defaultUI != null)
        {
            defaultUI.SetActive(true); // ������ �� �⺻ UI Ȱ��ȭ
        }
    }
    
    private void Update()
    {
        // ��Ʈ�ѷ��� ��ġ�� ������ ������
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(controller);
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(controller);


        RaycastHit hit;
        Ray ray = new Ray(controllerPosition, controllerRotation * Vector3.forward);

        Vector3 rayStart = ray.origin;
        Vector3 rayEnd = ray.origin + ray.direction * rayLength;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);
           
            defaultUI.SetActive(false); 

            rayEnd = hit.point;
            foreach (ChildAnimationMapping mapping in childMappings)
            {
                if (hit.transform == mapping.childObject)
                {
                    mapping.childAnimator.SetTrigger(mapping.animationTrigger);

                    if (currentActiveUI != null)
                    {
                        currentActiveUI.SetActive(false); // ���� UI�� ��Ȱ��ȭ
                    }

                    if (mapping.associatedUI != null)
                    {
                        mapping.associatedUI.SetActive(true); // ���ο� UI Ȱ��ȭ
                        currentActiveUI = mapping.associatedUI; // ���� Ȱ��ȭ�� UI ������Ʈ
                    }
                    else
                    {
                        Debug.LogWarning("No associated UI for " + mapping.childObject.name);
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
