using System;
using System.Windows.Forms;
using System.IO;

namespace fuzhu
{
    public partial class FormReportViewer : DevExpress.XtraEditors.XtraForm
    {
        DevExpress.XtraReports.UI.XtraReport _Report = null;
        
        private string _ReportPath = string.Empty;
        private object _DataSource = null;
        public event EventHandler AfterPrint;

        public FormReportViewer(DevExpress.XtraReports.UI.XtraReport report, object datasource)
        {
            InitializeComponent();
            _Report = report;
            _DataSource = datasource;
        }

        public FormReportViewer(string reportFile, object datasource)
        {
            InitializeComponent();
            _ReportPath = reportFile;
            _DataSource = datasource;
        }

        private void RebindReport()
        {
            //析构旧Report
            //if (_Report != null) _Report.Dispose();

            //文件加载模式需创建Report对象
            if (!string.IsNullOrEmpty(_ReportPath))
            {
                if (!File.Exists(_ReportPath)) return;
                FileStream stream = new FileStream(_ReportPath, FileMode.Open);
                _Report = DevExpress.XtraReports.UI.XtraReport.FromStream(stream, true);
                stream.Close();
            }
            _Report.AfterPrint += new EventHandler(_Report_AfterPrint);
            _Report.DataSource = _DataSource;
            printMain.PrintingSystem = _Report.PrintingSystem;
            _Report.CreateDocument();
        }

        void _Report_AfterPrint(object sender, EventArgs e)
        {
            if (AfterPrint != null)
                AfterPrint(sender,e);
        }

        private void FormReportViewer_Load(object sender, EventArgs e)
        {
            RebindReport();
        }

        private void FormReportViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                FormReportDesigner frmDesigner = new FormReportDesigner(_Report);
                frmDesigner.ShowDialog();
                RebindReport();
            }
        }

        private void printPreviewBarItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

    }
}
