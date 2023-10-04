using UnityEngine;
using UnityEngine.SceneManagement;


public class CarModelManager : MonoBehaviour
{
    public static CarModelManager Instance;

    public KHHModel khhModel; // ���� �𵨿� ���� ����
    public KHHModel.ModelType selectedModelType = KHHModel.ModelType.Black;
    private int currentModelTypeIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        // ������ Ŭ������ ���� ����
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            ChangeToNextModel();

            PlayerPrefs.SetInt("SelectedModelType", (int)khhModel.CurrentModelType);
        }

        // ���� Ŭ������ �� ��ȯ
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            // ������ ���� ���� PlayerPrefs�� ����
            PlayerPrefs.SetInt("SelectedModelType", (int)khhModel.CurrentModelType);

            // �� ��ȯ
            SceneManager.LoadScene("GameTrack");
        }

    }

    void ChangeToNextModel()
    {
        currentModelTypeIndex++; // ���� �� Ÿ������ �̵�
        if (currentModelTypeIndex >= (int)KHHModel.ModelType.Length)
        {
            currentModelTypeIndex = 0; // ��� ���� ��ȸ�� �� ó������ ���ƿ�
        }

        // �� Ÿ���� �����ϰ� �ش� ������ ����
        selectedModelType = (KHHModel.ModelType)currentModelTypeIndex;
        khhModel.Set(selectedModelType);
    }

    void LoadPlayerTestScene()
    {
        // PlayerTest �� �ε�
        SceneManager.LoadScene("GameTrack");
    }
}
