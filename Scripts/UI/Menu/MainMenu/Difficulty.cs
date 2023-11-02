using UnityEngine;

namespace UI.Menu.MainMenu
{
    [CreateAssetMenu(menuName = "Project 42/New Difficulty", fileName = "New Difficulty")]
    public class Difficulty : ScriptableObject
    {
        public string NameFr;
        public string NameEn;
        public Sprite Icon;
        public float Multiplier;
    }
}