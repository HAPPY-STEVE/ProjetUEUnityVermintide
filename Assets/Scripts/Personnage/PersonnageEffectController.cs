using Cyan;
using Personnage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PersonnageController))]
public class PersonnageEffectController : MonoBehaviour
{

    [SerializeField] private UniversalRendererData rendererData = null;
    [SerializeField] private string featureName = null;
    [SerializeField] private float tempsHitEffet = 1f;
    [SerializeField] private float tempsHealEffet = 1;
    [SerializeField] private float transitionPeriod = 1;
    private bool inHit = false; 
    private PersonnageController pc;

    private void Update()
    {

        ScriptableRendererFeature feature = rendererData.rendererFeatures.Where((f) => f.name == "BlitHit").FirstOrDefault();
        var blitFeature = feature as Blit;
    }

    private void Start()
    {
        pc = GetComponent<PersonnageController>();
        pc.onHitEvent.AddListener(() => { StopAllCoroutines();  StartCoroutine(hitEffect()); });
        ScriptableRendererFeature feature = rendererData.rendererFeatures.Where((f) => f.name == "BlitHit").FirstOrDefault();
        var blitFeature = feature as Blit;
        var material = blitFeature.blitPass.blitMaterial;
        int fullscreenIntensity = Shader.PropertyToID("_FullscreenIntensity");
        material.SetFloat(fullscreenIntensity, 0f);
    }
    private void OnDestroy()
    {
    }


    private IEnumerator hitEffect()
    {
            int fullscreenIntensity = Shader.PropertyToID("_FullscreenIntensity");
            Debug.Log("hit effect");
            float duration = tempsHitEffet;
            ScriptableRendererFeature feature = rendererData.rendererFeatures.Where((f) => f.name == "BlitHit").FirstOrDefault();
            var blitFeature = feature as Blit;
            var material = blitFeature.blitPass.blitMaterial;
            var start = material.GetFloat(fullscreenIntensity);
            var endValue = 0.2f;
            float time = 0; 
            while (time < duration && material.GetFloat(fullscreenIntensity) != endValue)
            {
                Debug.Log("in while" + material.GetFloat(fullscreenIntensity) + Time.timeScale);
                var value = Mathf.Lerp(start, endValue, time);
                material.SetFloat(fullscreenIntensity, value);
                time += Time.deltaTime;
                yield return null;

            }
            time = 0;
            endValue = 0f;
            start = material.GetFloat(fullscreenIntensity);
            while (time < duration && material.GetFloat(fullscreenIntensity) != endValue)
            {
                Debug.Log("out while " + material.GetFloat(fullscreenIntensity) + " " + time);
                var value = Mathf.Lerp(start, endValue, time);
                material.SetFloat(fullscreenIntensity, value);
                time += Time.deltaTime;
                yield return null;

            }


    }


}
