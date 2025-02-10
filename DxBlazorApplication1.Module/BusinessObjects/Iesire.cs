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
    public class Iesire : BaseObject
    {
        [RuleRequiredField("ValidationIesireNumar", DefaultContexts.Save, "Va rugam completati numarul.")]
        //[ModelDefault("AllowEdit", "False")]
        public virtual long? Numar { get; set; }

        [RuleRequiredField("ValidationIesireData", DefaultContexts.Save, "Va rugam completati data.")]
        public virtual DateTime? Data{ get; set; }
        public virtual Gestiune Gestiune { get; set; }

        [DisplayName("Detalii")]
        [Aggregated]
        public virtual ObservableCollection<IesireDetaliu> IesireDetalii {  get; set; } = new ObservableCollection<IesireDetaliu> { };

        /// <summary>
        /// Metoda care se apeleaza cand creez un obiect nou si ii setez implicit campuri
        /// </summary>
        public override void OnCreated()
        {
            #region InitializareNumar
            long? numarMaxim = 0;
            var query = ObjectSpace.GetObjectsQuery<Iesire>().Where(i => i.Data.Value.Year == DateTime.Now.Year);
            
            if (query != null)
            {
                if (query.Count() == 0) // daca nu am iesiri in anul curent
                    numarMaxim = 1; // voi lua de la capat numerotarea lor
                else numarMaxim = query.Max(i => i.Numar) + 1; // altfel, este numarul ultimei intrari + 1
            }

            Numar = numarMaxim;

            #endregion

            #region InitializareData

            Data = DateTime.Now;

            #endregion

            base.OnCreated();
        }

        //public override void OnSaving()
        //{

        //    if (ObjectSpace.GetObjectsQuery<Iesire>()
        //        .Where(i => i.Data.Value.Year == DateTime.Now.Year)
        //        .Where(i => i.Numar == this.Numar)
        //        .Where(i => i.ID != ID)
        //        .Count() > 0)
        //    {
        //        Numar = ObjectSpace.GetObjectsQuery<Iesire>()
        //            .Where(i => i.Data.Value.Year == DateTime.Now.Year)
        //            .Where(i => i.ID != ID)
        //            .Max(i => i.Numar) + 1;
        //    }
        //    base.OnSaving();
        //}
    }
}