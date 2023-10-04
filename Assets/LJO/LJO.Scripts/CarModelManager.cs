using UnityEngine;
using UnityEngine.SceneManagement;


public class CarModelManager : MonoBehaviour
{
    public static CarModelManager Instance;

    public KHHModel khhModel; // 차량 모델에 대한 참조
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
        // 오른쪽 클릭으로 색상 변경
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            ChangeToNextModel();

            PlayerPrefs.SetInt("SelectedModelType", (int)khhModel.CurrentModelType);
        }

        // 왼쪽 클릭으로 씬 전환
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            // 선택한 색상 값을 PlayerPrefs에 저장
            PlayerPrefs.SetInt("SelectedModelType", (int)khhModel.CurrentModelType);

            // 씬 전환
            SceneManager.LoadScene("GameTrack");
        }

    }

    void ChangeToNextModel()
    {
        currentModelTypeIndex++; // 다음 모델 타입으로 이동
        if (currentModelTypeIndex >= (int)KHHModel.ModelType.Length)
        {
            currentModelTypeIndex = 0; // 모든 모델을 순회한 후 처음으로 돌아옴
        }

        // 모델 타입을 변경하고 해당 색상을 설정
        selectedModelType = (KHHModel.ModelType)currentModelTypeIndex;
        khhModel.Set(selectedModelType);
    }

    void LoadPlayerTestScene()
    {
        // PlayerTest 씬 로드
        SceneManager.LoadScene("GameTrack");
    }
}
