using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Xpo.Helpers;
using DxBlazorApplication1.Module.BusinessObjects;

namespace DxBlazorApplication1.Blazor.Server;

[ToolboxItemFilter("Xaf.Platform.Blazor")]
// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
public sealed class DxBlazorApplication1BlazorModule : ModuleBase {
    public DxBlazorApplication1BlazorModule() {
    }
    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
        return ModuleUpdater.EmptyModuleUpdaters;
    }
    public override void Setup(XafApplication application) {
        base.Setup(application);
        application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
    }

    private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
    {
        if (e.ObjectSpace is NonPersistentObjectSpace)
        {
            IObjectSpace additionalObjectSpace = Application.CreateObjectSpace(typeof(Intrare));
            ((NonPersistentObjectSpace)e.ObjectSpace).AdditionalObjectSpaces.Add(additionalObjectSpace);
            e.ObjectSpace.Disposed += (s, args) =>
            {
                additionalObjectSpace.Dispose();
            };
        }
    }
}
