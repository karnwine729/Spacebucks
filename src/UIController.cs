using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spacebucks
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class UiControllerSpaceCentre : UiController { }

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class UiControllerFlight : UiController { }

    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class UiControllerTrackingStation : UiController { }

    public class UiController : MonoBehaviour
    {
        public static UiController Instance;
        private ApplicationLauncherButton toolbarButton;
        private PopupDialog mainWindow;
        private int padding;
        private const int PADFACTOR = 10;

        private void Awake()
        {
            Instance = this;
            GameEvents.onGUIApplicationLauncherReady.Add(SetupToolbarButton);
            GameEvents.onGUIApplicationLauncherUnreadifying.Add(RemoveToolbarButton);
        }

        public void SetupToolbarButton()
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER) toolbarButton = ApplicationLauncher.Instance.AddModApplication(ToggleUI, ToggleUI, null, null, null, null, ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT, GameDatabase.Instance.GetTexture("Spacebucks/Spacebucks", false));
        }

        public void RemoveToolbarButton(GameScenes data)
        {
            if (toolbarButton == null) return;
            ApplicationLauncher.Instance.RemoveModApplication(toolbarButton);
        }

        private void OnDisable()
        {
            RemoveToolbarButton(HighLogic.LoadedScene);
        }

        private void OnDestroy()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(SetupToolbarButton);
            GameEvents.onGUIApplicationLauncherUnreadifying.Remove(RemoveToolbarButton);
        }

        private void ToggleUI()
        {
            if (mainWindow == null) mainWindow = DrawMainUi();
            else mainWindow.Dismiss();
        }

        private PopupDialog DrawMainUi()
        {
            padding = 300;
            List<DialogGUIBase> dialogElements = new List<DialogGUIBase>();
            List<DialogGUIBase> innerElements = new List<DialogGUIBase>();
            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) innerElements.Add(new DialogGUILabel("Spacebucks is only available in Career Games!"));
            else
            {
                double gross = Utilities.Instance.GetGrossFunding();
                double facilityCosts = Costs.Instance.GetFacilityCosts();
                double launchCosts = LaunchCosts.Instance.GetLaunchCosts();
                double wageCosts = Costs.Instance.GetWageCosts();
                int bonuses = CrewManager.Instance.GetCrewBonuses();
                double net = Utilities.Instance.GetNetFunding();
                innerElements.Add(new DialogGUISpace(10));
                innerElements.Add(new DialogGUIHorizontalLayout(PaddedLabel("Gross Funding:  +" + RoundCosts(gross), false)));
                innerElements.Add(new DialogGUIHorizontalLayout(PaddedLabel("Facility Costs:  -" + RoundCosts(facilityCosts), false)));
                innerElements.Add(new DialogGUIHorizontalLayout(PaddedLabel("Launch Costs:  -" + RoundCosts(launchCosts), false)));
                innerElements.Add(new DialogGUIHorizontalLayout(PaddedLabel("Wage Costs:  -" + RoundCosts(wageCosts), false)));
                innerElements.Add(new DialogGUIHorizontalLayout(PaddedLabel("Mission Bonuses:  -" + bonuses, false)));
                innerElements.Add(new DialogGUIHorizontalLayout(PaddedLabel("Net Funding:  +" + RoundCosts(net), false)));
                DialogGUIVerticalLayout vertical = new DialogGUIVerticalLayout(innerElements.ToArray());
                vertical.AddChild(new DialogGUIContentSizer(widthMode: ContentSizeFitter.FitMode.Unconstrained, heightMode: ContentSizeFitter.FitMode.MinSize));
                dialogElements.Add(new DialogGUIScrollList(new Vector2(300, 200), false, false, vertical));
            }
            return PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new MultiOptionDialog("SpacebucksUI", "", "Current Funding Estimates", UISkinManager.GetSkin("MainMenuSkin"),
                    GetRect(dialogElements), dialogElements.ToArray()), false, UISkinManager.GetSkin("MainMenuSkin"), false);
        }

        private int RoundCosts(double costs)
        {
            return (int)Math.Round(costs);
        }

        private Rect GetRect(List<DialogGUIBase> dialogElements)
        {
            return new Rect(0.5f, 0.5f, 300, 265) { height = 150 + 50 * dialogElements.Count, width = Math.Max(padding, 280) };
        }

        private DialogGUIBase[] PaddedLabel(string stringToPad, bool largePrint)
        {
            DialogGUIBase[] paddedLayout = new DialogGUIBase[2];
            paddedLayout[0] = new DialogGUISpace(10);
            EvaluatePadding(stringToPad);
            paddedLayout[1] = new DialogGUILabel(stringToPad, MessageStyle(largePrint));
            return paddedLayout;
        }

        private void EvaluatePadding(string stringToEvaluate)
        {
            if (stringToEvaluate.Length * PADFACTOR > padding) padding = stringToEvaluate.Length * PADFACTOR;
        }

        private UIStyle MessageStyle(bool largePrint)
        {
            UIStyle style = new UIStyle
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.LowerCenter,
                stretchWidth = false,
                normal = new UIStyleState
                {
                    textColor = new Color(0.89f, 0.86f, 0.72f)
                }
            };
            if (largePrint) style.fontSize = 23;
            return style;
        }
    }
}
