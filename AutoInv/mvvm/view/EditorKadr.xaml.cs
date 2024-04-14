using AutoKadr.mvvm.model;
using AutoKadr.mvvm.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoKadr.mvvm.viewmodel;
using AutoKadr.mvvm.model;

namespace AutoKadr.mvvm.view
{
    public partial class EditorKadr : Page
    {
        public EditorKadr(viewmodel.MainVM mainVM)
        {
            InitializeComponent();
            var vm = ((EditorKadrVM)DataContext);
            vm.SetMainVM(mainVM, listPosts);
        }

        public EditorKadr(MainVM mainVM, Kadr selectedKadr) : this(mainVM)
        {
            ((EditorKadrVM)DataContext).SetEditDrink(selectedKadr);
        }
    }
}
