using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSoft.Model;


using System.Data;
using System.Reflection;
using NetOffice.ExcelApi.Enums;
using Excel = NetOffice.ExcelApi;
using Microsoft.Win32;

namespace TestSoft.Control
{
    public class ImportExcel
    {
        string pivo0, pivo1, pivo2, pivo3 ,pivo4 = "";
        //method for get content of excel file
        public ObservableCollection<bomm> getContent_EXCEL()
        {
            //list for return 
            ObservableCollection<bomm> db = new ObservableCollection<bomm>();

            //window for select excel file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xls)|*.xls";
            openFileDialog.ShowDialog();

            string local = openFileDialog.FileName;

            if (local != String.Empty)
            {
                //using library for acess excel file
                var application = new Excel.Application();
                Excel.Workbook book = application.Workbooks.Open(local);
                Excel.Worksheet mainSheet = (Excel.Worksheet)book.Sheets[4];

                DataTable tb = new DataTable();

                //getting excel dates and saving in collection
                foreach (var item in mainSheet.UsedRange.Rows)
                {
                    Excel.Range b = item;
                    List<string> str = new List<string>();

                    foreach (Excel.Range cell in item.Columns)
                    {
                        str.Add(cell.Value + "");
                    }

                    bomm bomobj = new bomm();

                    bomobj.bom_level = str[0];                    
                    bomobj.Part_Number = str[2];
                    bomobj.Part_Name = str[3];
                    bomobj.Revision = str[4];
                    bomobj.Quantit = str[5];
                    bomobj.Unit_of_measure = str[6];
                    bomobj.Procurement_Type = str[7];
                    bomobj.Reference_Designatos = str[8];
                    bomobj.BOM_Notes = str[9];

                    str.Clear();
                    db.Add(bomobj);
                }
                db.RemoveAt(0);
                db.RemoveAt(0);
                db.RemoveAt(61);
                db.RemoveAt(61);

                //CleanUp
                application.ActiveWorkbook.Close();
                application.Quit();
                application.Dispose();

            }

            

            foreach (var item in db)
            {

                switch (item.bom_level)
                {
                    case "0":
                        pivo0 = item.Part_Number;
                        break;
                    case "1":
                        pivo1 = item.Part_Number;
                        break;
                    case "2":
                        pivo2 = item.Part_Number;
                        break;
                    case "3":
                        pivo3 = item.Part_Number;
                        break;
                    case "4":
                        pivo4 = item.Part_Number;
                        break;
                    default:
                        {                           
                            break;
                        }
                }

                switch (item.bom_level)
                {
                    case "1":
                        item.Parent_Part_Number = pivo0;
                        break;
                    case "2":
                        item.Parent_Part_Number = pivo1;
                        break;
                    case "3":
                        item.Parent_Part_Number = pivo2;
                        break;
                    case "4":
                        item.Parent_Part_Number = pivo3;
                        break;
                    default:
                        {
                            break;
                        }
                }
            }
            return db;
        }

        //method for export content for excel file
        //Generate excell
        public Boolean exportToEXCEL(ObservableCollection<bomm> dblist)
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable();
            dataTable.TableName = "dataTable";
            dataSet.Tables.Add(dataTable);

            // we assume that the properties of DataSourceVM are the columns of the table
            // you can also provide the type via the second parameter
            dataTable.Columns.Add("Property1");
            dataTable.Columns.Add("Property2");
            dataTable.Columns.Add("Property3");
            dataTable.Columns.Add("Property4");
            dataTable.Columns.Add("Property5");
            dataTable.Columns.Add("Property6");
            dataTable.Columns.Add("Property7");
            dataTable.Columns.Add("Property8");
            dataTable.Columns.Add("Property9");
            dataTable.Columns.Add("Property10");

            //Header
            var newRowHeader = dataTable.NewRow();

            // fill the properties into the cells
            newRowHeader["Property1"] = "BOM Level";
            newRowHeader["Property2"] = "Parent Part Number";
            newRowHeader["Property3"] = "Part Number";
            newRowHeader["Property4"] = "Part Name";
            newRowHeader["Property5"] = "Revision";
            newRowHeader["Property6"] = "Quantit";
            newRowHeader["Property7"] = "Unit of Measure";
            newRowHeader["Property8"] = "Procurement Type";
            newRowHeader["Property9"] = "Refereces Designators";
            newRowHeader["Property10"] = "BOM Notes";

