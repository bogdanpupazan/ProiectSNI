using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using DxBlazorApplication1.Module.BusinessObjects;
using DxBlazorApplication1.Module.NonPersistent;
using DxBlazorApplication1.Module.Rapoarte;
using DxBlazorApplication1.Module.Rapoarte.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace DxBlazorApplication1.Blazor.Server.Controllers
{
    public partial class CustomSituatieViewController : ViewController<DetailView>
    {
        SimpleAction refreshSituatie; 
        SimpleAction listeazaSituatie; 
        DxBlazorApplication1EFCoreDbContext dbContext;
        private IReportDataSourceHelper reportDataSourceHelper;

        public CustomSituatieViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(CustomSituatieNonPersistent);
            refreshSituatie = new SimpleAction(this, "RefreshSituatieAction", "CustomSituatieActionsCategory")
            {
                Caption = "Refresh",
                ImageName = "Action_Refresh"            
            };
            refreshSituatie.Execute += RefreshSituatie_Execute;

            listeazaSituatie = new SimpleAction(this, "ListareSituatieAction", "CustomSituatieActionsCategory")
            {
                Caption = "Listeaza",
                ImageName = "BO_List"
            };
            listeazaSituatie.Execute += ListeazaSituatie_Execute;

        }

        private void ListeazaSituatie_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            // imi iau detail view ul curent
            var detailView = View.ObjectSpace.Owner as DetailView;
            // aduc dashboard ul ce contine listview ul meu custom de intrari/ iesiri
            var dashboardViewListaItemeSituatie = (detailView.Items.Where(i => i.Id == "ItemSituatieNonPersistentItem").FirstOrDefault() as DashboardViewItem);
            // iau listiew-ul efectiv din dashboard
            var listViewItemeSituatie = dashboardViewListaItemeSituatie.InnerView;
            // iau obiectul curent pentru a trimite mai departe ca parametri in procedura valorile introduse 
            CustomSituatieNonPersistent parametriCurenti = View.CurrentObject as CustomSituatieNonPersistent;

            if (parametriCurenti.DataInceput == DateTime.MinValue || parametriCurenti.DataSfarsit == DateTime.MinValue)
            {
                Application.ShowViewStrategy.ShowMessage("Campurile 'Data Inceput' si 'Data Sfarsit' sunt obligatorii.", InformationType.Error, 3000);
                return;
            }
            if (parametriCurenti.DataInceput > parametriCurenti.DataSfarsit)
            {
                Application.ShowViewStrategy.ShowMessage("Data Inceput nu poate fi mai mare decat Data Sfarsit.", InformationType.Error, 3000);
                return;
            }


            BindingList<ItemSituatieNonPersistent> itemsList = new BindingList<ItemSituatieNonPersistent>();

            // populez din procedura lista mea curenta

            if (parametriCurenti.ToateGestiunile)
            {
                parametriCurenti.Gestiune = null;
            }
            else if (parametriCurenti.OGestiune)
            {
                if (parametriCurenti.Gestiune == null)
                {
                    Application.ShowViewStrategy.ShowMessage("Va rugam selectati o gestiune.", InformationType.Error, 3000);
                    return;
                }
            }
            var storedProcedure = parametriCurenti.TipAfisare == TipAfisare.Intrari ? "CalculIntrari" : "CalculIesiri";


            using var command = dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = storedProcedure;
            command.CommandType = CommandType.StoredProcedure;

            var dataInceputParam = new SqlParameter("DataInceput", parametriCurenti.DataInceput);
            command.Parameters.Add(dataInceputParam);

            var dataSfarsitParam = new SqlParameter("DataSfarsit", parametriCurenti.DataSfarsit);
            command.Parameters.Add(dataSfarsitParam);

            if (parametriCurenti.Gestiune != null)
            {
                var gestiuneParam = new SqlParameter("GestiuneID", parametriCurenti.Gestiune.ID);
                command.Parameters.Add(gestiuneParam);

            }

            using var adapter = new SqlDataAdapter(command as SqlCommand);
            var dataSet = new DataSet();
            adapter.Fill(dataSet);

            DataTable dataTableProduseIntrari = dataSet.Tables[0];
            foreach (DataRow row in dataTableProduseIntrari.Rows)
            {
                ItemSituatieNonPersistent itemSituatie = new ItemSituatieNonPersistent
                {
                    ID = Guid.NewGuid(),
                    NumarMiscare = row["NumarMiscare"] != DBNull.Value ? Convert.ToInt32(row["NumarMiscare"]) : 0,
                    DataMiscare = row["DataMiscare"] != DBNull.Value ? Convert.ToDateTime(row["DataMiscare"]) : DateTime.MinValue,
                    DenumireProdus = row["DenumireProdus"] != DBNull.Value ? Convert.ToString(row["DenumireProdus"]) : "",
                    Cantitate = row["Cantitate"] != DBNull.Value ? Convert.ToInt32(row["Cantitate"]) : 0,
                    Valoare = row["Valoare"] != DBNull.Value ? Convert.ToInt32(row["Valoare"]) : 0,
                    Total = row["Total"] != DBNull.Value ? Convert.ToInt32(row["Total"]) : 0,
                    DenumireGestiune = row["DenumireGestiune"] != DBNull.Value ? Convert.ToString(row["DenumireGestiune"]) : "",
                };

                itemsList.Add(itemSituatie);
            }



            IReportDataV2 reportData = ObjectSpace.FirstOrDefault<ReportDataV2>(data => data.DisplayName == "Situatie");
            string handler = ReportDataProvider.GetReportStorage(Frame.Application.ServiceProvider).GetReportContainerHandle(reportData);

            ReportServiceController controller = Frame.GetController<ReportServiceController>();
            ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(Frame.Application.Modules);
            reportDataSourceHelper = reportsModule.ReportsDataSourceHelper;

            reportDataSourceHelper.BeforeShowPreview += (s, e) => {
                XtraReport report = e.Report as XtraReport;
               
                switch (report.GetType().Name)
                {
                    case nameof(SituatieReport):

                        if (report.Parameters["XafReportParametersObject"]?.Value == null)
                        {
                            SituatieRPO rpo = new SituatieRPO(ReportDataProvider.GetReportObjectSpaceProvider(Frame.Application.ServiceProvider));
                            rpo.DataSfarsit = parametriCurenti.DataSfarsit;
                            rpo.DataInceput = parametriCurenti.DataInceput;
                            if (parametriCurenti.TipAfisare == TipAfisare.Intrari)
                                rpo.TipSituatie = "intrari";
                            else rpo.TipSituatie = "iesiri";
                            reportDataSourceHelper.SetXafReportParametersObject(report, rpo);
                        }
                        report.Parameters["DataInceput"].Value = parametriCurenti.DataInceput;
                        report.Parameters["DataSfarsit"].Value = parametriCurenti.DataSfarsit;
                        if (parametriCurenti.TipAfisare == TipAfisare.Intrari)
                            report.Parameters["TipSituatie"].Value = "intrari";
                        else report.Parameters["TipSituatie"].Value = "iesiri";
                        report.DataSource = itemsList;
                        break;
                }
            };


            if (controller != null)
            {
                controller.ShowPreview(handler, null, null);
            }

        }

        private void ReportDataSourceHelper_BeforeShowPreview(object sender, BeforeShowPreviewEventArgs e)
        {
           
        }

        private void RefreshSituatie_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            // imi iau detail view ul curent
            var detailView = View.ObjectSpace.Owner as DetailView;
            // aduc dashboard ul ce contine listview ul meu custom de intrari/ iesiri
            var dashboardViewListaItemeSituatie = (detailView.Items.Where(i => i.Id == "ItemSituatieNonPersistentItem").FirstOrDefault() as DashboardViewItem);
            // iau listiew-ul efectiv din dashboard
            var listViewItemeSituatie = dashboardViewListaItemeSituatie.InnerView;
            // iau obiectul curent pentru a trimite mai departe ca parametri in procedura valorile introduse 
            CustomSituatieNonPersistent parametriCurenti = View.CurrentObject as CustomSituatieNonPersistent;

            if (parametriCurenti.DataInceput == DateTime.MinValue || parametriCurenti.DataSfarsit == DateTime.MinValue)
            {
                Application.ShowViewStrategy.ShowMessage("Campurile 'Data Inceput' si 'Data Sfarsit' sunt obligatorii.", InformationType.Error, 3000);
                return;
            }
            if (parametriCurenti.DataInceput > parametriCurenti.DataSfarsit)
            {
                Application.ShowViewStrategy.ShowMessage("Data Inceput nu poate fi mai mare decat Data Sfarsit.", InformationType.Error, 3000);
                return;
            }


            BindingList<ItemSituatieNonPersistent> itemsList = new BindingList<ItemSituatieNonPersistent>();

            // populez din procedura lista mea curenta

            
            if (parametriCurenti.ToateGestiunile)
            {
                parametriCurenti.Gestiune = null;
            }
            else if (parametriCurenti.OGestiune)
            {
                if (parametriCurenti.Gestiune == null)
                {
                    Application.ShowViewStrategy.ShowMessage("Va rugam selectati o gestiune.", InformationType.Error, 3000);
                    return;
                }
            }
            else
            {
                Application.ShowViewStrategy.ShowMessage("Va rugam selectati o gestiune.", InformationType.Error, 3000);
                return;
            }



            var storedProcedure = parametriCurenti.TipAfisare == TipAfisare.Intrari ? "CalculIntrari" : "CalculIesiri";


            using var command = dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = storedProcedure;
            command.CommandType = CommandType.StoredProcedure;

            var dataInceputParam = new SqlParameter("DataInceput", parametriCurenti.DataInceput);
            command.Parameters.Add(dataInceputParam);

            var dataSfarsitParam = new SqlParameter("DataSfarsit", parametriCurenti.DataSfarsit);
            command.Parameters.Add(dataSfarsitParam);

            if (parametriCurenti.Gestiune != null)
            {
                var gestiuneParam = new SqlParameter("GestiuneID", parametriCurenti.Gestiune.ID);
                command.Parameters.Add(gestiuneParam);

            }

            using var adapter = new SqlDataAdapter(command as SqlCommand);
            var dataSet = new DataSet();
            adapter.Fill(dataSet);

            DataTable dataTableProduseIntrari = dataSet.Tables[0];
            foreach (DataRow row in dataTableProduseIntrari.Rows)
            {
                ItemSituatieNonPersistent itemSituatie = new ItemSituatieNonPersistent
                {
                    ID = Guid.NewGuid(),
                    NumarMiscare = row["NumarMiscare"] != DBNull.Value ? Convert.ToInt32(row["NumarMiscare"]) : 0,
                    DataMiscare = row["DataMiscare"] != DBNull.Value ? Convert.ToDateTime(row["DataMiscare"]) : DateTime.MinValue,
                    DenumireProdus = row["DenumireProdus"] != DBNull.Value ? Convert.ToString(row["DenumireProdus"]) : "",
                    Cantitate = row["Cantitate"] != DBNull.Value ? Convert.ToInt32(row["Cantitate"]) : 0,
                    Valoare = row["Valoare"] != DBNull.Value ? Convert.ToInt32(row["Valoare"]) : 0,
                    Total = row["Total"] != DBNull.Value ? Convert.ToInt32(row["Total"]) : 0,
                    DenumireGestiune = row["DenumireGestiune"] != DBNull.Value ? Convert.ToString(row["DenumireGestiune"]) : "",
                };

                itemsList.Add(itemSituatie);
            }


            (listViewItemeSituatie as ListView).CollectionSource.List.Clear();
            foreach (var row in itemsList)
                (listViewItemeSituatie as ListView).CollectionSource.List.Add(row);
            (listViewItemeSituatie as ListView).Refresh();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            IObjectSpace objectSpace = (View.ObjectSpace as NonPersistentObjectSpace).AdditionalObjectSpaces[0];
            dbContext = ((DevExpress.ExpressApp.EFCore.EFCoreObjectSpace)objectSpace).DbContext as DxBlazorApplication1EFCoreDbContext;

        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
