using System;
using System.Collections.Generic;
using Datas;
using Entities;
using Entities.Parts;
using Factories;
using Levels.Sections;
using Managers.Events;
using UnityEngine;
using UI;
using Managers;

namespace Levels
{
    [RequireComponent(typeof(Collider))]
    public class Portal : MonoBehaviour, IToolTipInfo
    {
        [SerializeField] private Sprite _icon = null;
        [SerializeField] private Sprite _iconOverlay = null;
        [SerializeField] private SoundDefinition _portalSounds;
        private AudioSource _portalAudioSource;

        private Section _destination;
        
        private PartsFactory _partsFactory => PartsFactory.Instance;

        private PartIdentifier _illegalPart;
        
        public Section Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                _destination = value;
                List<Sprite> _imgList = new List<Sprite>();

                _illegalPart = _destination._restriction.IllegalParts;
                foreach (Part part in _partsFactory.GetPartInformation(_illegalPart))
                    _imgList.Add(part.Icon);

                ImageList = _imgList.ToArray();
                UpdateColor();
            }
        }

        public Sprite Icon => _icon;
        public string Name => "Portal";
        public string Description => "Used for long distance travel. Sometime they restrict the use of some part and " +
                                     "automatically replace those by company default.\n" + 
                                     "Destination: " + (Destination == null? "unknown":Destination.name) + ".";
        public string[] Info => Destination == null? default:new[]
        {
            _destination._restriction.maxHealth >= 1.0f? "":"Max health\n" + 
            (_destination._restriction.maxHealth <= 0.0f ? "" : "Min health"),
            _destination._restriction.maxHealth >= 1.0f? "":_destination._restriction.maxHealth * 100  + " %\n" + 
            (_destination._restriction.maxHealth <= 0.0f ? "" : _destination._restriction.maxHealth * 100 + " %")
        };
        public Sprite[] ImageList { get; private set; }
        public Sprite ImageListOverlay => _iconOverlay;
        public Color ImageListOverlayColor => new Color(1.0f,0.0f,0.0f,0.7f);
        public Color MainColor => new Color(0.57f,0.11f,0.5f,1.0f);

       
        private void Start() => UpdateColor();                           
        private void UpdateColor()
        {
            UnityEngine.ParticleSystem.MainModule m = GetComponentsInChildren<ParticleSystem>()[0].main;
            Color c = _destination == null? Color.red: Color.green;
            c.a = 0.8f;
            m.startColor = c;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(Destination == null)
                return;
            
            if(!other.TryGetComponent(out IWarpable iWarpable))
                return;
            
            if(other.gameObject.GetInstanceID() != Entity.PlayerGuid)
                return;
            
            if(_destination._restriction.TestEntity(other.GetComponent<Entity>(), _illegalPart))
            {
                _portalSounds.PlayOneSFX(AudioManager.Instance._sfxSource, 0);
                iWarpable.WarpTo(Destination.EntryPortal.transform.position, Quaternion.identity);
                this.Publish();
            }
        }
    }
}