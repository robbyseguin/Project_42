using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.HUD
{
    public class HudPart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _background;
        [SerializeField] private Image _loading;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _statisticName;
        [SerializeField] private TMP_Text _statisticValue;
        [SerializeField] private Transform _informationPanel;
        [SerializeField] private Image[] _imageColor;

        private WaitForFixedUpdate _wait;
        private Coroutine _cooldown;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _informationPanel.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _informationPanel.gameObject.SetActive(false);
        }

        public void SetInfo(IToolTipInfo toolTipInfo)
        {
            _icon.sprite = toolTipInfo.Icon;
            _title.text = toolTipInfo.Name;
            _description.text = toolTipInfo.Description;
            _statisticName.text = toolTipInfo.Info?[0];
            _statisticValue.text = toolTipInfo.Info?[1];
            _background.color = toolTipInfo.MainColor;

            foreach (Image img in _imageColor)
                img.color = toolTipInfo.MainColor;
        }

        public void StartCooldown(float time)
        {
            if(_cooldown != null)
                StopCoroutine(_cooldown);
            
            _cooldown = StartCoroutine(Cooldown(time));
        }

        private IEnumerator Cooldown(float time)
        {
            float timeOfStart = Time.time;

            while (Time.time < timeOfStart + time)
            {
                _loading.fillAmount = Mathf.InverseLerp(time, 0,Time.time - timeOfStart);
                
                yield return _wait;
            }

            _loading.fillAmount = 0;
        }
    }
}
