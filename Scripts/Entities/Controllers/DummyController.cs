using Entities.Parts;
using Managers;
using UI;
using UnityEngine;

namespace Entities.Controllers
{
    public class DummyController : Controller, IToolTipInfo
    {
        [SerializeField] private Sprite _icon;
        
        public string Name => LocalizationManager.GetLocalizedString("Coquilles vides", "Empty shell");
        public Sprite Icon => _icon;
        //public string[] Info => Entity.PartsHandler.GetPartInfo(~(PartIdentifier)0);
        public Color MainColor => Color.cyan;
        public Sprite[] ImageList => Entity.PartsHandler.GetPartImage(~(PartIdentifier)0);
        public bool UpdateAction => true;
    }
}
