using CaelumCoreLibrary.Cards;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumGameManagerGUI.ViewModels
{
    public class EditCardViewModel : Screen
    {
        private string selectedType = CardType.Mod.ToString();

        private void SetContextualNames(string context)
        {
            switch (context)
            {
                case "create":
                    this.DisplayName = "Create Card";
                    this.ConfirmText = "Create";
                    break;
                case "edit":
                    this.DisplayName = "Edit Card";
                    this.ConfirmText = "Confirm";
                    break;
                default:
                    this.DisplayName = "Edit Card";
                    this.ConfirmText = "Confirm";
                    break;
            }
        }

        public EditCardViewModel(string openContext)
        {
            SetContextualNames(openContext);
        }

        public string ConfirmText { get; private set; }
        public void ConfirmButton()
        {

        }

        public string[] CardTypes { get; } = Enum.GetNames(typeof(CardType));

        public string SelectedType
        {
            get { return selectedType; }
            set { selectedType = value; }
        }
    }
}
