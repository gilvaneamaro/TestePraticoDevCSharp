using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestePraticoDevCSharp.App.Interfaces
{
    public interface IFormNavigator
    {
        void Navigate<TForm>() where TForm : Form;

        void Navigate<TForm>(Action<TForm> configure) where TForm : Form;
    }
}
