using NPOI.HSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Post;
using GrassrootsFloodCtrl.ServiceModel.Route;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Configuration;
using System.Data;
using Dy.Common;

namespace GrassrootsFloodCtrl.Logic.Common
{
   public class creatExcel
    {
       /// <summary>
       /// 设置excel并创建Excel
       /// </summary>
       /// <param name="workBook"></param>
       /// <param name="sheetName"></param>
       /// <param name="mergeCellContent"></param>
       /// <param name="mergeCellNum"></param>
       /// <param name="listTitle"></param>
       /// <returns></returns>
        public ISheet createSheet1(HSSFWorkbook workBook, string sheetName,string mergeCellContent,int mergeCellNum, List<string> listTitle)
        {
            ISheet sheet = workBook.CreateSheet(sheetName);
            IRow RowHead = sheet.CreateRow(0);
            //合并第一行单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, mergeCellNum));

            string noitce = "注意： ";
            noitce += mergeCellContent;
            RowHead.CreateCell(0).SetCellValue(noitce);
            /*****单元格样式 start******/
            var cellStyleHead = workBook.CreateCellStyle();
            cellStyleHead.Alignment = HorizontalAlignment.Left;//居中显示
            cellStyleHead.VerticalAlignment = VerticalAlignment.Top;//垂直居中
            cellStyleHead.WrapText = true;
            //高度
            RowHead.HeightInPoints = noitce.Length/mergeCellNum> mergeCellNum? 70:50; //2 * sheet.DefaultRowHeight / 10;
            //字体设置,字体要调用CreateFont()
            IFont fonthead = workBook.CreateFont();
            fonthead.FontHeightInPoints = 15;
            fonthead.Color = HSSFColor.Red.Index;
            cellStyleHead.SetFont(fonthead);
            //这里调试出来的,样式一定要给到单元格才有效
            RowHead.Cells[0].CellStyle = cellStyleHead;
            /*****单元格样式 end******/
            /*********添加表头 s********/
            IRow RowBody = sheet.CreateRow(1);

            for (int iColumnIndex = 0; iColumnIndex < listTitle.Count(); iColumnIndex++)
            {
                
                RowBody.CreateCell(iColumnIndex).SetCellValue(listTitle[iColumnIndex].ToString());
                RowBody.Cells[iColumnIndex].Row.HeightInPoints = 40;
                //单元格样式
                ICellStyle cellStyle = workBook.CreateCellStyle();
                cellStyle.FillForegroundColor =HSSFColor.Grey25Percent.Index;
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.FillBackgroundColor = HSSFColor.Grey25Percent.Index;
                //设置单元格上下左右边框线  
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
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
           
            return sheet;
        }

        /// <summary>
        /// 设置下拉选项
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="sheet"></param>
        /// <param name="cellName"></param>
        /// <param name="cellNo"></param>
        /// <param name="list"></param>
        public void setSheet2(HSSFWorkbook workBook, ISheet sheet, string cellName,int cellNo,List<string> list)
        {
            //创建表
            ISheet sheet2 = workBook.CreateSheet(cellName);
            //隐藏
            workBook.SetSheetHidden(1, true);
            //取数据
            for (int i = 0; i < list.Count; i++)
            {
                sheet2.CreateRow(i).CreateCell(0).SetCellValue(list[i]);
            }
            
            //设计表名称
            IName range = workBook.CreateName();
            range.RefersToFormula = cellName+"!$A:$A";
            range.NameName = cellName;
            //定义下拉框范围
            CellRangeAddressList regions = new CellRangeAddressList(2, 65535, cellNo, cellNo);
            //设置数据引用
            DVConstraint constraint = DVConstraint.CreateFormulaListConstraint(cellName);
            HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
            sheet.AddValidationData(dataValidate);
            
        }

        /// <summary>
        /// 创建村的工作表
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="cellName"></param>
        /// <param name="list"></param>
        public void setSheet3(HSSFWorkbook workBook,string cellName, List<string> list)
        {
            //创建表
            ISheet sheet2 = workBook.CreateSheet(cellName);
            //隐藏
            //workBook.SetSheetHidden(1, true);

            //单元格样式
            ICellStyle cellStyle = workBook.CreateCellStyle();
            cellStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.FillBackgroundColor = HSSFColor.Grey25Percent.Index;
            //设置单元格上下左右边框线  
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            //文字水平和垂直对齐方式  
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Top;
            //是否换行
            cellStyle.WrapText = true;

            //字体大小
            IFont cellfont = workBook.CreateFont();
            cellfont.FontHeightInPoints = 14;
            cellStyle.SetFont(cellfont);
            sheet2.SetColumnWidth(0, 20 * 256);
            //取数据
            for (int i = 0; i < list.Count; i++)
            {
                ICell cell = sheet2.CreateRow(i).CreateCell(0);
                cell.CellStyle= cellStyle;
                cell.SetCellValue(list[i]);
            }
        }

        readonly int EXCEL03_MaxRow = 65535;
        /// <summary>
        /// 将DataTable转换为excel2003格式。
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public byte[] DataTableToExcel(DataTable dt, string sheetName, int mergeCellNum, string mergeCellContent)
        {

            IWorkbook book = new HSSFWorkbook();
            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWriteToSheet(dt, 0, dt.Rows.Count - 1, book, sheetName, mergeCellNum, mergeCellContent);
            else
            {
                int page = dt.Rows.Count / EXCEL03_MaxRow;
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWriteToSheet(dt, start, end, book, sheetName + i.ToString(), mergeCellNum, mergeCellContent);
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWriteToSheet(dt, dt.Rows.Count - lastPageItemCount, lastPageItemCount, book, sheetName + page.ToString(), mergeCellNum, mergeCellContent);
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            return ms.ToArray();
        }

        private void DataWriteToSheet(DataTable dt, int startRow, int endRow, IWorkbook book, string sheetName, int mergeCellNum, string mergeCellContent)
        {
            ISheet sheet = book.CreateSheet(sheetName);
            IRow header = null; int rowIndex = 0;
            //表头
            if (!string.IsNullOrEmpty(mergeCellContent) && mergeCellNum > 0)
            {
                var cellStyleHeadNotice = book.CreateCellStyle();
                cellStyleHeadNotice.Alignment = HorizontalAlignment.Left;//居中显示
                cellStyleHeadNotice.VerticalAlignment = VerticalAlignment.Top;//垂直居中
                ExcelHeadAttribute ehanotice = new ExcelHeadAttribute()
                {
                    rowIndex = 0,
                    firstRow = 0,
                    lastRow = 0,
                    firstCol = 0,
                    lastCol = mergeCellNum-1,
                    fontColor = 10,
                    fontSize = 10,
                    HeightInPoints = 30,
                    name = mergeCellContent
                };
                CreateHead(book, sheet, cellStyleHeadNotice, ehanotice);
                header = sheet.CreateRow(1);
                rowIndex = 2;
            }
            else
            {
                header = sheet.CreateRow(0);
                rowIndex = 1;
            }
             
            /*****单元格样式 start******/
            var cellStyleHead = book.CreateCellStyle();
            cellStyleHead.Alignment = HorizontalAlignment.Center;//居中显示
            cellStyleHead.VerticalAlignment = VerticalAlignment.Top;//垂直居中
            cellStyleHead.WrapText = true;
            //高度
            header.HeightInPoints = 50; //2 * sheet.DefaultRowHeight / 10;
            //字体设置,字体要调用CreateFont()
            IFont fonthead = book.CreateFont();
            fonthead.FontHeightInPoints = 10;
            fonthead.Color = HSSFColor.Red.Index;
            cellStyleHead.SetFont(fonthead);
            //这里调试出来的,样式一定要给到单元格才有效
            //header.Cells[0].CellStyle = cellStyleHead;
            /*****单元格样式 end******/
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = header.CreateCell(i);
                string val = dt.Columns[i].Caption ?? dt.Columns[i].ColumnName;
                cell.SetCellValue(val);

                header.Cells[i].Row.HeightInPoints = 20;
                //单元格样式
                ICellStyle cellStyle = book.CreateCellStyle();
                cellStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.FillBackgroundColor = HSSFColor.Grey25Percent.Index;
                //设置单元格上下左右边框线  
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                //文字水平和垂直对齐方式  
                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = VerticalAlignment.Top;
                //是否换行
                cellStyle.WrapText = true;

                header.Cells[i].CellStyle = cellStyle;
                //字体大小
                IFont cellfont = book.CreateFont();
                cellfont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellfont);
                sheet.SetColumnWidth(i, 20 * 256);
            }
            
            for (int i = startRow; i <= endRow; i++)
            {
                DataRow dtRow = dt.Rows[i];
                IRow excelRow = sheet.CreateRow(rowIndex++);

                for (int j = 0; j < dtRow.ItemArray.Length; j++)
                {
                    excelRow.CreateCell(j).SetCellValue(dtRow[j].ToString());
                }
                
            }

        }
         //表头制作
    public void CreateHead(IWorkbook workBook, ISheet sheet, ICellStyle cellStyleHeadTitle, ExcelHeadAttribute eha)
    {
        IRow RowHeadTitle = sheet.CreateRow(eha.rowIndex);
        //合并单元格
        sheet.AddMergedRegion(new CellRangeAddress(eha.firstRow, eha.lastRow, eha.firstCol, eha.lastCol));
        RowHeadTitle.CreateCell(0).SetCellValue(eha.name);
        /*****单元格样式 start******/
        cellStyleHeadTitle.WrapText = true;
        //高度
        RowHeadTitle.HeightInPoints = eha.HeightInPoints; //2 * sheet.DefaultRowHeight / 10;
                                                          //字体设置,字体要调用CreateFont()
        IFont fontheadTitle = workBook.CreateFont();
        fontheadTitle.FontHeightInPoints = eha.fontSize;
        //HSSFColor.OliveGreen.Black.Index
        fontheadTitle.Color = eha.fontColor;
        cellStyleHeadTitle.SetFont(fontheadTitle);
        //这里调试出来的,样式一定要给到单元格才有效
        RowHeadTitle.Cells[0].CellStyle = cellStyleHeadTitle;
    }
    }
}
