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
using System.Linq;
using System.Text;

namespace DxBlazorApplication1.Blazor.Server.Controllers
{
    public partial class SaveProdusViewController : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public SaveProdusViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Produs);
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

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var currentProdus = View.CurrentObject as Produs;
            if (currentProdus == null) return;

            // verific daca e deja codul
            var existing = ObjectSpace.GetObjectsQuery<Produs>()
                .FirstOrDefault(p => p.Cod == currentProdus.Cod && p.ID != currentProdus.ID);

            if (existing != null)
            {
                // calculez noul cod
                int maxCod = ObjectSpace.GetObjectsQuery<Produs>().Max(p => (int?)p.Cod) ?? 0;
                int newCod = maxCod + 1;

                // update
                currentProdus.Cod = newCod;

                // mesaj info
                Application.ShowViewStrategy.ShowMessage(
                    $"Atentie! Codul pe care l-ati introdus este deja folosit. Vi s-a generat automat un nou numar, anume {newCod}",
                    InformationType.Info,
                    displayInterval: 5000
                );
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
