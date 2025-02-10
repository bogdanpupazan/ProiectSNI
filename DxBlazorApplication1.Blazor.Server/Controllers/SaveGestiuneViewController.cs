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
    public partial class SaveGestiuneViewController : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public SaveGestiuneViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Gestiune);
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
            var currentGestiune = View.CurrentObject as Gestiune;
            if (currentGestiune == null) return;

            // verific daca e deja codul
            var existing = ObjectSpace.GetObjectsQuery<Gestiune>()
                .FirstOrDefault(p => p.Cod == currentGestiune.Cod && p.ID != currentGestiune.ID);

            if (existing != null)
            {
                // calculez noul cod
                int maxCod = ObjectSpace.GetObjectsQuery<Gestiune>().Max(p => (int?)p.Cod) ?? 0;
                int newCod = maxCod + 1;

                // update
                currentGestiune.Cod = newCod;

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
