using System;

namespace fuzhu
{
    public partial class FormReportDesigner : DevExpress.XtraEditors.XtraForm
    {
        DevExpress.XtraReports.UI.XtraReport _Report = null;

        public FormReportDesigner(DevExpress.XtraReports.UI.XtraReport report)
        {
            InitializeComponent();
            _Report = report;
        }

        private void FormReportDesigner_Load(object sender, EventArgs e)
        {
            this.designMain.OpenReport(_Report);
        }

    }
}
