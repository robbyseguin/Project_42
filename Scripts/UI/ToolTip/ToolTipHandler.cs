using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class ToolTipHandler : MonoBehaviour
    {
        private IToolTipInfo _target;

        [SerializeField] private Image _iconTitle;
        [SerializeField] private TMP_Text _textTitle;
        [SerializeField] private TMP_Text _textDescription;
        [SerializeField] private TMP_Text _textAction;
        [SerializeField] private GameObject _infoContainer;
        [SerializeField] private Image _loadingMask;
        [SerializeField] private Transform _imagesListContainer;
        [SerializeField] private Image[] _mainColorGroup;
        
        private TMP_Text[] _textsInfoList;
        private CanvasGroup _canvasGroup;
        private Image _imagesListItemReference;
        private RectTransform _rectTransform;
        private RectTransform _rectTransformBackground;
        private RectTransform _rectTransformCanvas;
        private List<Image> _imagesList = new List<Image>();
        private List<Image> _imagesOverlayList = new List<Image>();
        private Camera _mainCamera;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _textsInfoList = _infoContainer.GetComponentsInChildren<TMP_Text>(true);
            _imagesListItemReference = _imagesListContainer.GetChild(0).GetComponent<Image>();
            _imagesListItemReference.gameObject.SetActive(false);
            _rectTransform = GetComponent<RectTransform>();
            _rectTransformBackground = transform.GetChild(0).GetComponent<RectTransform>();
            _rectTransformCanvas = transform.parent.GetComponent<RectTransform>();
            _mainCamera = Camera.main;

            HideToolTip();
        }

        private void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Time.timeScale != 0 && Physics.Raycast(ray, out hit, 100.0f) && hit.collider.gameObject.TryGetComponent(out IToolTipInfo target))
            {
                AdjustAnchorOnEdges();
                ShowToolTip();
                
                if(target.UpdateAction)
                {
                    _loadingMask.fillAmount = target.Loading;
                    UpdateInfo(target.Action, _textAction);
                    
                    for (int i = 0; i < _textsInfoList.Length; i++)
                        UpdateInfo(i < target.Info?.Length ? target.Info[i] : default, _textsInfoList[i]);
                }
                
                if(target == _target)
                    return;

                _target = target;
                UpdateUI();
            }
            else
            {
                HideToolTip();
            }
        }

        private void UpdateUI()
        {
            UpdateInfo(_target.Icon, _iconTitle);
            UpdateInfo(_target.Name, _textTitle);
            UpdateInfo(_target.Description, _textDescription);
            UpdateInfo(_target.Action, _textAction);
            _loadingMask.fillAmount = _target.Loading;

            UpdateImageList();

            for (int i = 0; i < _textsInfoList.Length; i++)
                UpdateInfo(i < _target.Info?.Length ? _target.Info[i] : default, _textsInfoList[i]);
            
            foreach (Image image in _mainColorGroup)
                image.color = _target.MainColor;
        }

        private void UpdateImageList()
        {
            foreach (Image image in _imagesList)
                image.gameObject.SetActive(false);
            
            foreach (Image image in _imagesOverlayList)
                image.gameObject.SetActive(false);

            if (_target.ImageList == default || !_target.ImageList.Any())
            {
                _imagesListContainer.gameObject.SetActive(false);
                return;
            }

            _imagesListContainer.gameObject.SetActive(true);

            for (int i = 0; i < _target.ImageList.Length; i++)
            {
                if (i >= _imagesList.Count)
                {
                    _imagesList.Add(Instantiate(_imagesListItemReference, _imagesListContainer));
                    _imagesOverlayList.Add(_imagesList[i].transform.GetChild(0).GetComponent<Image>());
                }
                
                _imagesList[i].gameObject.SetActive(true);
                _imagesList[i].sprite = _target.ImageList[i];
                
                if(_target.ImageListOverlay == default)
                    continue;
                
                _imagesOverlayList[i].gameObject.SetActive(true);
                _imagesOverlayList[i].sprite = _target.ImageListOverlay;
                _imagesOverlayList[i].color = _target.ImageListOverlayColor;
            }
        }

        private void UpdateInfo(string info, TMP_Text container)
        {
            if (info == default)
            {
                container.gameObject.SetActive(false);
                return;
            }

            container.gameObject.SetActive(true);
            container.text = info;
        }

        private void UpdateInfo(Sprite info, Image container)
        {
            if (info == default)
            {
                container.gameObject.SetActive(false);
                return;
            }

            container.gameObject.SetActive(true);
            container.sprite = info;
        }

        private void HideToolTip()
        {
            _canvasGroup.alpha = 0;
        }

        private void ShowToolTip()
        {
            _canvasGroup.alpha = 0.7f;
        }

        private void AdjustAnchorOnEdges()
        {
            Vector2 anchor = Input.mousePosition / _rectTransformCanvas.localScale.x;

            if (anchor.x + _rectTransformBackground.rect.width > _rectTransformCanvas.rect.width)
                anchor.x = _rectTransformCanvas.rect.width - _rectTransformBackground.rect.width;

            if (anchor.y + _rectTransformBackground.rect.height > _rectTransformCanvas.rect.height)
                anchor.y = _rectTransformCanvas.rect.height - _rectTransformBackground.rect.height;

            _rectTransform.anchoredPosition = anchor;
        }
    }
}
