using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DxBlazorApplication1.Module.BusinessObjects
{
    [DefaultProperty(nameof(Denumire))]
    public class Gestiune : BaseObject
    {
        [RuleRequiredField("ValidationGestiuneCod", DefaultContexts.Save, "Va rugam specificati un cod.")]
        public virtual int? Cod {  get; set; }
        [RuleRequiredField("ValidationGestiuneDenumire", DefaultContexts.Save, "Va rugam specificati o denumire.")]
        public virtual string Denumire { get; set; }

        public override void OnCreated()
        {
            if (ObjectSpace.IsNewObject(this))
            {
                var maxCod = ObjectSpace.GetObjectsQuery<Gestiune>().Max(p => (int?)p.Cod);
                Cod = (maxCod ?? 0) + 1;
            }

            base.OnCreated();
        }
    }
}
