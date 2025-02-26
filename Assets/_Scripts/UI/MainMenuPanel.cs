using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : UIPanel
{
    [SerializeField] private Button _playButton;
    protected override void Awake()
    {
        base.Awake();
        _playButton.onClick.AddListener(RequestPanel<LevelSelectionPanel>);
    }
}
