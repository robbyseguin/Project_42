using Dan.Main;
using TMPro;
using UnityEngine;

namespace UI.Menu.MainMenu
{
    public class LeaderboardMenu : Menu
    {
        [SerializeField] private TMP_Text _positions;
        [SerializeField] private TMP_Text _names;
        [SerializeField] private TMP_Text _scores;

        private static string publicKey = "785d2398870aead0db6f37a636add195b23b22e9a2b1bc08a6228880146e4ae5";

        public void GetLeaderboard()
        {
            LeaderboardCreator.GetLeaderboard(publicKey, (msg) =>
            {
                _positions.text = "";
                _names.text = "";
                _scores.text = "";
                
                for (int i = 0; i < msg.Length; i++)
                {
                    _positions.text += (i + 1) + "\n";
                    _names.text += msg[i].Username + "\n";
                    _scores.text += msg[i].Score + "\n";
                }
            });
        }

        public static void SetLeaderboardEntry(string username, int score)
        {
            LeaderboardCreator.UploadNewEntry(publicKey, username, score);
        }
    }
}
