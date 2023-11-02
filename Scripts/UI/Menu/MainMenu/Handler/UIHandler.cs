using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIHandler : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    protected IEnumerator FadeIn(float transitionTime)
    {
        _canvasGroup.interactable = true;
        float fadeInStart = Time.time + 2.0f;

        while (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha = Mathf.InverseLerp(0, transitionTime, Time.time - fadeInStart);
            yield return null;
        }

        Show();
    }

    protected IEnumerator FadeOut(float transitionTime)
    {
        float fadeInStart = Time.time + 2.0f;

        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha = 1 - Mathf.InverseLerp(0, fadeInStart, Time.time);
            yield return null;
        }

        _canvasGroup.interactable = false;

        Hide();
    }

    protected void Show()
    {
        _canvasGroup.alpha = 1;
    }

    protected void Hide()
    {
        _canvasGroup.alpha = 0;
    }

    public virtual void OnEnableMenu(float transitionTime){}
    public virtual void OnEnableEntryScreen(float transitionTime){}
    public virtual void OnEnableLoadingScreen(float transitionTime){}
}
