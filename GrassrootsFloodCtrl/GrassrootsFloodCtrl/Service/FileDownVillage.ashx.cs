using GrassrootsFloodCtrl.Controllers;
using System.Web;
using ServiceStack;

namespace GrassrootsFloodCtrl.Service
{
    /// <summary>
    /// FileDownVillage 的摘要说明
    /// </summary>
    public class FileDownVillage : ControllerBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //var name = DateTime.Now.ToString("yyyyMMddhhmmss") + new Random(DateTime.Now.Second).Next(10000);
            //var path = System.Web.HttpContext.Current.Server.MapPath("~/Files/workinggroup/" + name + ".xls");
            //var dt = new System.Data.DataTable();
            //HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //ISheet sheet = hssfworkbook.CreateSheet("村级防汛防台工作组");
            ////表头
            //IRow row = sheet.CreateRow(0);
            //List<string> listtitle = new List<string>();
            //listtitle.Add("序号");
            //listtitle.Add("县市名称");
            //listtitle.Add("乡镇名称");
            //listtitle.Add("行政村名称");
            //listtitle.Add("岗位");
            //listtitle.Add("责任人");
            //listtitle.Add("姓名");
            //listtitle.Add("职务");
            //listtitle.Add("手机");
            //listtitle.Add("备注");
            ////var Columns =listtitle.Select(d => new DataColumn( d.ToString(), typeof(string))).ToArray();
            ////dt.Columns.AddRange(Columns);
            //for (int i = 0; i < listtitle.Count; i++)
            //{
            //    ICell cell = row.CreateCell(i);
            //    cell.SetCellValue(listtitle[i].ToString());

            //    ICellStyle cellStyle = hssfworkbook.CreateCellStyle();
            //    IFont fonthead = hssfworkbook.CreateFont();
            //    fonthead.FontHeightInPoints = 13;
            //    fonthead.Boldweight = 12;
            //    fonthead.Color = HSSFColor.OliveGreen.Red.Index;
            //    cellStyle.SetFont(fonthead);
            //    cell.CellStyle = cellStyle;
            //}
            ///****内容****/
            //try
            //{
            //    Database db = DatabaseManager.CreateDatabase("ConnectionString", false);
            //    int roleId = Convert.ToInt32(userLogin.Get_RoleId);
            //    string where = "";
            //    if (roleId == 4)
            //    {
            //        string LoginADCD = userLogin.Get_UserAdcd.ToString().Trim();
            //        where = " AND left(UnitCode,9)+'000000' LIKE '%" + LoginADCD.Substring(0, 9) + "%'";
            //    }
            //    //人员信息,查询字段顺序不能乱
            //    DataTable zrrData = db.ExecuteDataTable("SELECT b.Title,a.UnitCode,a.Name,a.Position,a.OfficePhone,a.Mobile,a.Email,a.Address,a.Remark FROM P_Zrr as a,DataDictionary as b where a.ZrrGroupID = b.id " + where + "");
            //    //获取单位
            //    var DepER = db.ExecuteDataTable("SELECT DeptCD,DeptNM FROM Dept_B").AsEnumerable();
            //    var ADCDER = db.ExecuteDataTable("SELECT ADCD,ADNM FROM DC_ADCD_Info").AsEnumerable();
            //    for (int i = 0; i < zrrData.Rows.Count; i++)
            //    {
            //        IRow row1 = sheet.CreateRow(i + 1);
            //        for (int j = 0; j < zrrData.Columns.Count; j++)
            //        {
            //            ICell cell = row1.CreateCell(j);
            //            // cell.SetCellValue(zrrData.Rows[i][j].ToString());
            //            switch (j)
            //            {
            //                case 1:
            //                    var unitName = Common.GetADNMORDeptNM(ADCDER, DepER, zrrData.Rows[i][j].ToString());
            //                    cell.SetCellValue(unitName);
            //                    break;
            //                default:
            //                    cell.SetCellValue(zrrData.Rows[i][j].ToString());
            //                    break;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            ////转为字节数组
            //MemoryStream stream = new MemoryStream();
            //hssfworkbook.Write(stream);
            //var buf = stream.ToArray();

            ////保存为Excel文件
            //using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            //{
            //    fs.Write(buf, 0, buf.Length);
            //    fs.Flush();
            //}
            ///********/
            //context.Response.Write("files/zrx/" + name + ".xls");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}