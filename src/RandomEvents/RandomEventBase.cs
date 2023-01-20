using System.Collections.Generic;
using UnityEngine;

namespace Spacebucks
{
    public abstract class RandomEventBase
    {
        public string Name;
        protected string Title;
        protected string Body;
        protected string AcceptString;
        protected string KerbalName;
        protected int EventEffect;
        private PopupDialog eventDialog;

        public abstract bool EventCanFire();

        protected abstract void OnEventAccepted();        

        public void OnEventFire()
        {
            List<DialogGUIBase> dialogElements = new List<DialogGUIBase>();
            List<DialogGUIBase> innerElements = new List<DialogGUIBase>
            {
                new DialogGUISpace(10),
                new DialogGUIHorizontalLayout(PaddedLabel(Body))
            };
            DialogGUIVerticalLayout vertical = new DialogGUIVerticalLayout(innerElements.ToArray());
            dialogElements.Add(new DialogGUIScrollList(-Vector2.one, false, false, vertical));
            dialogElements.Add(new DialogGUIButton(AcceptString, OnEventAccepted));
            eventDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("EventDialog", "", Title, UISkinManager.GetSkin("MainMenuSkin"), new Rect(0.5f, 0.5f, 300, 200), dialogElements.ToArray()), false, UISkinManager.GetSkin("MainMenuSkin"));
        }

        private DialogGUIBase[] PaddedLabel(string stringToPad)
        {
            DialogGUIBase[] paddedLayout = new DialogGUIBase[2];
            paddedLayout[0] = new DialogGUISpace(10);
            paddedLayout[1] = new DialogGUILabel(stringToPad);
            return paddedLayout;
        }
    }
}