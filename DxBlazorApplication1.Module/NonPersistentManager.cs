using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DxBlazorApplication1.Module.NonPersistent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DxBlazorApplication1.Module
{
    public class NonPersistentManager
    {

        public static void NonPersistentObjectSpace_ObjectByKeyGetting(object sender, ObjectByKeyGettingEventArgs e)
        {
            if (e.ObjectType == typeof(CustomSituatieNonPersistent))
            {
                e.Object = new CustomSituatieNonPersistent()
                {
                };
            }
        }


        public static void NonPersistentObjectSpace_ObjectsGetting(object sender, ObjectsGettingEventArgs e)
        {
            // Adaugare obiecte non-persistent CNAS_ExportTipNP
            if (e.ObjectType == typeof(CustomSituatieNonPersistent))
            {
                BindingList<CustomSituatieNonPersistent> objects = new BindingList<CustomSituatieNonPersistent>();
                e.Objects = objects;
            }
          
        }
    }
}
