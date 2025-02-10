using DxBlazorApplication1.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;

namespace DxBlazorApplication1.Module.Rapoarte.Interfaces
{
    public interface IRaportManager<TDTO, TParams>
    {
        void ConfigurareRaport(DxBlazorApplication1EFCoreDbContext dbContext, XtraReport report);

        TDTO ConstruiesteDataSource(DxBlazorApplication1EFCoreDbContext dbContext, TParams reportParameterObject);
    }
}
