using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectName : uint
{
    PlayerSpawnEffect,

}

public class EffectManager : MonoSingleTon<EffectManager>
{
    public void PlayEffect(EffectName effectName, Vector3 position)
    {
        GameObject effectPrefab = Resources.Load<GameObject>($"Effect/{effectName}");
        if (effectPrefab != null)
        {
            GameObject effectInstance = Instantiate(effectPrefab, position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect not found: {effectName}");
        }
    }
}
