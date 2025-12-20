using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestePraticoDevCSharp.UI.Navigation
{
    public class FormNavigator
    {
        private Form _currentForm;
        private readonly Panel _container;

        public FormNavigator(Panel container)
        {
            _container = container;
        }

        public void Load(Form form)
        {
            if (_currentForm != null)
            {
                _currentForm.Close();
                _currentForm.Dispose();
            }

            _currentForm = form;

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            _container.Controls.Clear();
            _container.Controls.Add(form);

            form.Show();
        }
    }
}
