using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TechTest
{
    class DataFormat
    {
        public struct DataMemb
        {
            string gridRef;

            List<string[]> Data;

            public DataMemb(string pGridRef, List<String[]> pData)
            {
                gridRef = pGridRef;
                Data = pData;
            }

            public List<string[]> GetData()
            {
                return Data;
            }

            public string GetGridRef()
            {
                return gridRef;
            }
        }

        private string FileInfo;
        private string FileType;
        private string Line3;
        private string LongLatGrid;
        private string BoxYearMultiMissing;
        private List<DataMemb> DataList;

        public DataFormat()
        {
            DataList = new List<DataMemb>();
        }

        public string GetFileInfo()
        {
            return FileInfo;
        }

        public void SetFileInfo(string pInput)
        {
            FileInfo = pInput;
        }

        public string GetFileType()
        {
            return FileType;
        }

        public void SetFileType(string pInput)
        {
            FileType = pInput;
        }

        public string GetLine3()
        {
            return Line3;
        }

        public void SetLine3(string pinput)
        {
            Line3 = pinput;
        }

        public string GetLongLatGrid()
        {
            return LongLatGrid;
        }

        public void SetLongLatGrid(string pInput)
        {
            LongLatGrid = pInput;
        }

        public string GetBoxYearMultiMissing()
        {
            return BoxYearMultiMissing;
        }

        public void SetBoxYearMultiMissing(string pInput)
        {
            BoxYearMultiMissing = pInput;
        }

        public List<DataMemb> GetDataList()
        {
            return DataList;
        }

        public void SetDataList(List<DataMemb> pInput)
        {
            DataList = pInput;
        }

        public void AddToDataList(DataMemb pInput)
        {
            DataList.Add(pInput);
        }

        public int GetDataLength()
        {
            return DataList.Count();
        }
    }
}
