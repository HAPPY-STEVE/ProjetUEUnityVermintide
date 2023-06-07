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
    private PersonnageController pc;

    private void Update()
    {

    }

    private void Start()
    {
        pc = GetComponent<PersonnageController>();
        pc.onHitEvent.AddListener(() => { hitEffect(tempsHitEffet); });
        ScriptableRendererFeature feature = rendererData.rendererFeatures.Where((f) => f.name == "BlitHit").FirstOrDefault();
        var blitFeature = feature as Blit;
        var material = blitFeature.blitPass.blitMaterial;
        material.SetFloat("_VoronoiSpeed", 20f);
    }
    private void OnDestroy()
    {
    }


    private void hitEffect(float f)
    {
        float duration = tempsHitEffet;
        float voronoiSpeed = 0f;
        ScriptableRendererFeature feature = rendererData.rendererFeatures.Where((f) => f.name == "BlitHit").FirstOrDefault();
        var blitFeature = feature as Blit;
        var material = blitFeature.blitPass.blitMaterial;
        float time = 0;
        float startValue = material.GetFloat("_VoronoiSpeed");

        material.SetFloat("_VoronoiSpeed", voronoiSpeed);
        rendererData.SetDirty();
        while (time < duration)
        {
            startValue = material.GetFloat("_VoronoiSpeed");
            time += Time.deltaTime;
            Debug.Log(material.GetFloat("_VoronoiSpeed"));

        }

    }

    private void startHitEffect(float hitTime)
    {
        float duration = hitTime / 2; 
        float voronoiSpeed = 0f; 
        ScriptableRendererFeature feature = rendererData.rendererFeatures.Where((f) => f.name == "BlitHit").FirstOrDefault();
        var blitFeature = feature as Blit;
        var material = blitFeature.blitPass.blitMaterial;
        float time = 0;
        float startValue = material.GetFloat("_VoronoiSpeed");
        while (time < duration)
        {
            startValue = material.GetFloat("_VoronoiSpeed");
            material.SetFloat("_VoronoiSpeed", voronoiSpeed);
            time += Time.deltaTime;
            Debug.Log(material.GetFloat("_VoronoiSpeed"));

        }
        endHitEffect(hitTime/2); 
    }

    private void endHitEffect(float hitTime)
    {
        float voronoiSpeed =  10f;
        ScriptableRendererFeature feature = rendererData.rendererFeatures.Where((f) => f.name == "BlitHit").FirstOrDefault();
        var blitFeature = feature as Blit;
        var material = blitFeature.blitPass.blitMaterial;
        float time = 0;
        float startValue = material.GetFloat("_VoronoiSpeed");
        while (time < hitTime)
        {
            startValue = material.GetFloat("_VoronoiSpeed");

            material.SetFloat("_VoronoiSpeed", voronoiSpeed);
            Debug.Log(material.GetFloat("_VoronoiSpeed"));
            time += Time.deltaTime;
        }
    }

}
