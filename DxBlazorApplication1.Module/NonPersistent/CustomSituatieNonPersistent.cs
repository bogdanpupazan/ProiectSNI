using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DxBlazorApplication1.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DxBlazorApplication1.Module.NonPersistent
{
    [DomainComponent]
    [DefaultClassOptions]
    public class CustomSituatieNonPersistent : NonPersistentBaseObject, IXafEntityObject
    {
        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]
        public virtual Guid ID { get; set; }
        public virtual DateTime DataInceput {  get; set; }
        public virtual DateTime DataSfarsit { get; set; }
        /*
        public virtual bool ToateGestiunile { get; set; }
        public virtual bool OGestiune {  get; set; }
        */

        private bool _toateGestiunile;
        public virtual bool ToateGestiunile
        {
            get { return _toateGestiunile; }
            set
            {
                if (_toateGestiunile != value)
                {
                    _toateGestiunile = value;
                    // deselectez OGestiune
                    if (value)
                        OGestiune = false;
                    OnPropertyChanged(nameof(ToateGestiunile));
                }
            }
        }

        private bool _oGestiune;
        public virtual bool OGestiune
        {
            get { return _oGestiune; }
            set
            {
                if (_oGestiune != value)
                {
                    _oGestiune = value;
                    // deselectez ToateGestiunile
                    if (value)
                        ToateGestiunile = false;
                    OnPropertyChanged(nameof(OGestiune));
                }
            }
        }
        [Appearance("HideGestiuneIfNotOGestiune", Criteria = "OGestiune = False", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "DetailView")]
        public virtual Gestiune Gestiune { get; set; }
        public virtual TipAfisare TipAfisare { get; set; }


    }
    public enum TipAfisare
    {
        Intrari,
        Iesiri
    }
}
