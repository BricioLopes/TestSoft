using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSoft.Model;

namespace TestSoft.Control
{
    public class Manager
    {
        public void push()
        {
            //method for get informations in BOMM table
        }

        public async Task<ObservableCollection<bomm>> pushOfExcel()
        {
            ObservableCollection<bomm> db = new ObservableCollection<bomm>();

            try
            {
                ImportExcel imp = new ImportExcel();
                db = imp.getContent_EXCEL();
            }
            catch (Exception e)
            {
                await MainWindow.Instance.ShowMessageAsync("Error", e.Message);

            }
            return db;
        }

        public async Task<ObservableCollection<bomm>> pushofDB()
        {
            ObservableCollection<bomm> db = new ObservableCollection<bomm>();

            try
            {
                using (var ctx = new dbcontext())
                {
                    db = new ObservableCollection<bomm>(ctx.Set<bomm>().ToList());

                }
            }
            catch (Exception e)
            {
                await MainWindow.Instance.ShowMessageAsync("Error", e.Message);
            }
            return db;
        }

        public async void saveDB(ObservableCollection<bomm> db)
        {
            try
            {
                using (var ctx = new dbcontext())
                {
                    foreach (var item in db)
                    {
                        bomm b = new bomm();
                        b.bom_level = item.bom_level;
                        b.Parent_Part_Number = item.Parent_Part_Number;
                        b.Part_Number = item.Part_Number;
                        b.Part_Name = item.Part_Name;
                        b.Revision = item.Revision;
                        b.Quantit = item.Quantit;
                        b.Unit_of_measure = item.Unit_of_measure;
                        b.Procurement_Type = item.Procurement_Type;
                        b.Reference_Designatos = item.Reference_Designatos;
                        b.BOM_Notes = item.BOM_Notes;

                        ctx.bomm.Add(b);

                    }
                    ctx.SaveChanges();
                }

            }
            catch (Exception e)
            {
                await MainWindow.Instance.ShowMessageAsync("Error", e.Message);
            }
        }

        public void exportTOExcell(ObservableCollection<bomm> db)
        {
            ImportExcel imp = new ImportExcel();
            imp.exportToEXCEL(db);

        }
    }
}

