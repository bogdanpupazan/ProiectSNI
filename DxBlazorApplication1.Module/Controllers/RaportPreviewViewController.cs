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
using DevExpress.XtraReports.UI;
using DxBlazorApplication1.Module.BusinessObjects;
using DxBlazorApplication1.Module.Rapoarte.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DxBlazorApplication1.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
    public partial class RaportPreviewViewController : ViewController
    {
        private IReportDataSourceHelper reportDataSourceHelper;
        public RaportPreviewViewController()
        {
            InitializeComponent();
            this.TargetViewId = "";
            this.TargetObjectType = typeof(ReportDataV2);
            this.TargetViewType = ViewType.ListView;
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(Frame.Application.Modules);
            reportDataSourceHelper = reportsModule.ReportsDataSourceHelper;

            reportDataSourceHelper.BeforeShowPreview += ReportDataSourceHelper_BeforeShowPreview;
        }

        private void ReportDataSourceHelper_BeforeShowPreview(object sender, BeforeShowPreviewEventArgs e)
        {

            XtraReport report = e.Report as XtraReport;
            if (!(report is IRaportGeneral)) return;

            using IObjectSpace objectSpace = Frame.Application.CreateObjectSpace(typeof(ReportDataV2));
            DxBlazorApplication1EFCoreDbContext dbContext = (((DevExpress.ExpressApp.EFCore.EFCoreObjectSpace)(objectSpace)).DbContext as DxBlazorApplication1EFCoreDbContext);


            //switch (report.GetType().Name)
            //{
            //    case nameof(ScrisoareMedicalaReport):

            //        ScrisoareMedicalaManager scrisoareMedicalaManager = new ScrisoareMedicalaManager();
            //        scrisoareMedicalaManager.ConfigurareRaport(dbContext, report);

            //        break;
                
            //}
        }


        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            reportDataSourceHelper.BeforeShowPreview -= ReportDataSourceHelper_BeforeShowPreview;
            base.OnDeactivated();

        }
    }
}
