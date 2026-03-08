using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;


namespace SOL
{
    public class PortraitPresenter : DialoguePresenterBase
    {
        [SerializeField] private Image portraitLeft;
        [SerializeField] private Image portraitRight;
        [SerializeField] private DialogueCharacterStruct[] charList;

        public override YarnTask OnDialogueStartedAsync()
        {
            return YarnTask.CompletedTask;
        }

        public override YarnTask OnDialogueCompleteAsync()
        {
            return YarnTask.CompletedTask;
        }

        public override YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
        {
            SetCharacterPortrait(line.CharacterName);
            return YarnTask.CompletedTask;
        }

        public void SetCharacterPortrait(string charName)
        {
            foreach (DialogueCharacterStruct c in charList)
            {
                if (charName == c.name)
                {
                    Image image = checkPortrait(c.sprite);
                    if (image != null)
                    {
                        image.sprite = c.sprite;
                    }
                }
            }
        }

        private Image checkPortrait(Sprite character)
        {
            if (portraitLeft.sprite == character)
            {
                return null;
            }
            else if (portraitRight.sprite == character)
            {
                return null;
            }
            else if (portraitLeft.sprite == null)
            {
                return portraitLeft;
            }
            else if (portraitRight.sprite == null)
            {
                return portraitRight;
            }
            else
            {
                return portraitLeft;
            }
        }

        [System.Serializable]
        public struct DialogueCharacterStruct
        {
            public string name;
            public Sprite sprite;
        }
    }
}
