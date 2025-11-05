using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(CrazyFox.MelonLoaderMod.Core), "CrazyFox", "1.0.1", "Slimaeus", null)]
[assembly: MelonGame("Ved", "Megabonk")]

namespace CrazyFox.MelonLoaderMod;

public class Core : MelonMod
{
    private const string _mainMenuSceneName = "MainMenu";
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("Initialized.");
    }

    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        if (sceneName != _mainMenuSceneName)
            return;

        var playButtonGameObject = GameObject.Find("B_Play");
        if (playButtonGameObject is null) return;

        var playButton = playButtonGameObject.GetComponent<Button>();
        if (playButton is null) return;
        playButton.onClick.AddListener((UnityAction)UpdateFox);
    }

    private void UpdateFox()
    {
        var weapons = new List<WeaponData>();
        var passives = new List<PassiveData>();
        foreach (var unlockable in DataManager.Instance.unsortedUnlockables)
        {
            LoggerInstance.Msg($"Found unlockable: {unlockable.name} ({unlockable.WasCollected})");
        }
        foreach (var character in DataManager.Instance.unsortedCharacterData)
        {
            weapons.Add(character.weapon);
            passives.Add(character.passive);
            LoggerInstance.Msg($"Added {character.name} ({character.isEnabled})'s {character.weapon.name} ({character.weapon.isEnabled}) and {character.passive.name} ({character.passive}) ");
        }
        var fox = DataManager.Instance.characterData[ECharacter.Fox];
        //var x = DataManager.Instance.characterData[ECharacter.Ninja];

        //var baseWeapon = x.weapon;
        //var basePassive = x.passive;
        fox.weapon = weapons[System.Random.Shared.Next(0, weapons.Count)];
        fox.passive = passives[System.Random.Shared.Next(0, passives.Count)];


        //fox.weapon = baseWeapon;
        //fox.passive = basePassive;
    }

}