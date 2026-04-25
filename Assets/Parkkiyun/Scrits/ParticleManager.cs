using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager instance;
    public static ParticleManager Instance { get { return instance; } }

    [SerializeField] 
    private GameObject matchEffectPF; 
    [SerializeField] 
    private GameObject clickEffectPF;  

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayMatchEffect(Vector3 position)
    {
        PlayEffect(matchEffectPF, position);
    }

    public void PlayClickEffect(Vector3 pos)
    {
        PlayEffect(clickEffectPF, pos);
    }

    private void PlayEffect(GameObject _obj, Vector3 pos)
    {
        if (_obj != null)
        {
            Quaternion rot = Quaternion.Euler(90, 0, 0);
            GameObject effect = Instantiate(_obj, pos, rot);
            effect.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Destroy(effect,1.5f);
        }
    }
}
