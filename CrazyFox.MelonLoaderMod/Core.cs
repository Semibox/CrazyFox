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

        foreach (var character in DataManager.Instance.unsortedCharacterData)
        {
            passives.Add(character.passive);

            var weapon = character.weapon;
            var requirementName = weapon.GetUnlockRequirement()?.name;
            if (requirementName is null || string.IsNullOrEmpty(requirementName))
            {
                weapons.Add(character.weapon);
                continue;
            }
            var hasAchivement = DataManager.Instance.achievementsData.TryGetValue(requirementName, out var achievement);
            if (!hasAchivement)
            {
                weapons.Add(character.weapon);
                continue;
            }
            var alreadyUnlock = achievement.IsUnlocked();
            if (alreadyUnlock)
            {
                weapons.Add(character.weapon);
                continue;
            }
        }
        var fox = DataManager.Instance.characterData[ECharacter.Fox];

        fox.weapon = weapons[System.Random.Shared.Next(0, weapons.Count)];
        fox.passive = passives[System.Random.Shared.Next(0, passives.Count)];
    }

}