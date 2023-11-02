using System.Collections;
using Entities;
using UnityEngine;
using UI;
using System.Collections.Generic;
using Entities.Parts;
using Factories;
using Levels.Sections;
using Unity.VisualScripting;
using Utilities;

namespace Levels.Interactable
{
    public class Loot : SectionComponent, IInteractable, IToolTipInfo
    {
        [SerializeField] private GameObject _constantLoot;
        [SerializeField] private Transform _lootContainer;
        [SerializeField] private float _delay = 2.0f;
        
        private ILootable _lootable;
        private GameObject _content;
        private ParticleSystem.MainModule _mainModule;

        private Entity _entity;
        private int _entityGuid;
        private WaitForSeconds _wait;
        private Coroutine _coroutine;

        public Color MainColor => _lootable.MainColor;
        public Sprite Icon => _lootable.Icon;
        public string Name => _lootable.Name;
        public string Description => _lootable.Description;
        public string[] Info => _lootable.Info == default ? default : GenerateInfo();
        public string Action => "<color=" + (_entity == null ? "red" : "green") + ">Equip (E)</color>";
        public float Loading { get; private set; }
        public bool UpdateAction => true;

        protected override void Awake()
        {
            base.Awake();
            
            _wait = new WaitForSeconds(0.02f);
            _mainModule = GetComponentsInChildren<ParticleSystem>()[1].main;
        }

        public override void OnSectionInitialisation()
        {
            base.OnSectionInitialisation();

            if(_constantLoot != null)
            {
                SetLoot(Instantiate(_constantLoot, transform));
                return;
            }
            
            SetLoot(LootsFactory.Instance.GetLoot().gameObject);
        }
        
        public void SetLoot(GameObject loot)
        {
            _content = loot;
            _content.transform.SetParent(_lootContainer, false);

            loot.TryGetComponent(out _lootable);
                
            Color color = MainColor;
            color.a = 0.5f;
            _mainModule.startColor = color;
        }
        
        public override void OnSectionReset()
        {
            base.OnSectionReset();
            
            InteractionCanceled();
            _lootable.Destroy();
            _entity = null;
            _lootable = null;
            _content = null;
            
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetInstanceID() != Entity.PlayerGuid) 
                return;
            
            _entityGuid = other.gameObject.GetInstanceID();
            _entity = other.GetComponent<Entity>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetInstanceID() != _entityGuid) 
                return;

            _entity = null;
        }

        public void InteractionStarted()
        {
            if (!gameObject.activeSelf || _entity == null) 
                return;

            if (_coroutine == null)
                _coroutine = StartCoroutine(ActivationCoroutine());
        }

        public void InteractionCanceled()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            Loading = 0.0f;
            _coroutine = null;
        }

        private IEnumerator ActivationCoroutine()
        {
            float time = Time.time;

            while (Loading < 1.0f)
            {
                Loading = Mathf.InverseLerp(0,_delay, Time.time - time);
                yield return _wait;
            }
            
            if(_lootable.OnPickUp(_entity.gameObject))
            {
                gameObject.SetActive(false);

                if (_lootable as Part)
                    _lootable = null;
                
                _content = null;
            }
        }

        private string[] GenerateInfo()
        {
            List<string> _list = new List<string>(_lootable.Info);
            
            if (_lootable as Part && _entity != null)
            {
                _list.Add("");
                
                _list.Add(_entity.PartsHandler.GetPartInfo((_lootable as Part).PartID)[1]);
                
                string[] splitLoot = _list[1].Split('\n');
                string[] splitEquip = _list[3].Split('\n');

                for (int i = 0; i < splitEquip.Length; i++)
                {
                    if (float.TryParse(splitEquip[i], out float statEquip) &&
                        float.TryParse(splitLoot[i], out float statLoot))
                    {
                        Color color = (statEquip > statLoot || i == 0 ? Color.red :
                            statEquip < statLoot ? Color.green : Color.white);
                        
                        _list[2] += "<color=#" + color.ToHexString() + ">>></color>";
                    }
                    
                    _list[2] += "\n";
                }
                
                // Swapping :O
                (_list[1], _list[3]) = (_list[3], _list[1]);
            }

            return _list.ToArray();
        }
    }
}