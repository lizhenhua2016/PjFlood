using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using System.Data;

namespace GrassrootsFloodCtrl.Logic.Common
{
   public class CreateExcelModel
    {
        public string getExcelModel(ExcelModelAttribute ema)
        {
            string _excelpath = "";
            var path = System.Web.HttpContext.Current.Server.MapPath("~/" + ema.excelSavePath + "/" + ema.excelName + ".xls");
            HSSFWorkbook workBook = new HSSFWorkbook();
            createSheet1(workBook, ema.excelTableName,ema);
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                workBook.Write(file);//创建Excel文件。
                file.Close();
            }
            _excelpath = ema.excelSavePath + "/" + ema.excelName + ".xls";
            return _excelpath;
        }
        private ISheet createSheet1(HSSFWorkbook workBook, string sheetName,ExcelModelAttribute ema)
        {
            ISheet sheet = workBook.CreateSheet(sheetName);
            IRow RowHead = sheet.CreateRow(0);
            //合并第一行单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, ema.excelNoticeCols));
            
            RowHead.CreateCell(0).SetCellValue(ema.excelNotice);
            /*****单元格样式 start******/
            var cellStyleHead = workBook.CreateCellStyle();
            cellStyleHead.Alignment = HorizontalAlignment.Center;//居中显示
            cellStyleHead.VerticalAlignment = VerticalAlignment.Top;//垂直居中
            cellStyleHead.WrapText = true;
            //高度
            RowHead.HeightInPoints = 50; //2 * sheet.DefaultRowHeight / 10;
            //字体设置,字体要调用CreateFont()
            IFont fonthead = workBook.CreateFont();
            fonthead.FontHeightInPoints = 10;
            fonthead.Color = HSSFColor.OliveGreen.Red.Index;
            cellStyleHead.SetFont(fonthead);
            //这里调试出来的,样式一定要给到单元格才有效
            RowHead.Cells[0].CellStyle = cellStyleHead;
            /*****单元格样式 end******/
            /*********添加表头 s********/
            IRow RowBody = sheet.CreateRow(1);
            
            for (int iColumnIndex = 0; iColumnIndex < ema.tableHeadList.Count(); iColumnIndex++)
            {
                RowBody.CreateCell(iColumnIndex).SetCellValue(ema.tableHeadList[iColumnIndex].ToString());
                RowBody.Cells[iColumnIndex].Row.HeightInPoints = 20;
                //单元格样式
                ICellStyle cellStyle = workBook.CreateCellStyle();
                cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                //设置单元格上下左右边框线  
                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                //文字水平和垂直对齐方式  
                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = VerticalAlignment.Top;
                //是否换行
                cellStyle.WrapText = true;

                RowBody.Cells[iColumnIndex].CellStyle = cellStyle;
                //字体大小
                IFont cellfont = workBook.CreateFont();
                cellfont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellfont);
                sheet.SetColumnWidth(iColumnIndex, 20 * 256);
            }
            /*********添加表头 e********/
            //设置下拉框
            setSheet2(workBook, sheet, ema);
            return sheet;
        }
        private void setSheet2(HSSFWorkbook workBook, ISheet sheet, ExcelModelAttribute ema)
        {
            ////创建表
            //ISheet sheet2 = workBook.CreateSheet("岗位数据");
            ////隐藏
            //workBook.SetSheetHidden(1, true);
            ////取数据
            //for (int iRowIndex = 0; iRowIndex < rlist.Count; iRowIndex++)
            //{
            //    sheet2.CreateRow(iRowIndex).CreateCell(0).SetCellValue(rlist[iRowIndex].PostName);
            //}
            ////设计表名称
            //IName range = workBook.CreateName();
            //range.RefersToFormula = "岗位数据!$A:$A";
            //range.NameName = "PostDataName";
            ////定义下拉框范围
            //CellRangeAddressList regions = new CellRangeAddressList(2, 65535, 1, 1);
            ////设置数据引用
            //DVConstraint constraint = DVConstraint.CreateFormulaListConstraint("PostDataName");
            //HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
            //sheet.AddValidationData(dataValidate);
        }
        public class ExcelModelAttribute
        {
            //excel表名 村级防汛防台工作组
            public string excelTableName { get; set; }
            //excel保存路径 Files/workinggroup
            public string excelSavePath { get; set; }
            //excel文件名称 201701201233
            public string excelName { get; set; }
            /// <summary>
            /// 表头提醒单元格合并
            /// </summary>
            public int excelNoticeCols { get; set; }
            /// <summary>
            /// 表提醒
            /// </summary>
            public string excelNotice { get; set; }
            /// <summary>
            /// TableHeads 表头名
            /// </summary>
            public List<TableHeads> tableHeadList { get; set; }
        }
        public class TableHeads {
            public string headname { get; set; }
        }
       
    }
}
