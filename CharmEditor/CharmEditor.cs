using Modding;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CharmEditor
{
    internal class CharmEditor : Mod, ITogglableMod, IMenuMod, IGlobalSettings<GlobalSettings>
    {
        internal static CharmEditor Instance { get; private set; }

        public GlobalSettings globalSettings { get; private set; } = new();

        public bool ToggleButtonInsideMenu => true;

        public CharmEditor() : base("CharmEditor") { }

        public override string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public override void Initialize()
        {
            Log("Initializing");

            Instance = this;
            ModHooks.GetPlayerIntHook += ModHooks_GetPlayerIntHook;
            ModHooks.DashVectorHook += ModHooks_DashVectorHook;
            Log("Initialized");
        }

        public void OnLoadGlobal(GlobalSettings globalSettings)
        {
            this.globalSettings = globalSettings;
        }

        public GlobalSettings OnSaveGlobal()
        {
            return globalSettings;
        }

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            return new List<IMenuMod.MenuEntry> {
                toggleButtonEntry.Value,
                new IMenuMod.MenuEntry {
                    Name = "Reset",
                    Description = "Resets every option",
                    Values = new string[] { "", "Options have been reset (Reopen HK)" },
                    Saver = opt => {
                        if (opt == 1) {
                            globalSettings.Reset();
                        }
                    },
                    Loader = () => 0
                },
                GetMenuEntry("Wayward Compass Cost", "Wayward Compass notch cost", () => globalSettings.CompassCost, val => globalSettings.CompassCost = val),
                GetMenuEntry("Gathering Swarm Cost", "Gathering Swarm notch cost", () => globalSettings.GatheringSwarmCost, val => globalSettings.GatheringSwarmCost = val),
                GetMenuEntry("Stalwart Shell Cost", "Stalwart Shell notch cost", () => globalSettings.StalwartShellCost, val => globalSettings.StalwartShellCost = val),
                GetMenuEntry("Soul Catcher Cost", "Soul Catcher notch cost", () => globalSettings.SoulCatcherCost, val => globalSettings.SoulCatcherCost = val),
                GetMenuEntry("Shaman Stone Cost", "Shaman Stone notch cost", () => globalSettings.ShamanStoneCost, val => globalSettings.ShamanStoneCost = val),
                GetMenuEntry("Soul Eater Cost", "Soul Eater notch cost", () => globalSettings.SoulEaterCost, val => globalSettings.SoulEaterCost = val),
                new IMenuMod.MenuEntry {
                    Name = "No DownwardDash (Dashmaster)",
                    Description = "Disables the ability to down dash while using DashMaster",
                    Values = new string[] { "Off", "On" },
                    Saver = opt => globalSettings.No_DownwardsDash_DashMaster = opt switch {
                        0 => false,
                        1 => true,
                        _ => throw new InvalidOperationException()
                    },
                    Loader = () => globalSettings.No_DownwardsDash_DashMaster ? 1 : 0
                },
                GetMenuEntry("Dashmaster Cost", "Dashmaster notch cost", () => globalSettings.DashmasterCost, val => globalSettings.DashmasterCost = val),
                GetMenuEntry("Sprintmaster Cost", "Sprintmaster notch cost", () => globalSettings.SprintmasterCost, val => globalSettings.SprintmasterCost = val),
                GetMenuEntry("Grubsong Cost", "Grubsong notch cost", () => globalSettings.GrubsongCost, val => globalSettings.GrubsongCost = val),
                GetMenuEntry("Grubberfly's Elegy Cost", "Grubberfly's Elegy notch cost", () => globalSettings.GrubberflysElegyCost, val => globalSettings.GrubberflysElegyCost = val),
                GetMenuEntry("Fragile/Unbreakable Heart", "Fragile Heart notch cost", () => globalSettings.FragileHeartCost, val => globalSettings.FragileHeartCost = val),
                GetMenuEntry("Fragile/Unbreakable Greed", "Fragile Greed notch cost", () => globalSettings.FragileGreedCost, val => globalSettings.FragileGreedCost = val),
                GetMenuEntry("Fragile/Unbreakable Strength", "Fragile Strength notch cost", () => globalSettings.FragileStrengthCost, val => globalSettings.FragileStrengthCost = val),
                GetMenuEntry("Spell Twister Cost", "Spell Twister notch cost", () => globalSettings.SpellTwisterCost, val => globalSettings.SpellTwisterCost = val),
                GetMenuEntry("Steady Body Cost", "Steady Body notch cost", () => globalSettings.SteadyBodyCost, val => globalSettings.SteadyBodyCost = val),
                GetMenuEntry("Heavy Blow Cost", "Heavy Blow notch cost", () => globalSettings.HeavyBlowCost, val => globalSettings.HeavyBlowCost = val),
                GetMenuEntry("Quick Slash Cost", "Quick Slash notch cost", () => globalSettings.QuickslashCost, val => globalSettings.QuickslashCost = val),
                GetMenuEntry("Longnail Cost", "Longnail notch cost", () => globalSettings.LongnailCost, val => globalSettings.LongnailCost = val),
                GetMenuEntry("Mark of Pride Cost", "Mark of Pride notch cost", () => globalSettings.MarkOfPrideCost, val => globalSettings.MarkOfPrideCost = val),
                GetMenuEntry("Baldur Shell Cost", "Baldur Shell notch cost", () => globalSettings.BaldurShellCost, val => globalSettings.BaldurShellCost = val),
                GetMenuEntry("Flukenest Cost", "Flukenest notch cost", () => globalSettings.FlukenestCost, val => globalSettings.FlukenestCost = val),
                GetMenuEntry("Defender's Crest Cost", "Defender's Crest notch cost", () => globalSettings.DefendersCrestCost, val => globalSettings.DefendersCrestCost = val),
                GetMenuEntry("Thorns of Agony Cost", "Thorns of Agony notch cost", () => globalSettings.ThornsCost, val => globalSettings.ThornsCost = val),
                GetMenuEntry("Fury of the Fallen Cost", "Fury of the Fallen notch cost", () => globalSettings.FuryCost, val => globalSettings.FuryCost = val),
                GetMenuEntry("Quick Focus Cost", "Quick Focus notch cost", () => globalSettings.QuickFocusCost, val => globalSettings.QuickFocusCost = val),
                GetMenuEntry("Lifeblood Heart Cost", "Lifeblood Heart notch cost", () => globalSettings.LifebloodHeartCost, val => globalSettings.LifebloodHeartCost = val),
                GetMenuEntry("Lifeblood Core Cost", "Lifeblood Core notch cost", () => globalSettings.LifebloodCoreCost, val => globalSettings.LifebloodCoreCost = val),
                GetMenuEntry("Joni's Blessing Cost", "Joni's Blessing notch cost", () => globalSettings.JonisBlessingCost, val => globalSettings.JonisBlessingCost = val),
                GetMenuEntry("Shape of Unn Cost", "Shape of Unn notch cost", () => globalSettings.ShapeOfUnnCost, val => globalSettings.ShapeOfUnnCost = val),
                GetMenuEntry("Nailmaster's Glory Cost", "Nailmaster's Glory notch cost", () => globalSettings.NailmastersGloryCost, val => globalSettings.NailmastersGloryCost = val),
                GetMenuEntry("Weaversong Cost", "Weaversong notch cost", () => globalSettings.WeaversongCost, val => globalSettings.WeaversongCost = val),
                GetMenuEntry("Dream Wielder Cost", "Dream Wielder notch cost", () => globalSettings.DreamWielderCost, val => globalSettings.DreamWielderCost = val),
                GetMenuEntry("Dreamshield Cost", "Dreamshield notch cost", () => globalSettings.DreamshieldCost, val => globalSettings.DreamshieldCost = val),
                GetMenuEntry("Glowing Womb Cost", "Glowing Womb notch cost", () => globalSettings.GlowingWombCost, val => globalSettings.GlowingWombCost = val),
                GetMenuEntry("Deep Focus Cost", "Deep Focus notch cost", () => globalSettings.DeepFocusCost, val => globalSettings.DeepFocusCost = val),
                GetMenuEntry("Sharp Shadow Cost", "Sharp Shadow notch cost", () => globalSettings.SharpShadowCost, val => globalSettings.SharpShadowCost = val),
                GetMenuEntry("Sporeshroom Cost", "Sporeshroom notch cost", () => globalSettings.SporeshroomCost, val => globalSettings.SporeshroomCost = val),
                GetMenuEntry("Hiveblood Cost", "Hiveblood notch cost", () => globalSettings.HivebloodCost, val => globalSettings.HivebloodCost = val),
                GetMenuEntry("Grimmchild Cost", "Grimmchild notch cost", () => globalSettings.GrimmchildCost, val => globalSettings.GrimmchildCost = val),
                GetMenuEntry("Carefree Melody Cost", "Carefree Melody notch cost", () => globalSettings.CarefreeMelodyCost, val => globalSettings.CarefreeMelodyCost = val),
                GetMenuEntry("King's Soul Cost", "King's Soul notch cost", () => globalSettings.KingsSoulCost, val => globalSettings.KingsSoulCost = val),
                GetMenuEntry("Void Heart Cost", "Void Heart notch cost", () => globalSettings.VoidHeartCost, val => globalSettings.VoidHeartCost = val)
            };
        }

        private IMenuMod.MenuEntry GetMenuEntry(string name, string Description, Func<int> getter, Action<int> setter)
        {
            return new IMenuMod.MenuEntry
            {
                Name = name,
                Description = Description,
                Values = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11" },
                Saver = opt =>
                {
                    if (opt < 0 || opt > 11)
                        throw new InvalidOperationException();
                    setter(opt);
                },
                Loader = () =>
                {
                    int value = getter();
                    if (value < 0 || value > 11)
                        throw new InvalidOperationException();
                    return value;
                }
            };
        }

        public void Unload()
        {
            ModHooks.GetPlayerIntHook -= ModHooks_GetPlayerIntHook;
            ModHooks.DashVectorHook -= ModHooks_DashVectorHook;
        }

        private Vector2 ModHooks_DashVectorHook(Vector2 vector)
        {
            if (globalSettings.No_DownwardsDash_DashMaster)
            {
                if (vector.y < 0)
                {
                    HeroController hc = HeroController.instance;
                    return new Vector2(hc.cState.facingRight ? hc.DASH_SPEED : -hc.DASH_SPEED, 0f);
                }
            }
            return vector;
        }

        private int HandleCharm(int charmId)
        {
            return charmId switch
            {
                36 => PlayerData.instance.royalCharmState == 4
                    ? globalSettings.VoidHeartCost
                    : globalSettings.KingsSoulCost,

                40 => PlayerData.instance.grimmChildLevel == 5
                    ? globalSettings.CarefreeMelodyCost
                    : globalSettings.GrimmchildCost,

                _ => throw new ArgumentOutOfRangeException(nameof(charmId), "Unsupported charm ID")
            };
        }

        private int ModHooks_GetPlayerIntHook(string name, int orig)
        {
            return name switch
            {
                nameof(PlayerData.instance.charmCost_1) => globalSettings.GatheringSwarmCost,
                nameof(PlayerData.instance.charmCost_2) => globalSettings.CompassCost,
                nameof(PlayerData.instance.charmCost_3) => globalSettings.GrubsongCost,
                nameof(PlayerData.instance.charmCost_4) => globalSettings.StalwartShellCost,
                nameof(PlayerData.instance.charmCost_5) => globalSettings.BaldurShellCost,
                nameof(PlayerData.instance.charmCost_6) => globalSettings.FuryCost,
                nameof(PlayerData.instance.charmCost_7) => globalSettings.QuickFocusCost,
                nameof(PlayerData.instance.charmCost_8) => globalSettings.LifebloodHeartCost,
                nameof(PlayerData.instance.charmCost_9) => globalSettings.LifebloodCoreCost,
                nameof(PlayerData.instance.charmCost_10) => globalSettings.DefendersCrestCost,
                nameof(PlayerData.instance.charmCost_11) => globalSettings.FlukenestCost,
                nameof(PlayerData.instance.charmCost_12) => globalSettings.ThornsCost,
                nameof(PlayerData.instance.charmCost_13) => globalSettings.MarkOfPrideCost,
                nameof(PlayerData.instance.charmCost_14) => globalSettings.SteadyBodyCost,
                nameof(PlayerData.instance.charmCost_15) => globalSettings.HeavyBlowCost,
                nameof(PlayerData.instance.charmCost_16) => globalSettings.SharpShadowCost,
                nameof(PlayerData.instance.charmCost_17) => globalSettings.SporeshroomCost,
                nameof(PlayerData.instance.charmCost_18) => globalSettings.LongnailCost,
                nameof(PlayerData.instance.charmCost_19) => globalSettings.ShamanStoneCost,
                nameof(PlayerData.instance.charmCost_20) => globalSettings.SoulCatcherCost,
                nameof(PlayerData.instance.charmCost_21) => globalSettings.SoulEaterCost,
                nameof(PlayerData.instance.charmCost_22) => globalSettings.GlowingWombCost,
                nameof(PlayerData.instance.charmCost_23) => globalSettings.FragileHeartCost,
                nameof(PlayerData.instance.charmCost_24) => globalSettings.FragileGreedCost,
                nameof(PlayerData.instance.charmCost_25) => globalSettings.FragileStrengthCost,
                nameof(PlayerData.instance.charmCost_26) => globalSettings.NailmastersGloryCost,
                nameof(PlayerData.instance.charmCost_27) => globalSettings.JonisBlessingCost,
                nameof(PlayerData.instance.charmCost_28) => globalSettings.ShapeOfUnnCost,
                nameof(PlayerData.instance.charmCost_29) => globalSettings.HivebloodCost,
                nameof(PlayerData.instance.charmCost_30) => globalSettings.DreamWielderCost,
                nameof(PlayerData.instance.charmCost_31) => globalSettings.DashmasterCost,
                nameof(PlayerData.instance.charmCost_32) => globalSettings.QuickslashCost,
                nameof(PlayerData.instance.charmCost_33) => globalSettings.SpellTwisterCost,
                nameof(PlayerData.instance.charmCost_34) => globalSettings.DeepFocusCost,
                nameof(PlayerData.instance.charmCost_35) => globalSettings.GrubberflysElegyCost,
                nameof(PlayerData.instance.charmCost_36) => HandleCharm(36),
                nameof(PlayerData.instance.charmCost_37) => globalSettings.SprintmasterCost,
                nameof(PlayerData.instance.charmCost_38) => globalSettings.DreamshieldCost,
                nameof(PlayerData.instance.charmCost_39) => globalSettings.WeaversongCost,
                nameof(PlayerData.instance.charmCost_40) => HandleCharm(40),
                _ => orig,
            };
        }
    }
}