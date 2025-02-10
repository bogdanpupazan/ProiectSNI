using DevExpress.CodeParser;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using DevExpress.XtraRichEdit.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DxBlazorApplication1.Module.BusinessObjects
{
    [DefaultProperty(nameof(Denumire))]
    public class Produs : BaseObject
    {
        [RuleRequiredField("ValidationProdusCod",DefaultContexts.Save,"Va rugam specificati un cod.")]
        public virtual int? Cod { get; set; }
        [RuleRequiredField("ValidationProdusDenumire", DefaultContexts.Save, "Va rugam specificati o denumire.")]
        public virtual string Denumire { get; set; }
        public virtual double PretUnitar { get; set; }

        public override void OnCreated()
        {
            if (ObjectSpace.IsNewObject(this))
            {
                var maxCod = ObjectSpace.GetObjectsQuery<Produs>().Max(p => (int?)p.Cod);
                Cod = (maxCod ?? 0) + 1;
            }

            base.OnCreated();
        }

        

    }
}
