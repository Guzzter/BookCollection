using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BookCollection.Helpers
{
    public enum FileStamper
    {
        WithDate,
        WithDateTime,
        None
    }

    public class DownloadExcelFileActionResult : ActionResult
    {
        public GridView ExcelGridView { get; set; }
        public string FileName { get; set; }
        public FileStamper Stamp { get; set; }

        public DownloadExcelFileActionResult(GridView gv, string fileName, FileStamper filestamp = FileStamper.None)
        {
            ExcelGridView = gv;
            FileName = fileName;
            Stamp = filestamp;
        }

        public override void ExecuteResult(ControllerContext context)
        {

            HttpContext curContext = HttpContext.Current;
            curContext.Response.Clear();
            switch (Stamp)
            {
                case FileStamper.WithDate:
                    curContext.Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileNameWithoutExtension(FileName) + "-" + DateTime.Now.ToString("yyyyMMdd") + Path.GetExtension(FileName));
                    break;
                case FileStamper.WithDateTime:
                    curContext.Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileNameWithoutExtension(FileName) + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(FileName));
                    break;
                default:
                    curContext.Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                    break;
            }

            curContext.Response.Charset = "";
            curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            curContext.Response.ContentType = "application/vnd.ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            ExcelGridView.RenderControl(htw);

            byte[] byteArray = Encoding.UTF8.GetBytes(sw.ToString());
            MemoryStream s = new MemoryStream(byteArray);
            StreamReader sr = new StreamReader(s, Encoding.UTF8);

            curContext.Response.Write(sr.ReadToEnd());
            curContext.Response.End();
        }

    }
}