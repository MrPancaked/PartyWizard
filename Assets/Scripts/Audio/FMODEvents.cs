using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance;

    [field: Header("SFX")]
    [field: SerializeField] public EventReference deathSound { get; private set; }
    [field: SerializeField] public EventReference winSound { get; private set; }
    [field: SerializeField] public EventReference playerHurtSound { get; private set; }
    [field: SerializeField] public EventReference skullHurtSound { get; private set; }
    [field: SerializeField] public EventReference knightHurtSound { get; private set; }
    [field: SerializeField] public EventReference bossHurtSound { get; private set; }
    [field: SerializeField] public EventReference castSound { get; private set; }
    [field: SerializeField] public EventReference explosionSound { get; private set; }
    

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return;} // Singleton
        Instance = this;
    }
}
