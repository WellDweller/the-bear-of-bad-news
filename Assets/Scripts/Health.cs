using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnDead { get; private set; }

    [field:SerializeField] public UnityEvent OnDamage { get; private set; }

    [SerializeField] Sprite emptyHit;
    [SerializeField] Sprite filledHit;

    [SerializeField] int maxHealth;

    int currentHealth;

    Stack<Image> renderers = new();



    void Awake()
    {
        Reset();
        EnsureCorrectNumberOfSprites();
        UpdateSprites();
    }



    public void TakeDamage()
    {
        if (currentHealth <= 0)
            throw new System.Exception("Took damage after already dead!");
        
        currentHealth -= 1;

        UpdateSprites();

        if (currentHealth > 0)
            OnDamage?.Invoke();
        else
            OnDead?.Invoke();
    }

    public void Reset()
    {
        currentHealth = maxHealth;
    }

    void EnsureCorrectNumberOfSprites()
    {
        while (renderers.Count < maxHealth)
        {
            var obj = new GameObject("Health Sprite");
            obj.transform.SetParent(transform);

            var renderer = obj.AddComponent<Image>();
            renderer.transform.localScale = Vector3.one;
            renderers.Push(renderer);
        }

        while (renderers.Count > maxHealth)
        {
            var renderer = renderers.Pop();
            Destroy(renderer.gameObject);
        }
    }

    void UpdateSprites()
    {
        int hits = maxHealth - currentHealth;

        foreach (var renderer in renderers)
        {
            Sprite sprite;
            if (hits > 0)
            {
                sprite = filledHit;
                hits -= 1;
            }
            else
            {
                sprite = emptyHit;
            }

            renderer.sprite = sprite;
            // sprite bounds are defined in world-space units, but this is rendered to the canvas which sizes things in pixels :(
            renderer.rectTransform.sizeDelta = sprite.bounds.size * 100;
        }
    }
}
