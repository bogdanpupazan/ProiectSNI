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
    [DefaultProperty(nameof(Numar))]
    public class Intrare : BaseObject
    {
        [RuleRequiredField("ValidationIntrareNumar", DefaultContexts.Save, "Va rugam completati numarul.")]
        public virtual long? Numar { get; set; }
        [RuleRequiredField("ValidationIntrareData", DefaultContexts.Save, "Va rugam completati data.")]
        public virtual DateTime? Data { get; set; } = DateTime.Now;
        public virtual Partener Partener { get; set; }
        public virtual Gestiune Gestiune { get; set; }
        public override void OnCreated()
        {
            long? numarMaxim = 0;
            var query = ObjectSpace.GetObjectsQuery<Intrare>().Where(i => i.Data.Value.Year == DateTime.Now.Year);

            if (query != null)
            {
                if (query.Count() == 0) 
                    numarMaxim = 1; 
                else numarMaxim = query.Max(i => i.Numar) + 1; 
            }

            Numar = numarMaxim;
           
            //Data = DateTime.Now;
            base.OnCreated();
        }
        [DisplayName("Detalii")]
        [Aggregated]
        public virtual ObservableCollection<IntrareDetaliu> IntrareDetalii { get; set; } = new ObservableCollection<IntrareDetaliu> { };
    }
}