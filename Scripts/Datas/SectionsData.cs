using Levels.Sections;
using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(menuName = "Project 42/New Sections Data", fileName = "SD_Default")]
    public class SectionsData : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private Vector2 _sectionsMaximumSize = new Vector2(250,250);
        [SerializeField] private int _sectionColumn = 3;
        [SerializeField] private int _specialSectionInterval = 5;
        
        [Header("Prefabs")]
        [SerializeField] private Section[] startingSection;
        [SerializeField] private Section[] normalSections;
        [SerializeField] private Section[] specialSections;

        public Vector2 SectionsMaximumSize => _sectionsMaximumSize;
        public int SectionColumn => _sectionColumn;
        public int SpecialSectionInterval => _specialSectionInterval + 1;
        public Section[] StartingSection => startingSection;
        public Section[] NormalSections => normalSections;
        public Section[] SpecialSections => specialSections;
    }
}
