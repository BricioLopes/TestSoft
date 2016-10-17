using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using TestSoft.Control;
using TestSoft.Model;

namespace TestSoft.View
{
    /// <summary>
    /// Interaction logic for Output.xaml
    /// </summary>
    public partial class Output : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<bomm> db = new ObservableCollection<bomm>();
        private ObservableCollection<bomm> originalExcel = new ObservableCollection<bomm>();       

        public ObservableCollection<bomm> Db
        {
            get
            {
                return db;
            }
            set
            {
                db = value;
                OnPropertyChanged("Db");
            }
        }

        //Method for function notity property changed
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }

        }

        public Output()
        {
            InitializeComponent();
            DataContext = this;      
        }
        
        //get data of database and show in datagrid
        private async void REFRESH_BUTTON(object sender, RoutedEventArgs e)
        {
            Manager m = new Manager();
            Db = await m.pushofDB();
            
        }

        //open a excel file and show in datagrid
        private async void OPEN_BUTTON(object sender, RoutedEventArgs e)
        {
            Manager m = new Manager();
            var ctrl = await MainWindow.Instance.ShowProgressAsync("Loading Excel", "Aguarde um momento...");           
            Db = await m.pushOfExcel();
            originalExcel = Db;
            await ctrl.CloseAsync();
        }

        //save datagrid data in database
        private void SAVE_DATABASE_BUTTON(object sender, RoutedEventArgs e)
        {
            Manager m = new Manager();
            m.saveDB(Db);
        }

        private void SAVE_EXCEL_BUTTON(object sender, RoutedEventArgs e)
        {
            Manager m = new Manager();
            m.exportTOExcell(Db);
        }

        private void BUSCAR_BUTTON(object sender, RoutedEventArgs e)
        {
            
            if (input_textbox.Text != String.Empty)
            {                
                Db = new ObservableCollection<bomm>(originalExcel.Where(b => b.Part_Number.Contains(input_textbox.Text)).ToList());
            }
            else
            {
                Db = originalExcel;
            }
        }

        private void input_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            BUSCAR_BUTTON(null, null);
            
        }
    }
}
