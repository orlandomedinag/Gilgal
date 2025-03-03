using BoldReports.Web;
using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.IO;

namespace GilgalInventar.Controllers
{
    [Route("api/{controller}/{action}/{id?}")]
    public class BoldReportsAPIController : ControllerBase, IReportController
    {
        // Report Viewer requires a memory cache to store the information of consecutive client requests and
        // the rendered Report Viewer in the server.
        private IMemoryCache _cache;

        // IHostingEnvironment used with sample to get the application data from wwwroot.
        private IWebHostEnvironment _hostingEnvironment;


        public BoldReportsAPIController(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }


        //Get action for getting resources from the report
        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        // Method will be called from Report Viewer client to get the image src for Image report item.
        public object GetResource(ReportResource resource)
        {
            //resource.isPrint = true;
            return ReportHelper.GetResource(resource, this, _cache);
        }


        // Method will be called to initialize the report information to load the report with ReportHelper for processing.
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            string basePath = _hostingEnvironment.WebRootPath;
            reportOption.ReportModel.DataSourceCredentials.Add(new DataSourceCredentials("DefaultConnection", "sa", "A6minC3nt@ur*5"));//Conection string

            // Here, we have loaded the sales-order-detail.rdl report from the application folder wwwroot\Resources. sales-order-detail.rdl should be in the wwwroot\Resources application folder.
            //System.IO.FileStream reportStream = new System.IO.FileStream(basePath + @"\resources\" + reportOption.ReportModel.ReportPath + ".rdl", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            //reportOption.ReportModel.Stream = reportStream;
            //reportOption.ReportModel.EmbedImageData = true;
            System.IO.FileStream inputStream = new System.IO.FileStream(basePath + @"\resources\" + reportOption.ReportModel.ReportPath + ".rdl", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;
            inputStream.Close();
            reportOption.ReportModel.Stream = reportStream;
        }


        // Method will be called when report is loaded internally to start the layout process with ReportHelper.
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }


        [HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, _cache);
        }

        // Post action to process the report from the server based on json parameters and send the result back to the client.
        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonArray)
        {
            return ReportHelper.ProcessReport(jsonArray, this, this._cache);
        }
    }
}
