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
    public class IntrareDetaliu : BaseObject
    {
        // Backing fields (public, cu litera mica)
        //public Iesire iesire;
        [VisibleInDetailView(false)]
        public Produs produs;
        [VisibleInDetailView(false)]
        public double cantitate;
        [VisibleInDetailView(false)]
        public double valoare;

        [VisibleInDetailView(false)] 
        public Intrare IdIntrari;

        //[RuleRequiredField(DefaultContexts.Save)]
        //public virtual Intrare Intrare { get; set; }
        [RuleRequiredField(DefaultContexts.Save)]
        public virtual Produs Produs
        {
            get => produs;
            set
            {
                if (produs != value)
                {
                    produs = value;
                    // Aici poti adauga cod de validare / notificari (daca este cazul)
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

        [ModelDefault("AllowEdit", "False")]
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