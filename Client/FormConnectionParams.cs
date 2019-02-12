using System.Windows.Forms;

namespace ProgramPlannerClient
{   
    /// <summary>
    /// Форма изменения параметров подключения к серверу
    /// </summary>
    public partial class FormConnectionParams : Form
    {
        public FormConnectionParams()
        {
            InitializeComponent();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
