using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using UI.Menu.MainMenu;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class MainMenuHandler : UIHandler
{
    [SerializeField] private Menu _current = null;

    private List<Menu> _itemsMenu;
    private Stack<Menu> _previous = new Stack<Menu>();

    protected override void Awake()
    {
        SaveManager.LoadGame();
        
        base.Awake();

        _itemsMenu = new List<Menu>(transform.GetComponentsInChildren<Menu>(true));
        
        SubscribeToAllButton();
    }

    private void Start()
    {
        HideAll();
    }

    private void HideAll()
    {
        foreach (Menu menu in _itemsMenu)
        {
            switch (menu)
            {
                case LeaderboardMenu leaderboardMenu:
                    leaderboardMenu.GetLeaderboard();
                    break;
                case SoundMenu soundMenu:
                    soundMenu.InitSoundSliders();
                    break;
            }
            
            menu.Hide();
        }
    }

    public void ClickSound() => AudioManager.Instance.PlayUI(0);
    private void SubscribeToAllButton()
    {
        foreach(Button button in transform.GetComponentsInChildren<Button>(true))
        {
            button.onClick.AddListener(ClickSound);
        }
    }

    public void GoToMenu(Menu menu)
    {
        _previous.Push(_current.Hide());
        _current = menu;
        menu.Show();
    }

    public void GoBack()
    {
        if(_previous.Count <= 0)
            return;

        _current.Hide();
        _current = _previous.Pop().Show();
    }

    public override void OnEnableMenu(float transitionTime)
    {
        if (_current != null)
            _current.Show();

        StartCoroutine(FadeIn(transitionTime));
    }

    private void OnDestroy()
    {
        _itemsMenu.Clear();
        _previous.Clear();
    }
}
