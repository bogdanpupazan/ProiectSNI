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
    public partial class SavePartenerViewController : ViewController
    {
        public SavePartenerViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Partener);
            TargetViewType = ViewType.Any;
            // Target required Views (via the TargetXXX properties) and create their Actions.
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
            var currentPartener = View.CurrentObject as Partener;
            if (currentPartener == null) return;

            var existing = ObjectSpace.GetObjectsQuery<Partener>()
                .FirstOrDefault(p => p.Cod == currentPartener.Cod && p.ID != currentPartener.ID);

            if (existing != null)
            {
                int maxCod = ObjectSpace.GetObjectsQuery<Partener>().Max(p => (int?)p.Cod) ?? 0;
                int newCod = maxCod + 1;

                currentPartener.Cod = newCod;

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
