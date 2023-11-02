using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Parts;
using Entities.Parts.Cockpits;
using Entities.Parts.Heads;
using Entities.Parts.HeavyWeapons;
using Entities.Parts.LightWeapons;
using Entities.Parts.Movements;
using Managers;
using Managers.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace  UI.HUD
{
    public class HudHandler : MonoBehaviour
    {
        [SerializeField] private Slider _lifeBarSlider;
        [SerializeField] private TMP_Text _lifeText;
        [SerializeField] private Slider _bateryBarSlider;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _roomText;
        [SerializeField] private TMP_Text _console;
        [SerializeField] private CanvasGroup _consoleBG;
        [SerializeField] private TMP_Text _fps;
        [SerializeField] private HudPart[] _hudParts;

        private Queue<string> _logMessages;
        private int _roomNumber;
        private string GetLocalizedPlayer() => LocalizationManager.GetLocalizedString("Joueur", "Player");
        private string GetLocalizedPickedUp() => LocalizationManager.GetLocalizedString("a prit ", "picked up ");

        protected void Awake()
        {
            Cursor.visible = false;
            
            _logMessages = new Queue<string>();
            
            EventsManager.Subscribe<Entity>(UpdateLifeBar, EntityEvent.LIFE_UPDATE);
            EventsManager.Subscribe<Entity>(UpdateBatteryBar, EntityEvent.LIFE_UPDATE);
            EventsManager.Subscribe<WorldManager>(UpdateRoomCount);
            
            EventsManager.Subscribe<HeavyWeaponPart>(StartCooldown, PartsEvents.ACTIVATED);
            EventsManager.Subscribe<HeadPart>(StartCooldown, PartsEvents.ACTIVATED);
            EventsManager.Subscribe<MovementPart>(StartCooldown, PartsEvents.ACTIVATED);
            EventsManager.Subscribe<LightWeaponPart>(StartCooldown, PartsEvents.ACTIVATED);
            
            EventsManager.Subscribe<HeavyWeaponPart>(UpdatePartInfo, PartsEvents.EQUIPED);
            EventsManager.Subscribe<HeadPart>(UpdatePartInfo, PartsEvents.EQUIPED);
            EventsManager.Subscribe<MovementPart>(UpdatePartInfo, PartsEvents.EQUIPED);
            EventsManager.Subscribe<LightWeaponPart>(UpdatePartInfo, PartsEvents.EQUIPED);
            EventsManager.Subscribe<CockpitPart>(UpdatePartInfo, PartsEvents.EQUIPED);
            
            EventsManager.Subscribe<GameManager>(UpdateScore);
            StartCoroutine(UpdateFps());
        }

        private IEnumerator UpdateFps()
        {
            while (true)
            {
                _fps.text = Mathf.Round(1f / Time.unscaledDeltaTime).ToString();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void UpdateScore(EventsDictionary<GameManager>.CallbackContext ctx)
        {
            _scoreText.text = ctx.ReadValue<int>().ToString();
        }

        private void StartCooldown(EventsDictionary<HeavyWeaponPart>.CallbackContext ctx)
        {
            _hudParts[4].StartCooldown(ctx.Caller.Delay);
        }
        private void StartCooldown(EventsDictionary<LightWeaponPart>.CallbackContext ctx)
        {
            _hudParts[3].StartCooldown(ctx.Caller.Delay);
        }
        private void StartCooldown(EventsDictionary<HeadPart>.CallbackContext ctx)
        {
            _hudParts[2].StartCooldown(ctx.Caller.Delay);
        }
        private void StartCooldown(EventsDictionary<MovementPart>.CallbackContext ctx)
        {
            _hudParts[0].StartCooldown(ctx.Caller.Delay);
        }
        private void UpdatePartInfo(EventsDictionary<HeavyWeaponPart>.CallbackContext ctx)
        {
            _hudParts[4].SetInfo(ctx.Caller);
            LogMessage(GetLocalizedPlayer(), GetLocalizedPickedUp() + ctx.Caller.Name, Color.yellow);
        }
        private void UpdatePartInfo(EventsDictionary<LightWeaponPart>.CallbackContext ctx)
        {
            _hudParts[3].SetInfo(ctx.Caller);
            LogMessage(GetLocalizedPlayer(), GetLocalizedPickedUp() + ctx.Caller.Name, Color.magenta);
        }
        private void UpdatePartInfo(EventsDictionary<HeadPart>.CallbackContext ctx)
        {
            _hudParts[2].SetInfo(ctx.Caller);
            LogMessage(GetLocalizedPlayer(), GetLocalizedPickedUp() + ctx.Caller.Name, Color.red);
        }
        private void UpdatePartInfo(EventsDictionary<CockpitPart>.CallbackContext ctx)
        {
            _hudParts[1].SetInfo(ctx.Caller);
        }
        private void UpdatePartInfo(EventsDictionary<MovementPart>.CallbackContext ctx)
        {
            _hudParts[0].SetInfo(ctx.Caller);
            LogMessage(GetLocalizedPlayer(), GetLocalizedPickedUp() + ctx.Caller.Name, Color.blue);
        }
        private void UpdateRoomCount(EventsDictionary<WorldManager>.CallbackContext ctx)
        {
            switch (ctx.EventID)
            {
                case WorldEvents.NEW_WORLD:
                    _roomNumber = 0;
                    break;
                case WorldEvents.NEW_SECTION:
                    _roomNumber++;
                    break;
            }
            _roomText.text = _roomNumber.ToString();
        }
        private void UpdateBatteryBar(EventsDictionary<Entity>.CallbackContext ctx)
        {
            _bateryBarSlider.value = ctx.ReadValue<float>();
        }
        private void UpdateLifeBar(EventsDictionary<Entity>.CallbackContext ctx)
        {
            _lifeBarSlider.value = ctx.ReadValue<float>();
            _lifeText.text = ctx.Caller.Health + "/" + ctx.Caller.MaxHealth;
        }

        public void LogMessage(string name, string message, Color color)
        {
            StopAllCoroutines();
            _consoleBG.alpha = 1;
            
            string msg = "<color=#" + color.ToHexString() + ">[" + name + "]</color> " + message;
            _logMessages.Enqueue(msg);

            if (_logMessages.Count > 10)
                _logMessages.Dequeue();
            
            string result = "";

            foreach (string m in _logMessages)
            {
                result += m + "\n";
            }

            _console.text = result;

            StartCoroutine(FadeOut(2f));
        }

        private IEnumerator FadeOut(float transitionTime)
        {
            yield return new WaitForSeconds(transitionTime);
            float startTime = Time.time;

            while (_consoleBG.alpha > 0.2f)
            {
                _consoleBG.alpha = 1 - Mathf.InverseLerp(0,startTime + transitionTime, Time.time);
                yield return null;
            }
        }
    }
}
