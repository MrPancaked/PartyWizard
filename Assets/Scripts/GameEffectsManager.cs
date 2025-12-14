using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameEffectsManager : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private AnimationCurve effectCurve;
    [SerializeField] private float effectDuration;

    private void Start()
    {
        GameManager.Instance.RoomClearedEvent += LevelClearEffectMethod;
    }

    private void OnDisable()
    {
        GameManager.Instance.RoomClearedEvent -= LevelClearEffectMethod;
    }
    
    private void LevelClearEffectMethod()
    {
        StopAllCoroutines();
        StartCoroutine(LevelClearEffect());
    }
    private IEnumerator LevelClearEffect()
    {
        LensDistortion lensDistortion = volume.profile.TryGet(out LensDistortion lens) ? lens : null;
        ChromaticAberration chromatic = volume.profile.TryGet(out ChromaticAberration chrom) ? chrom : null;
        
        if (lensDistortion != null && chromatic != null)
        {
            float lensDistortionIntensity = (float)lensDistortion.intensity;
            float chromaticIntensity = (float)chromatic.intensity;
            float i = 0;
            while (i < effectDuration)
            {
                //lensDistortion.intensity.Override( lensDistortionIntensity + effectCurve.Evaluate(i / effectDuration));
                chromatic.intensity.Override( chromaticIntensity + effectCurve.Evaluate(i / effectDuration));
                yield return new WaitForEndOfFrame();
                i += Time.deltaTime;
            }
        }
    }
}
