using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private HpController hpController;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        hpController =  gameObject.GetComponent<HpController>();
        spriteRenderer =  gameObject.GetComponent<SpriteRenderer>();
        hpController.TakeDamageEvent += HitColorEffect;
    }

    private void OnDisable()
    {
        hpController.TakeDamageEvent -= HitColorEffect;
    }

    private void HitColorEffect(TakeDamageData data)
    {
        spriteRenderer.color = Color.white;
        StopAllCoroutines();
        StartCoroutine(HitColorCoroutine());
    }
    private IEnumerator HitColorCoroutine()
    {
        spriteRenderer.color = Color.red;
        
        float i = 0;
        while (i < duration)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.white, i / duration);
            i += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
