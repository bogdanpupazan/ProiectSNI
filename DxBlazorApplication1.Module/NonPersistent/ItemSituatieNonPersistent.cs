using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DxBlazorApplication1.Module.NonPersistent

{
    [DomainComponent]
    public class ItemSituatieNonPersistent: NonPersistentBaseObject, IXafEntityObject
    {
        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]
        public virtual Guid ID { get; set; }
        public virtual int NumarMiscare { get; set; }
        public virtual DateTime DataMiscare { get; set; }
        public virtual string DenumireProdus { get; set; }
        public virtual int Cantitate { get; set; }
        public virtual int Valoare { get; set; }
        public virtual int Total { get; set; }
        public virtual string DenumireGestiune { get; set; }

    }
}
