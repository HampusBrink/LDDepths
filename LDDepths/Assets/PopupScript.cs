using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupScript : MonoBehaviour
{
    [SerializeField] private Ease easeType = Ease.OutBack;
    [SerializeField] private float animationTime = 1;
    [SerializeField] private float scaleFrom = 0;
    [SerializeField] private float scaleTo = 1;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool closeOnSceneSwitch = true;

    private void Awake()
    {
        transform.localScale = new Vector3(scaleFrom, scaleFrom, scaleFrom);
        if(playOnAwake) Open();
    }

    private void OnEnable()
    {
        SceneLoader.StartedSceneLoad += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneLoader.StartedSceneLoad -= OnSceneUnloaded;
    }
    
    private void OnSceneUnloaded()
    {
        if(closeOnSceneSwitch) Close();
    }

    public void Open()
    {
        transform.DOScale(scaleTo,animationTime).SetEase(easeType);
    }
    
    public void Close()
    {
        transform.DOScale(scaleFrom,animationTime).SetEase(easeType);
    }
}
