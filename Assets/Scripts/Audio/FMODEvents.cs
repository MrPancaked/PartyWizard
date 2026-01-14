using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance;

    [field: Header("SFX")]
    [field: SerializeField] public EventReference indoorWalkSound { get; private set; }
    [field: SerializeField] public EventReference outdoorWalkSound { get; private set; }
    [field: SerializeField] public EventReference doorOpenSounds { get; private set; }
    [field: SerializeField] public EventReference doorCloseSounds { get; private set; }
    [field: SerializeField] public EventReference healSound { get; private set; }
    [field: SerializeField] public EventReference shieldCastSound { get; private set; }
    [field: SerializeField] public EventReference shieldBreakSound { get; private set; }
    [field: SerializeField] public EventReference deathSound { get; private set; }
    [field: SerializeField] public EventReference winSound { get; private set; }
    [field: SerializeField] public EventReference playerHurtSound { get; private set; }
    [field: SerializeField] public EventReference enemyHurtSound { get; private set; }
    [field: SerializeField] public EventReference castSound { get; private set; }
    [field: SerializeField] public EventReference swordSlash { get; private set; }
    [field: SerializeField] public EventReference explosionSound { get; private set; }
    [field: SerializeField] public EventReference pickupSound { get; private set; }
    [field: SerializeField] public EventReference levelUpSound { get; private set; }
    
    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return;} // Singleton
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
