using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.ComponentModel.DataAnnotations;

namespace DxBlazorApplication1.Module.BusinessObjects
{
    public class IesireDetaliu : BaseObject
    {
        // backing fields
        [VisibleInDetailView(false)]
        public Iesire iesire;
        [VisibleInDetailView(false)]
        public Produs produs;
        [VisibleInDetailView(false)]
        public double cantitate;
        [VisibleInDetailView(false)]
        public double valoare;

        [System.ComponentModel.DataAnnotations.Required]
        public virtual Iesire Iesire
        {
            get => iesire;
            set
            {
                if (iesire != value)
                {
                    iesire = value;
                    // aici mai adaug validari etc
                }
            }
        }

        public virtual Produs Produs
        {
            get => produs;
            set
            {
                if (produs != value)
                {
                    produs = value;
                    Valoare = cantitate * (produs?.PretUnitar ?? 0);
                }
            }
        }

        public virtual double Cantitate
        {
            get => cantitate;
            set
            {
                if (cantitate != value)
                {
                    cantitate = value;
                    Valoare = cantitate * (produs?.PretUnitar ?? 0);
                }
            }
        }

        //[ModelDefault("AllowEdit", "False")]
        public virtual double Valoare
        {
            get => valoare;
            set
            {
                if (valoare != value)
                {
                    valoare = value;
                }
            }
        }
    }
}
