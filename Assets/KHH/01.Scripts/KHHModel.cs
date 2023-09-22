using UnityEngine;
using static KHHModel;

public class KHHModel : MonoBehaviour
{
    public enum ModelType
    {
        Black,
        Red,
        Camo,
        G24K,
        Camo2,
    }
    //public ModelType modelType = ModelType.Black;

    public MeshRenderer[] body;
    public MeshRenderer[] interior;
    public MeshRenderer[] weapon;
    public Material[] bodyMats;
    public Material[] interiorMats;
    public Material[] weaponMats;
    public KHHModel.ModelType modelType = KHHModel.ModelType.Black;

    // Start is called before the first frame update
    public void Set(ModelType modelType)
    {
        this.modelType = modelType;
        SetModel((int)modelType);
        Debug.Log("Model Type: " + modelType);

    }
    public KHHModel.ModelType CurrentModelType
    {
        get { return modelType; }
    }


    void SetModel(int type)
    {
        for (int i = 0; i < body.Length; i++)
        {
            body[i].material = bodyMats[type];
        }
        for (int i = 0; i < interior.Length; i++)
        {
            interior[i].material = interiorMats[type];
        }
        for (int i = 0; i < weapon.Length; i++)
        {
            weapon[i].material = weaponMats[type];
        }
    }
}