            dataTable.Rows.Add(newRowHeader);

            foreach (var element in dblist)
            {
                var newRow = dataTable.NewRow();

                // fill the properties into the cells
                newRow["Property1"] = element.bom_level;
                newRow["Property2"] = element.Parent_Part_Number;
                newRow["Property3"] = element.Part_Number;
                newRow["Property4"] = element.Part_Name;
                newRow["Property5"] = element.Revision;
                newRow["Property6"] = element.Quantit;
                newRow["Property7"] = element.Unit_of_measure;
                newRow["Property8"] = element.Procurement_Type;
                newRow["Property9"] = element.Reference_Designatos;
                newRow["Property10"] = element.BOM_Notes;

                dataTable.Rows.Add(newRow);
            }


            //Create excel application
            var application = new Excel.Application();
            //Add workbook
            application.Workbooks.Add();

            application.DisplayAlerts = false;

            //Get all data into an array
            var tempArray = new object[dataSet.Tables["dataTable"].Rows.Count, dataSet.Tables["dataTable"].Columns.Count];
            for (var r = 0; r < dataSet.Tables["dataTable"].Rows.Count; r++)
            {
                for (var c = 0; c < dataSet.Tables["dataTable"].Columns.Count; c++)
                    tempArray[r, c] = dataSet.Tables["dataTable"].Rows[r][c];
            }

            //Get column names into an array
            var tempHeadingArray = new object[dataSet.Tables["dataTable"].Columns.Count];
            for (var i = 0; i < dataSet.Tables["dataTable"].Columns.Count; i++)
            {
                //tempHeadingArray[i] = dataSet.Tables["dataTable"].Columns[i].ColumnName;
            }

            //Get active worksheet
            var sheet = (Excel.Worksheet)application.ActiveSheet;

            //AddColumnNames(sheet, tempHeadingArray);

            AddExcelHeadingText(sheet);

            AddDataRows(sheet, dataSet, tempArray);

            sheet.Columns.AutoFit();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excell Files|*.xlsx";
            saveFileDialog1.Title = "Save an Excell File";
            saveFileDialog1.ShowDialog();
            string path = saveFileDialog1.FileName;
            //"E:\\Sampledocument"
            if (path != string.Empty && path != null)
            {
                application.ActiveWorkbook.SaveAs(path, Missing.Value, Missing.Value, Missing.Value, false,
                                              false, XlSaveAsAccessMode.xlExclusive);
            }

            //CleanUp
            application.ActiveWorkbook.Close();
            application.Quit();
            application.Dispose();



            return true;
        }

        private static void AddDataRows(Excel.Worksheet sheet, DataSet dataset, object[,] tempArray)
        {
            var range = sheet.Range(sheet.Cells[1, 1],
                            sheet.Cells[(dataset.Tables["dataTable"].Rows.Count), (dataset.Tables["dataTable"].Columns.Count)]);
            sheet.Name = "Relatório";
            range.Value = tempArray;
        }

        private static void AddColumnNames(Excel.Worksheet sheet, object[] tempHeadingArray)
        {
            var columnNameRange = sheet.get_Range(sheet.Cells[3, 3], sheet.Cells[3, tempHeadingArray.Length + 2]);
            columnNameRange.Style = "NewStyle";
            columnNameRange.Value = tempHeadingArray;
            columnNameRange.UseStandardWidth = true;
        }

        private static void AddExcelHeadingText(Excel.Worksheet sheet)
        {            
            sheet.Cells[1, 1].Font.Bold = true;
            sheet.Cells[1, 2].Font.Bold = true;
            sheet.Cells[1, 3].Font.Bold = true;
            sheet.Cells[1, 4].Font.Bold = true;
            sheet.Cells[1, 5].Font.Bold = true;
            sheet.Cells[1, 6].Font.Bold = true;
            sheet.Cells[1, 7].Font.Bold = true;
            sheet.Cells[1, 8].Font.Bold = true;
            sheet.Cells[1, 9].Font.Bold = true;
            sheet.Cells[1, 10].Font.Bold = true;            
        }

    }
}
