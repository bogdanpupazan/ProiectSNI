using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxBlazorApplication1.Module.BusinessObjects;

namespace DxBlazorApplication1.Module.Rapoarte
{
    public class SituatieRPO : ReportParametersObjectBase
    {
        public string TipSituatie { get; set; }
        public DateTime DataInceput { get; set; }
        public DateTime DataSfarsit { get; set; }

        public SituatieRPO(IObjectSpaceCreator provider) : base(provider)
        {
        }

        protected override IObjectSpace CreateObjectSpace()
        {
            return objectSpaceCreator.CreateObjectSpace(typeof(Intrare));
        }

        public override CriteriaOperator GetCriteria()
        {
            return CriteriaOperator.Parse("");
        }

        public override SortProperty[] GetSorting()
        {
            return Array.Empty<SortProperty>();
        }

        public override string ToString()
        {
            return "SituatieRPO";
        }
    }
}
