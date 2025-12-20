using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestePraticoDevCSharp.App.Interfaces;

namespace TestePraticoDevCSharp.UI.Navigation
{
    public class FormNavigator : IFormNavigator
    {
        private Form _currentForm;
        private readonly Panel _container;
        private readonly IServiceProvider _serviceProvider;

        public FormNavigator(Panel container, IServiceProvider serviceProvider)
        {
            _container = container;
            _serviceProvider = serviceProvider;
        }

        public void Navigate<TForm>() where TForm : Form
        {
            _currentForm?.Close();
            _currentForm?.Dispose();

            var form = _serviceProvider.GetRequiredService<TForm>();

            if (form is INavigable navigable)
                navigable.SetNavigator(this);

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
