using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace TechTest
{
    class Program
    {
        
        static void Main(string[] args)
        {          

            DataFormat data = new DataFormat();
            DataLayout layout = new DataLayout();
            string filename = "cru-ts-2-10.1991-2000-cutdown.pre";
            Console.WriteLine("Hello! Please enter file name. If left blank file with default to data given.");
            string tempRead = Console.ReadLine();
            if (tempRead != "")
            {
                filename = tempRead;
            }
            StreamReader reader = new StreamReader(filename);

            data.SetFileInfo(reader.ReadLine());
            data.SetFileType(reader.ReadLine());
            data.SetLine3(reader.ReadLine());
            data.SetLongLatGrid(reader.ReadLine());
            data.SetBoxYearMultiMissing(reader.ReadLine());

            int yearDiff = GetYearDiff(data.GetBoxYearMultiMissing());
            int[] years = GetYears(data.GetBoxYearMultiMissing());
            bool loop = true;
            string GridRef = "";
            
            do
            {
                GridRef = reader.ReadLine();
                if (GridRef != null)
                {
                    data.AddToDataList(ReadGridData(yearDiff, reader, GridRef));
                }
                else
                {
                    loop = false;
                }
            }
            while (loop == true);

            layout = FormatintoTable(data, years, yearDiff);

            UpdateDatabase(layout);

            Console.ReadKey();
        }

        private static int GetYearDiff(string pBoxYearMultiMissing)
        {
            string[] temp = pBoxYearMultiMissing.Split(']');
            string[] yearsTemp = temp[1].Split('=');
            string[] Years = yearsTemp[1].Split('-');
            Years[1] = Years[1].Trim(']');
            int yearDiff = int.Parse(Years[1]) - int.Parse(Years[0]);
            yearDiff++;
            return yearDiff;
        }

        private static int[] GetYears(string pBoxYearMultiMissing)
        {
            string[] temp = pBoxYearMultiMissing.Split(']');
            string[] yearsTemp = temp[1].Split('=');
            string[] Years = yearsTemp[1].Split('-');
            Years[1] = Years[1].Trim(']');
            int[] years = new int[2];
            years[0] = int.Parse(Years[0]);
            years[1] = int.Parse(Years[1]);
            return years;
        }


        public static DataFormat.DataMemb ReadGridData(int pYearDiff, StreamReader pReader, string pGridRef)
        {
            List<string[]> tempData = new List<string[]>();
            for(int i = 0; i < pYearDiff; i++)
            {
                string tempLine = pReader.ReadLine();
                string[] LineSplitArray = tempLine.Split(' ');
                string[] tempLineArray = new string[12];
                int count = 0;
                for (int j = 0; j < LineSplitArray.Length; j++)
                {
                    if (LineSplitArray[j].Count() > 5)
                    {
                        int split = LineSplitArray[j].Count() - 5;
                        string tempFirstValue = LineSplitArray[j].Substring(0, split);
                        tempLineArray[count] = tempFirstValue;
                        count++;
                        string tempSecondValue = LineSplitArray[j].Substring(split);
                        tempLineArray[count] = tempSecondValue;
                        count++;
                    }
                    else if (LineSplitArray[j] != "")
                    {
                        tempLineArray[count] = LineSplitArray[j];
                        count++;
                    }
                }
                tempData.Add(tempLineArray);
            }
            DataFormat.DataMemb tempDatameb = new DataFormat.DataMemb(pGridRef, tempData);
            return tempDatameb;
        }

        private static DataLayout FormatintoTable(DataFormat pdata, int[] pYears, int pYearDiff)
        {
            DataLayout table = new DataLayout();

            List<DataFormat.DataMemb> tempData = pdata.GetDataList();
            for(int i = 0; i < pdata.GetDataLength(); i++)
            {
                string tempGridRef = tempData[i].GetGridRef();
                string[] splitRef = tempGridRef.Split('=');
                string[] gridRefString = splitRef[1].Split(',');
                int[] gridRef = new int[2];
                gridRef[0] = int.Parse(gridRefString[0]);
                gridRef[1] = int.Parse(gridRefString[1]);

                List<string[]> gridData = tempData[i].GetData();

                for (int j = 0; j < pYearDiff; j++)
                {
                    string[] yearData = gridData[j];
                    for(int k = 0; k < 12; k++)
                    {

                        string date = (k + 1) + "/1/" + (pYears[0] + j);

                        table.AddtoXref(gridRef[0]);
                        table.AddtoYref(gridRef[1]);
                        table.AddtoDate(date);
                        table.AddtoValue(int.Parse(yearData[k]));
                    }
                }
            }
            return table;
        }

        public static void UpdateDatabase(DataLayout pLayout)
        {
            
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();

            DataColumn xRefColumn = new DataColumn();
            DataColumn yRefColumn = new DataColumn();
            DataColumn dateColumn = new DataColumn();
            DataColumn valueColumn = new DataColumn();

            DataRow row;

            List<int> xRefList = pLayout.GetXref();
            List<int> yRefList = pLayout.GetYref();
            List<string> dateList = pLayout.GetDate();
            List<int> ValueList = pLayout.GetValue();

            xRefColumn.DataType = Type.GetType("System.Int32");
            xRefColumn.ColumnName = "Xref";
            xRefColumn.ReadOnly = false;
            xRefColumn.Unique = false;
            

            yRefColumn.DataType = Type.GetType("System.Int32");
            yRefColumn.ColumnName = "Yref";
            yRefColumn.ReadOnly = false;
            yRefColumn.Unique = false;

            dateColumn.DataType = Type.GetType("System.String");
            dateColumn.ColumnName = "Date";
            dateColumn.ReadOnly = false;
            dateColumn.Unique = false;

            valueColumn.DataType = Type.GetType("System.Int32");
            valueColumn.ColumnName = "Value";
            valueColumn.ReadOnly = false;
            valueColumn.Unique = false;

            dataTable.Columns.Add(xRefColumn);
            dataTable.Columns.Add(yRefColumn);
            dataTable.Columns.Add(dateColumn);
            dataTable.Columns.Add(valueColumn);

            dataSet.Tables.Add(dataTable);

            for(int i = 0; i < xRefList.Count(); i++)
            {
                row = dataTable.NewRow();
                row["Xref"] = xRefList[i];
                row["Yref"] = yRefList[i];
                row["Date"] = dateList[i];
                row["Value"] = ValueList[i];
                dataTable.Rows.Add(row);
            }

            dataSet.AcceptChanges();
        }

    }
}
