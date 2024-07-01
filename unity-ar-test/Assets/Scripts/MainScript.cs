using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class MainScript : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private ARTrackedImageManager _trackedImageManager;
    [SerializeField] private TMP_Text _text;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Button _createButton;

    private ARTrackedImage _trackedImage = null;
    private GameObject _cube = null;

    private void Awake()
    {
        _trackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);

        _createButton.onClick.AddListener(() =>
        {
            if (_cube != null)
            {
                return;
            }
            _cube = Instantiate(_cubePrefab);
            _cube.transform.position =  _mainCamera.transform.position + _mainCamera.transform.forward * 0.5f;
            _cube.AddComponent<ARAnchor>();
        });
    }

    public void Update()
    {
        List<string> texts = new List<string>(2)
        {
            TransformToString("camera", _mainCamera.transform)
        };

        if (_trackedImage != null)
        {
            texts.Add(TransformToString("tracked", _trackedImage.transform));
        }

        if (_cube != null)
        {
            texts.Add(TransformToString("cube", _cube.transform));
        }

        _text.text = string.Join("\n", texts);
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            _trackedImage = trackedImage;
        }
    }

    private string TransformToString(string tag, Transform transform)
    {
        return $"[{tag}]\n{transform.position}\n{transform.rotation.eulerAngles}\n";
    }
}
