using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DxBlazorApplication1.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DxBlazorApplication1.Blazor.Server.Controllers
{
    public partial class SaveIesireViewController : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public SaveIesireViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Iesire);
            TargetViewType = ViewType.Any;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            ModificationsController modificationsController = Frame.GetController<ModificationsController>();

            if (modificationsController != null)
            {
                modificationsController.SaveAction.Executing += SaveAction_Executing;

            }
        }

        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            var currentIesire = View.CurrentObject as Iesire;

            // daca exista iesiri din anul curent care au acelasi numar cu iesirea curenta
            if (ObjectSpace.GetObjectsQuery<Iesire>()
                .Where(i => i.Data.Value.Year == DateTime.Now.Year)
                .Where(i => i.Numar == currentIesire.Numar)
                .Where(i => i.ID != currentIesire.ID)
                .Count() > 0)
            {
                var numarNou = ObjectSpace.GetObjectsQuery<Iesire>()
                 .Where(i => i.Data.Value.Year == DateTime.Now.Year)
                 .Where(i => i.ID != currentIesire.ID)
                 .Max(i => i.Numar) + 1;

                currentIesire.Numar = numarNou;

                Application.ShowViewStrategy.ShowMessage($"Atentie! Numarul pe care l-ati introdus este deja folosit. Vi s-a generat automat un nou numar, anume {numarNou}", InformationType.Info, displayInterval: 5000);

            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
