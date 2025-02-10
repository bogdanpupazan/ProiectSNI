using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DxBlazorApplication1.Module.BusinessObjects
{
    public enum TipPartener
    {
        [Description("Furnizor")] Furnizor,
        [Description("Client")] Client
    }

    [DefaultProperty(nameof(Denumire))]
    public class Partener : BaseObject
    {
        [RuleRequiredField("ValidationParteneriCod", DefaultContexts.Save, "Va rugam specificati un cod")]
        public virtual int? Cod { get; set; }
        [RuleRequiredField("ValidationParteneriDenumire", DefaultContexts.Save, "Va rugam specificati o denumire.")]
        public virtual string Denumire { get; set; }
        //[ModelDefault("AllowEdit", "False")]
        [RuleRequiredField("ValidationParteneriTip", DefaultContexts.Save, "Va rugam specificati un tip de partener.")]
        public virtual TipPartener? TipPartener { get; set; }
        public virtual string CUI { get; set; }
        public virtual string Adresa { get; set; }

        public override void OnCreated()
        {
            if (ObjectSpace.IsNewObject(this))
            {
                var maxCod = ObjectSpace.GetObjectsQuery<Partener>().Max(p => (int?)p.Cod);
                Cod = (maxCod ?? 0) + 1;
            }
            base.OnCreated();
        }
    }
}