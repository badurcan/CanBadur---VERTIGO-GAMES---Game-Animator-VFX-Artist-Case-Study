using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private float _dissolveTime = 0.75f;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private int _verticalDissolveAmount = Shader.PropertyToID("_VerticalDissolve");

    private bool isDissolved = false;
    private bool isVerticallyDissolved = false;

    private void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame) 
        {
            if (!isDissolved)
            {
                StartCoroutine(Vanish(true, false)); // Only DissolveAmount
                isDissolved = true;
            }
        }

        if (Keyboard.current.fKey.wasPressedThisFrame) 
        {
            if (!isVerticallyDissolved)
            {
                StartCoroutine(Vanish(false, true)); // Only VerticalDissolve
                isVerticallyDissolved = true;
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame) 
        {
            StartCoroutine(Appear(true, true)); // Reset both dissolves
            isDissolved = false;
            isVerticallyDissolved = false;
        }
    }

    private IEnumerator Vanish(bool useDissolve, bool useVertical)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedDissolve = Mathf.Lerp(0, 1.1f, elapsedTime / _dissolveTime);
            float lerpedVerticalDissolve = Mathf.Lerp(0f, 1.1f, elapsedTime / _dissolveTime);

            for (int i = 0; i < _materials.Length; i++)
            {
                if (useDissolve)
                    _materials[i].SetFloat(_dissolveAmount, lerpedDissolve);

                if (useVertical)
                    _materials[i].SetFloat(_verticalDissolveAmount, lerpedVerticalDissolve);
            }

            yield return null;
        }
    }

    private IEnumerator Appear(bool useDissolve, bool useVertical)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedDissolve = Mathf.Lerp(1.1f, 0f, elapsedTime / _dissolveTime);
            float lerpedVerticalDissolve = Mathf.Lerp(1.1f, 0f, elapsedTime / _dissolveTime);

            for (int i = 0; i < _materials.Length; i++)
            {
                if (useDissolve)
                    _materials[i].SetFloat(_dissolveAmount, lerpedDissolve);

                if (useVertical)
                    _materials[i].SetFloat(_verticalDissolveAmount, lerpedVerticalDissolve);
            }

            yield return null;
        }
    }
}
