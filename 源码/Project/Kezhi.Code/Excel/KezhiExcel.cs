/*******************************************************************************
 * Copyright © 2016 科致电气
 * Author: Kezhi
 * Description: 上海科致MES系统
 * Website：www.kezhi-electric.com
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System.IO;
using System.Data;
using System.Web;
using System.Data.OleDb;
using System.Web.Hosting;
using System.Reflection;
using NPOI.SS.Util;

namespace Kezhi.Code.Excel
{
    public class KezhiExcel
    {

        /// <summary>
        /// 公共方法：DataTable数据导出到Excel流中
        /// </summary>
        /// <param name="dt">要导出的DataTable</param>
        /// <param name="type">要导出的表类型</param>
        /// <param name="exportType">要导出的特殊类型</param>
        /// <returns>Dictionary<string, string> bg</returns>
        public static MemoryStream DataTableToMS(DataTable dt, String type,String exportType)
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            
            //表头风格
            ICellStyle HeadercellStyle = workbook.CreateCellStyle();
            HeadercellStyle.BorderBottom = BorderStyle.Thin;
            HeadercellStyle.BorderLeft = BorderStyle.Thin;
            HeadercellStyle.BorderRight = BorderStyle.Thin;
            HeadercellStyle.BorderTop = BorderStyle.Thin;
            HeadercellStyle.Alignment = HorizontalAlignment.Center;
            HeadercellStyle.FillForegroundColor = HSSFColor.White.Index;
            HeadercellStyle.FillPattern = FillPattern.SolidForeground;

            //表头字体
            IFont headerfont = workbook.CreateFont();
            headerfont.FontHeightInPoints = 12;
            headerfont.FontName = "宋体";
            headerfont.Boldweight = (short)FontBoldWeight.Bold;
            HeadercellStyle.SetFont(headerfont);

            //用column name作为列名
            int icolIndex = 0;
            IRow headerRow = sheet.CreateRow(0);
            foreach (DataColumn item in dt.Columns)
            {
                ICell cell = headerRow.CreateCell(icolIndex);
                cell.SetCellValue(ColumHeaderCodeToName(item.ColumnName, type));
                cell.CellStyle = HeadercellStyle;
                icolIndex++;
            }

            //表内容风格
            ICellStyle cellStyle = workbook.CreateCellStyle();
            //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            cellStyle.FillForegroundColor = HSSFColor.White.Index;
            cellStyle.FillBackgroundColor = HSSFColor.White.Index;
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.FillPattern = FillPattern.FineDots;
            //cellStyle.Alignment = HorizontalAlignment.Center;//水平居中
            cellStyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
              //内容字体
            IFont contentfont = workbook.CreateFont();
            contentfont.FontHeightInPoints = 12;
            contentfont.FontName = "宋体";
            cellStyle.SetFont(contentfont);

            //特殊行背景设置(小计)
            ICellStyle specialStyle = workbook.CreateCellStyle();
            specialStyle.FillForegroundColor = HSSFColor.LightOrange.Index;
            specialStyle.FillBackgroundColor = HSSFColor.LightOrange.Index;
            specialStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyle.FillPattern = FillPattern.FineDots;
            specialStyle.SetFont(contentfont);

            //特殊行背景设置(工作时长)
            ICellStyle specialStyleHours = workbook.CreateCellStyle();
            specialStyleHours.FillForegroundColor = HSSFColor.White.Index;
            specialStyleHours.FillBackgroundColor = HSSFColor.White.Index;
            specialStyleHours.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleHours.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleHours.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleHours.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleHours.FillPattern = FillPattern.FineDots;
            specialStyleHours.SetFont(contentfont);
            //建立内容
            int iRowIndex = 1;
            int iCellIndex = 0;
            foreach (DataRow Rowitem in dt.Rows)
            {
                IRow DataRow = sheet.CreateRow(iRowIndex);
               
                #region 下面方法专为此项目导出绩效功能设计，不影响其他excel导出，设置"小计"一行的背景
                string username = Rowitem.ItemArray[0].ToString();
                if ("小计".Equals(username))
                {
                    foreach (DataColumn Colitem in dt.Columns)
                    {
                        ICell cell = DataRow.CreateCell(iCellIndex);

                        cell.SetCellValue(Rowitem[Colitem].ToString());
                        cell.CellStyle = specialStyle;

                        iCellIndex++;
                    }
                }
                else
                {
                    foreach (DataColumn Colitem in dt.Columns)
                    {
                        
                        ICell cell = DataRow.CreateCell(iCellIndex);
                        if (Colitem.Caption.Equals("F_WorkedHours"))
                        {
                            cell.CellStyle = specialStyleHours;
                            cell.SetCellValue(Common.SetNumberByString(Rowitem[Colitem].ToString()));
                        }
                        else if (Colitem.Caption.Equals("F_WeekWorkedHours"))
                        {
                            cell.CellStyle = specialStyleHours;
                            cell.SetCellFormula(Rowitem[Colitem].ToString());
                        }
                        else
                        {
                            cell.CellStyle = cellStyle;
                            cell.SetCellValue(Rowitem[Colitem].ToString());
                        }
                        

                        iCellIndex++;
                    }
                }
                #endregion
           
                iCellIndex = 0;
                iRowIndex++;
            }

            //自适应列宽度
            for (int i = 0; i < icolIndex; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            //合并单元格
            if (exportType.Equals("周报"))
            {
                SetWeeklyWorkLog(sheet, dt);
            }
            
            //写Excel
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }
        public static void SetWeeklyWorkLog(ISheet sheet, DataTable dt)
        {
            #region 项目部分合并
            var rowStart = 1;//开始合并的行
            var rowEnd = 1;//结束合并的行
            bool flag = false;
            for (var i = 1; i < dt.Rows.Count - 1; i++)
            {

                var projectNum1 = dt.Rows[i]["F_ProjectNum"].ToString();
                var projectNum2 = dt.Rows[i + 1]["F_ProjectNum"].ToString();

                if ((!projectNum1.Equals("") && (projectNum2.Equals(""))))
                {
                    rowStart = i + 1;
                }
                else if ((projectNum1.Equals("") && (!projectNum2.Equals(""))))
                {
                    rowEnd = i + 1;
                    flag = true;
                }
                //最后一个与前一个不同，不合并;相同，最后一个为合并的end
                if (i == dt.Rows.Count - 2)
                {
                    if ((projectNum1.Equals("") && (projectNum2.Equals(""))))
                    {
                        rowEnd = i + 2;
                        flag = true;
                    }
                    else if ((!projectNum1.Equals("") && (projectNum2.Equals(""))))
                    {
                        rowStart = i + 1;
                        rowEnd = i + 2;
                        flag = true;
                    }

                }
                if (flag)
                {
                    //项目序号
                    CellRangeAddress region1 = new CellRangeAddress(rowStart, rowEnd, 0, 0);
                    sheet.AddMergedRegion(region1);
                    //项目编号
                    CellRangeAddress region2 = new CellRangeAddress(rowStart, rowEnd, 1, 1);
                    sheet.AddMergedRegion(region2);
                    //项目名称
                    CellRangeAddress region3 = new CellRangeAddress(rowStart, rowEnd, 2, 2);
                    sheet.AddMergedRegion(region3);
                    //项目经理
                    CellRangeAddress region4 = new CellRangeAddress(rowStart, rowEnd, 3, 3);
                    sheet.AddMergedRegion(region4);
                    flag = false;
                }
            }
            #endregion
            //员工合并
            var rowUserStart = 1;//开始合并的行
            var rowUserEnd = 1;//结束合并的行
            bool Userflag = false;
            for (var i = 0; i < dt.Rows.Count - 1; i++)
            {
                #region 员工部分合并

                var userName1 = dt.Rows[i]["F_UserNum"].ToString();
                var userName2 = dt.Rows[i + 1]["F_UserNum"].ToString();
                if ((!userName1.Equals("") && (userName2.Equals(""))))
                {
                    rowUserStart = i + 1;
                }
                else if ((userName1.Equals("") && (!userName2.Equals(""))))
                {
                    rowUserEnd = i + 1;
                    Userflag = true;
                }
                //最后一个与前一个不同，不合并，相同，最后一个为合并的end
                if (i == dt.Rows.Count - 2)
                {
                    if ((userName1.Equals("") && (userName2.Equals(""))))
                    {
                        rowUserEnd = i + 2;
                        Userflag = true;
                    }
                    else if ((!userName1.Equals("") && (userName2.Equals(""))))
                    {
                        rowUserStart = i + 1;
                        rowUserEnd = i + 2;
                        Userflag = true;
                    }
                }
                if (Userflag)
                {
                    //员工序号
                    CellRangeAddress region1 = new CellRangeAddress(rowUserStart, rowUserEnd, 4, 4);
                    sheet.AddMergedRegion(region1);
                    //员工姓名
                    CellRangeAddress region2 = new CellRangeAddress(rowUserStart, rowUserEnd, 5, 5);
                    sheet.AddMergedRegion(region2);
                    //周工作数
                    CellRangeAddress region3 = new CellRangeAddress(rowUserStart, rowUserEnd, 14, 14);
                    sheet.AddMergedRegion(region3);
                    Userflag = false;
                }

                #endregion
            }
        }
        /// <summary>
        /// 公共方法：导出工作绩效
        /// </summary>
        /// <param name="dt">要导出的DataTable</param>
        /// <param name="type">要导出的表类型</param>
        /// <param name="exportType">要导出的特殊类型</param>
        /// <returns>Dictionary<string, string> bg</returns>
        public static MemoryStream DataTableToMSByJobPerformance(DataTable dt, String type, String exportType)
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            //表头风格
            ICellStyle HeadercellStyle = workbook.CreateCellStyle();
            HeadercellStyle.BorderBottom = BorderStyle.Thin;
            HeadercellStyle.BorderLeft = BorderStyle.Thin;
            HeadercellStyle.BorderRight = BorderStyle.Thin;
            HeadercellStyle.BorderTop = BorderStyle.Thin;
            HeadercellStyle.Alignment = HorizontalAlignment.Center;
            HeadercellStyle.FillForegroundColor = HSSFColor.White.Index;
            HeadercellStyle.FillPattern = FillPattern.SolidForeground;

            //表头字体
            IFont headerfont = workbook.CreateFont();
            headerfont.FontHeightInPoints = 12;
            headerfont.FontName = "宋体";
            headerfont.Boldweight = (short)FontBoldWeight.Bold;
            HeadercellStyle.SetFont(headerfont);

            //用column name作为列名
            int icolIndex = 0;
            IRow headerRow = sheet.CreateRow(0);
            foreach (DataColumn item in dt.Columns)
            {
                ICell cell = headerRow.CreateCell(icolIndex);
                cell.SetCellValue(ColumHeaderCodeToName(item.ColumnName, type));
                cell.CellStyle = HeadercellStyle;
                icolIndex++;
            }

            //表内容风格
            ICellStyle cellStyle = workbook.CreateCellStyle();
            //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            cellStyle.FillForegroundColor = HSSFColor.White.Index;
            cellStyle.FillBackgroundColor = HSSFColor.White.Index;
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.FillPattern = FillPattern.FineDots;
            //cellStyle.Alignment = HorizontalAlignment.Center;//水平居中
            cellStyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            //内容字体
            IFont contentfont = workbook.CreateFont();
            contentfont.FontHeightInPoints = 12;
            contentfont.FontName = "宋体";
            cellStyle.SetFont(contentfont);

            //特殊行背景设置(小计)
            ICellStyle specialStyle = workbook.CreateCellStyle();
            specialStyle.FillForegroundColor = HSSFColor.LightOrange.Index;
            specialStyle.FillBackgroundColor = HSSFColor.LightOrange.Index;
            specialStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyle.FillPattern = FillPattern.FineDots;
            specialStyle.SetFont(contentfont);

            //特殊行背景设置(工作时长)
            ICellStyle specialStyleHours = workbook.CreateCellStyle();
            specialStyleHours.FillForegroundColor = HSSFColor.White.Index;
            specialStyleHours.FillBackgroundColor = HSSFColor.White.Index;
            specialStyleHours.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleHours.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleHours.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleHours.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleHours.FillPattern = FillPattern.FineDots;
            specialStyleHours.SetFont(contentfont);
            //特殊行背景设置(周末)
            ICellStyle specialStyleWeek = workbook.CreateCellStyle();
            specialStyleWeek.FillForegroundColor = HSSFColor.Lime.Index;
            specialStyleWeek.FillBackgroundColor = HSSFColor.Lime.Index;
            specialStyleWeek.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleWeek.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleWeek.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleWeek.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            specialStyleWeek.FillPattern = FillPattern.FineDots;
            specialStyleWeek.SetFont(contentfont);
            //建立内容
            int iRowIndex = 1;
            int iCellIndex = 0;
            foreach (DataRow Rowitem in dt.Rows)
            {
                IRow DataRow = sheet.CreateRow(iRowIndex);

                #region 下面方法专为此项目导出绩效功能设计，不影响其他excel导出，设置"小计"一行的背景
                string username = Rowitem.ItemArray[0].ToString();
                string weekName = Rowitem.ItemArray[2].ToString();
                //小计背景
                if ("小计".Equals(username))
                {
                    foreach (DataColumn Colitem in dt.Columns)
                    {
                        ICell cell = DataRow.CreateCell(iCellIndex);
                        string[] cellsName = { "F_WorkedHours", "F_WorkSubsidy", "F_PayHours", "F_RestHours", "F_DeductHours", "F_MealSubsidy" };
                        if (cellsName.Contains(Colitem.Caption))
                        {
                            cell.CellStyle = specialStyle;
                            cell.SetCellFormula(Rowitem[Colitem].ToString());
                        }
                        else
                        {
                            cell.SetCellValue(Rowitem[Colitem].ToString());
                            cell.CellStyle = specialStyle;
                        }
                        

                        iCellIndex++;
                    }
                }
                    //周末背景
                else if("星期六".Equals(weekName) || "星期日".Equals(weekName))
                {
                    foreach (DataColumn Colitem in dt.Columns)
                    {

                        ICell cell = DataRow.CreateCell(iCellIndex);
                        string[] cellsName = { "F_WorkedHours", "F_WorkSubsidy", "F_PayHours", "F_RestHours", "F_DeductHours", "F_MealSubsidy" };
                        if (cellsName.Contains(Colitem.Caption))
                        {
                            cell.CellStyle = specialStyleWeek;
                            cell.SetCellValue(Common.SetNumberByString(Rowitem[Colitem].ToString()));
                        }
                        else
                        {
                            cell.CellStyle = specialStyleWeek;
                            cell.SetCellValue(Rowitem[Colitem].ToString());
                        }


                        iCellIndex++;
                    }
                }
                else
                {
                    foreach (DataColumn Colitem in dt.Columns)
                    {

                        ICell cell = DataRow.CreateCell(iCellIndex);
                        string[] cellsName = {"F_WorkedHours","F_WorkSubsidy","F_PayHours","F_RestHours","F_DeductHours","F_MealSubsidy"};
                        if (cellsName.Contains(Colitem.Caption))
                        {
                            cell.CellStyle = specialStyleHours;
                            cell.SetCellValue(Common.SetNumberByString(Rowitem[Colitem].ToString()));
                        }
                        else
                        {
                            cell.CellStyle = cellStyle;
                            cell.SetCellValue(Rowitem[Colitem].ToString());
                        }


                        iCellIndex++;
                    }
                }
             
                #endregion

                iCellIndex = 0;
                iRowIndex++;
            }
            //自适应列宽度
            for (int i = 0; i < icolIndex; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            //合并单元格
            if (exportType.Equals("周报"))
            {
                SetWeeklyWorkLog(sheet, dt);
            }

            //写Excel
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }

        /// <summary>
        /// 公共方法：将某路径下的Excel文件中的数据传至DataTable
        /// </summary>
        /// <param name="strFilePath">Excel文件路径</param>
        /// <param name="strTableName">DataTable表名</param>
        /// <param name="iSheetIndex">Excel中第几个Sheet表</param>
        /// <returns></returns>
        public static DataTable GetUploadDataTable(String strFilePath, String strTableName, int iSheetIndex)
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(strFilePath))
            {
                strFilePath = "";
            }

            string strExtName = Path.GetExtension(strFilePath);

            if (!string.IsNullOrEmpty(strTableName))
            {
                dt.TableName = strTableName;
            }

            if (strExtName.Equals(".xls"))
            {
                using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(file);// 2003版本
                    ISheet sheet = workbook.GetSheetAt(iSheetIndex);

                    //列头
                    foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
                    {
                        dt.Columns.Add(item.ToString(), typeof(string));
                    }

                    //写入内容
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    while (rows.MoveNext())
                    {
                        IRow row = (HSSFRow)rows.Current;

                        if (row.RowNum == sheet.FirstRowNum)
                        {
                            continue;
                        }

                        DataRow dr = dt.NewRow();
                        foreach (ICell item in row.Cells)
                        {
                            switch (item.CellType)
                            {
                                case CellType.Boolean:
                                    dr[item.ColumnIndex] = item.BooleanCellValue;
                                    break;
                                case CellType.Formula:
                                    switch (item.CachedFormulaResultType)
                                    {
                                        case CellType.Boolean:
                                            dr[item.ColumnIndex] = item.BooleanCellValue;
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(item))
                                            {
                                                dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = item.NumericCellValue;
                                            }
                                            break;
                                        case CellType.String:
                                            string str = item.StringCellValue;
                                            if (!string.IsNullOrEmpty(str))
                                            {
                                                dr[item.ColumnIndex] = str.ToString();
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = null;
                                            }
                                            break;
                                        case CellType.Error:
                                        case CellType.Unknown:
                                        case CellType.Blank:
                                        default:
                                            dr[item.ColumnIndex] = string.Empty;
                                            break;
                                    }
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(item))
                                    {
                                        dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = item.NumericCellValue;
                                    }
                                    break;
                                case CellType.String:
                                    string strValue = item.StringCellValue;
                                    if (!string.IsNullOrEmpty(strValue))
                                    {
                                        dr[item.ColumnIndex] = strValue.ToString();
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = null;
                                    }
                                    break;
                                case CellType.Error:
                                case CellType.Unknown:
                                case CellType.Blank:
                                default:
                                    dr[item.ColumnIndex] = string.Empty;
                                    break;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 公共方法：将某路径下的Excel文件中的数据传至DataTable
        /// </summary>
        /// <param name="strFilePath">Excel文件路径</param>
        /// <param name="strTableName">DataTable表名</param>
        /// <param name="iSheetIndex">Excel中第几个Sheet表</param>
        /// <returns></returns>
        public static DataTable GetExcelDataTable(String strFilePath)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(strFilePath))
            {
                strFilePath = "";
            }
            string file = strFilePath;
            //Excel驱动连接
            string strConn = "";
            if (System.IO.Path.GetExtension(file).ToLower() == ".xls")
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;data source=" + file + ";Extended Properties='Excel 8.0; HDR=NO; IMEX=1'";
            }
            else
            {
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;data source=" + file + ";Extended Properties='Excel 12.0 Xml;HDR=NO; IMEX=1'";
            }

            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();

            //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等  
            DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

            //包含excel中表名的字符串数组
            string[] strTableNames = new string[dtSheetName.Rows.Count];
            for (int k = 0; k < dtSheetName.Rows.Count; k++)
            {
                strTableNames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
            }

            OleDbDataAdapter myCommand = null;

            //从指定的表明查询数据,可先把所有表明列出来供用户选择
            string strExcel = "select * from [" + strTableNames[0] + "]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            dt = new DataTable();
            myCommand.Fill(dt);

            conn.Close();
            myCommand.Dispose();
            return dt;
        }
        /// <summary>
        /// 导入模板数据到Excel流中
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static MemoryStream TemplateExcelToMs(string templateName)
        {
            string filePath = HostingEnvironment.MapPath("/Content/TemplateFile/" + templateName);
            MemoryStream ms = new MemoryStream();
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // 创建一个webbook，对应一个Excel文件
            var workbook = new HSSFWorkbook(fs);

            workbook.Write(ms);
            // 设置当前流的位置.意思是在流的开始位置（SeekOrigin.Begin）偏移零位。
            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }
        /// <summary>
        /// 公共方法：将表头编码转换为表头名称（如果添加导出类型，请修改此方法）
        /// </summary>
        /// <param name="HeaderCode">表头编码</param>
        /// <param name="Type">导出的表类型</param>
        /// <returns></returns>
        public static string ColumHeaderCodeToName(String HeaderCode, String Type)
        {
            string HeaderName = "";

            if (Type == "performance")
            {
                switch (HeaderCode)
                {
                    case "F_WorkUserName":
                        HeaderName = "姓名";
                        break;
                    case "F_Weekday":
                        HeaderName = "星期";
                        break;
                    case "F_WorkDate":
                        HeaderName = "日期";
                        break;
                    case "F_ProjectCode":
                        HeaderName = "所在项目";
                        break;
                    case "F_WorkAddress":
                        HeaderName = "上班地点";
                        break;
                    case "F_WorkAddressCode":
                        HeaderName = "外场说明";
                        break;
                    case "F_WorkedHours":
                        HeaderName = "出勤时间";
                        break;
                    case "F_PayHours":
                        HeaderName = "已支付工时";
                        break;
                    case "F_RestHours":
                        HeaderName = "可调休使用工时";
                        break;
                    case "F_DeductHours":
                        HeaderName = "考核扣除工时";
                        break;
                    case "F_MealSubsidy":
                        HeaderName = "餐贴";
                        break;
                    case "F_WorkSubsidy":
                        HeaderName = "津贴";
                        break;
                    default:
                        break;
                }
            }
            else if (Type == "WorkDailyRecord")
            {
                switch (HeaderCode)
                {
                    case "F_WorkUserName":
                        HeaderName = "姓名";
                        break;
                    case "F_WorkDate":
                        HeaderName = "日期";
                        break;
                    case "F_WorkAddress":
                        HeaderName = "工作地点";
                        break;
                    case "F_WorkedHours":
                        HeaderName = "工作时长";
                        break;
                    case "F_ProjectCode":
                        HeaderName = "项目编号";
                        break;
                    case "F_DailyRecord":
                        HeaderName = "工作内容";
                        break;
                    case "F_Description":
                        HeaderName = "备注";
                        break;
                    case "F_WorkTimeStart":
                        HeaderName = "上班时间";
                        break;
                    case "F_WorkTimeEnd":
                        HeaderName = "下班时间";
                        break;
                    case "F_WorkSubsidy":
                        HeaderName = "津贴";
                        break;
                    default:
                        break;
                }
            }
            else if (Type == "WorkWeekDailyRecord")
            {
                switch (HeaderCode)
                {
                    case "F_WorkUserName":
                        HeaderName = "姓名";
                        break;
                    case "F_WorkDate":
                        HeaderName = "日期";
                        break;
                    case "F_ProjectName":
                        HeaderName = "项目名称";
                        break;
                    case "F_ProjectCode":
                        HeaderName = "项目编号";
                        break;
                    case "F_ProjectManagerName":
                        HeaderName = "项目经理";
                        break;
                    case "F_WorkAddress":
                        HeaderName = "工作地点";
                        break;
                    case "F_DailyRecord":
                        HeaderName = "工作内容";
                        break;
                    case "F_Description":
                        HeaderName = "备注";
                        break;
                    case "F_WorkTimeStart":
                        HeaderName = "上班时间";
                        break;
                    case "F_WorkTimeEnd":
                        HeaderName = "下班时间";
                        break;
                    case "F_WorkedHours":
                        HeaderName = "工作时数";
                        break;
                    case "F_WeekWorkedHours":
                        HeaderName = "周工作数";
                        break;
                    case "F_ProjectNum":
                        HeaderName = "项目序号";
                        break;
                    case "F_UserNum":
                        HeaderName = "人员序号";
                        break;
                    case "F_ProjectCodeUser":
                        HeaderName = "项目编号";
                        break;
                    default:
                        break;
                }
            }
            
            return HeaderName;
        }

        /// <summary>
        /// 将实体集合转换成dataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entitys">实体集合</param>
        /// <returns>DataTable</returns>
        public static DataTable ListToDataTable<T>(List<T> entitys,bool flag)
        {

            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                return new DataTable();
            }

            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable("dt");
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }

            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
               
              
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);

                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
    }
}
