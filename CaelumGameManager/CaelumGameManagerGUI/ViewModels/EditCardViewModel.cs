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
        private void SetWindowName(string context)
        {
            switch (context)
            {
                case "create":
                    this.DisplayName = "Create Card";
                    break;
                case "edit":
                    this.DisplayName = "Edit Card";
                    break;
                default:
                    this.DisplayName = "Edit Card";
                    break;
            }
        }

        public EditCardViewModel(string openContext)
        {
            SetWindowName(openContext);
        }

        public string[] CardTypes { get; } = Enum.GetNames(typeof(CardType));

        private string selectedType = CardType.Mod.ToString();

        public string SelectedType
        {
            get { return selectedType; }
            set { selectedType = value; }
        }
    }
}
