
using Dy.Common;
using GrassrootsFloodCtrl.Model.CountryPerson;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Country;
using System.IO;
using GrassrootsFloodCtrl.Logic.Common;
using System.Collections;
using GrassrootsFloodCtrl.Model.Sys;
using System.Configuration;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Audit;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using System.Text;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Model.ZZTX;
using System.Text.RegularExpressions;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Supervise;
using Aspose.Cells;

namespace GrassrootsFloodCtrl.Logic.Country
{
	public class CountryPersonManager : ManagerBase,ICountryPersonManager
	{
		public ILogHelper logHelper { get; set; }
		string logMsgName = "县级防汛防台:";
		#region
		//public string AddChangePersons(AddChangePersons requset)
		//{
		//    using (var db = DbFactory.Open())
		//    {
		//        var builder = requset.information;
		//        //第一版更改批量导入暂时不用
		//        //var requestAddChangePersonList = JsonTools.Deserialize<List<RequestAddChangePerson>>(builder);
		//        //var countryPersonList = new List<CountryPerson>();
		//        //foreach (var info in requestAddChangePersonList)
		//        //{
		//        //    var countryPerson = new CountryPerson();
		//        //    countryPerson.UserName = info.Name;
		//        //    countryPerson.Phone =info.Phone;
		//        //    countryPerson.CountryName = RealName;
		//        //    countryPerson.PostionId = info.Id;
		//        //    countryPerson.AllowChanges = null;
		//        //    countryPersonList.Add(countryPerson);
		//        //}
		//        //db.InsertAll(countryPersonList);
		//        return "yes";
		//    }
		//}
		#endregion
		#region 执行批量插入数据的方法
		public ReturnInsertStatus InsertExeclCountryPerson(CountryImportExcel request)
		{
			using (var db = DbFactory.Open())
			{
				var postList = db.Select<Model.Post.Post>(x=>x.PostType== GrassrootsFloodCtrlEnums.ZZTXEnums.县级防汛防台责任人.ToString());                  
				//获取数据
				var newpath = System.Web.HttpContext.Current.Server.MapPath(request.filePath);
				//将数据转化table
				//var dt = Common.ExcelHelper.GetDataTable(newpath);
				//dt.Rows.RemoveAt(0);
				Workbook workbook = new Workbook();
				workbook.Open(newpath);
				Cells cells = workbook.Worksheets[0].Cells;
				var dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);
				dt.Rows.RemoveAt(0);
				//table传list
				var insertExeclCountryPersonList = DtChangeList(dt);
				//核对数据有没有重复和手机号是否正确
				var returnCheckInsert =CheckInsertData(insertExeclCountryPersonList, postList);
				if(returnCheckInsert.Status)
				{
					int checkCount;
					var auditing = db.Select<AuditCounty>("select * from dbo.AuditCounty where CountyADCD='" + adcd + "'");
					if (auditing.Count == 0)
					{
						checkCount = 1;
					}
					else
					{
						checkCount = Convert.ToInt32(auditing[0].AuditNums) + 1;
					}
					//插入数据库的List
					var countryPersonList = GetCountryPerson(insertExeclCountryPersonList, request, checkCount);
					if (countryPersonList != null)
					{
						db.InsertAll(countryPersonList);
						logHelper.WriteLog(logMsgName + countryPersonList.Count, OperationTypeEnums.新增);
						//if (logHelper.WriteLog(logMsgName + countryPersonList.Count, OperationTypeEnums.新增))
						//{ throw new Exception("插入日志"); }
					}
				}
				return returnCheckInsert;
				
			}
		}

		private ReturnInsertStatus CheckInsertData(List<InsertExeclCountryPerson> insertExeclCountryPersonList,List<Model.Post.Post> postList)
		{
			
			ReturnInsertStatus returnInsertStatus =new ReturnInsertStatus { Column=insertExeclCountryPersonList.Count+1,ColumnName="",Status=true};
			for(int i=0;i<insertExeclCountryPersonList.Count;i++)
			{
				List<InsertExeclCountryPerson> result=insertExeclCountryPersonList.FindAll(x=>x.Phone== insertExeclCountryPersonList[i].Phone && x.Post == insertExeclCountryPersonList[i].Post
				  &&x.Postion==insertExeclCountryPersonList[i].Postion&&x.Remark==insertExeclCountryPersonList[i].Remark&&x.UserName==insertExeclCountryPersonList[i].UserName);
				if (result.Count > 1)
				{
					returnInsertStatus.Status = false;
					returnInsertStatus.Column = i + 1;
					returnInsertStatus.ColumnName = "";
					returnInsertStatus.Description = "数据第" + returnInsertStatus.Column + "行重复！";
				}
				//匹配手机号
				if (!Regex.IsMatch(insertExeclCountryPersonList[i].Phone, @"^[1]+[3,5,7,8,9]+\d{9}"))
				{
					returnInsertStatus.Status = false;
					returnInsertStatus.Column = i + 1;
					returnInsertStatus.ColumnName = "手机号";
					returnInsertStatus.Description ="数据第"+ returnInsertStatus.Column + "行的手机号格式不正确！";
				}
				if (string.IsNullOrEmpty(insertExeclCountryPersonList[i].Phone)|| string.IsNullOrEmpty(insertExeclCountryPersonList[i].UserName)|| string.IsNullOrEmpty(insertExeclCountryPersonList[i].Postion))
				{
					returnInsertStatus.Status = false;
					returnInsertStatus.Column = i + 1;
					returnInsertStatus.ColumnName = "";
					returnInsertStatus.Description = "数据第" + returnInsertStatus.Column + "行的岗位,姓名和职务其中有空值！";
				}
				var listStr = postList.FindAll(x => x.PostName == insertExeclCountryPersonList[i].Postion);
				if (listStr.Count < 1)
				{
					returnInsertStatus.Status = false;
					returnInsertStatus.Column = i + 1;
					returnInsertStatus.ColumnName = "岗位";
					returnInsertStatus.Description = "数据第" + returnInsertStatus.Column + "行的岗位不匹配！";
				}
				if (!returnInsertStatus.Status)
				{
					return returnInsertStatus;
				}
							   
			}
			return returnInsertStatus;           
		}

		#endregion

		#region  核对有没有重复或者相同的数据
		private List<InsertExeclCountryPerson> DtChangeList(DataTable dt)
		{
			var execlList = from a in dt.AsEnumerable()
							select new InsertExeclCountryPerson
							{
								Postion = a.Field<string>(0),
								UserName = a.Field<string>(1),
								Post = a.Field<string>(2),
								Phone = a.Field<string>(3),
								Remark = a.Field<string>(4)
							};
			return execlList.ToList();
		}
		#endregion

		#region 核对有没有重复或者相同的数据
		private bool CheckInserExecl(List<InsertExeclCountryPerson> insertExeclCountryPersonList)
		{
			//判断list中有没有相同的数值
			if (insertExeclCountryPersonList.Distinct<InsertExeclCountryPerson>().Count() < insertExeclCountryPersonList.Count())
				return false;
			else
				return true;
		}
		#endregion
		#region 检查有没有空数据，有返回null，没有生成list
		private List<CountryPerson> GetCountryPerson(List<InsertExeclCountryPerson> insertExeclCountryPersonList, CountryImportExcel request,int checkCount)
		{
			var countryPersonList = new List<CountryPerson>();
			if (!CheckInserExecl(insertExeclCountryPersonList))
			{
				return null;
			}
			else
			{
				foreach (var insertExecl in insertExeclCountryPersonList)
				{
					var countryPerson = new CountryPerson();
					if (!string.IsNullOrEmpty(insertExecl.UserName) && !string.IsNullOrEmpty(insertExecl.Phone) && !string.IsNullOrEmpty(insertExecl.Postion))
					{
						countryPerson.UserName = insertExecl.UserName;
						countryPerson.Position = insertExecl.Postion;
						countryPerson.Post = insertExecl.Post;
						countryPerson.Remark = insertExecl.Remark;
						countryPerson.CreateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
						countryPerson.CreateName = insertExecl.UserName;
						countryPerson.Phone = insertExecl.Phone;
						countryPerson.UpdateName = insertExecl.UserName;
						countryPerson.Year = request.year;
						countryPerson.Country = RealName;
						countryPerson.adcd = adcd;
						countryPerson.AuditNums = checkCount;
						countryPerson.UpdateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
						countryPersonList.Add(countryPerson);
					}
					else
					{
						return null;
					}
				}
				return countryPersonList;
			}
		}
		#endregion

		#region 县级防汛防台体系列表
		public BsTableDataSource<CountryPersonServiceModel> GetCountryPersonList(GetCountryPersonList request)
		{
			using (var db = DbFactory.Open())
			{
				if (request.year == null)
					throw new Exception("年度异常");
				var bulider = db.From<CountryPerson>();
				//搜索部分判断

				if (!string.IsNullOrEmpty(request.adcd))
					bulider.Where(x => x.adcd == request.adcd);
				if(null != request.nums && request.nums > 1)
					bulider.Where(x => x.AuditNums == request.nums);
				if (!string.IsNullOrEmpty(request.Id.ToString())&&request.Id!=0)
					bulider.And(x => x.Id == request.Id);
				if (!string.IsNullOrEmpty(request.name))
					bulider.And(x => x.UserName == request.name);
				if (!string.IsNullOrEmpty(request.Position))
					bulider.And(x => x.Position == request.Position);
				if (!string.IsNullOrEmpty(request.Post))
					bulider.And(x=>x.Post==request.Post);

				if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
					bulider.OrderBy(x => request.Sort);
				else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
					bulider.OrderByDescending(x => request.Sort);
				else
					bulider.OrderByDescending(x => x.UpdateTime);

				bulider.And(w=>w.Year == request.year);
				var count = db.Count(bulider);
				var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
				var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
				bulider.Limit(PageIndex, PageSize);
				var list = db.Select<CountryPersonServiceModel>(bulider);
				return new BsTableDataSource<CountryPersonServiceModel> { rows = list, total = count };
			}
		}
		public BsTableDataSource<CountryPersonServiceModel> GetCountryPersonList1(GetCountryPersonList1 request)
		{
			using (var db = DbFactory.Open())
			{
				if (string.IsNullOrEmpty(request.adcd)) throw new Exception("adcd异常！");
				var bulider = db.From<CountryPerson>();
				if (!string.IsNullOrEmpty(request.adcd))
					bulider.Where(x => x.adcd == request.adcd);
				#region
				if (!string.IsNullOrEmpty(request.post))
					bulider.Where(w=>w.Position.Contains(request.post));
				if (!string.IsNullOrEmpty(request.position))
					bulider.Where(w => w.Post.Contains(request.position));
				if (!string.IsNullOrEmpty(request.name))
					bulider.Where(w => w.UserName.Contains(request.name));
				#endregion
				var _year = request.year == null ? DateTime.Now.Year : request.year;
				bulider.And(x=>x.Year==_year);
				bulider.Limit(0, 9999);
				
				//var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
				//var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
				//bulider.Limit(PageIndex, PageSize);
					
				
				var list = db.Select<CountryPersonServiceModel>(bulider);
				var newlist = list.Select(w=>w.UserName).Distinct().ToList();
			   
				List<CountryPersonServiceModel> rlist = new List<CountryPersonServiceModel>();
				var city = db.Single<ADCDInfo>(w=>w.adcd == request.adcd.Substring(0,4)+"00000000000");
				newlist.ForEach(w =>
				{
					var f = list.Where(x => x.UserName == w).ToList();
					var newpost = "";var phones = "";
					var gwid=0;
					f.ForEach(y =>
					{
						newpost += y.Position + "_" + y.Post + ";";
						phones += y.Phone + ";";
						if (y.Position == "指挥")
						{
							gwid = 1;
						}
						if (y.Position == "副指挥")
						{
							gwid = 2;
						}
						if (y.Position == "成员")
						{
							gwid = 3;
						}
						if (y.Position == "综合组")
						{
							gwid = 4;
						}
						if (y.Position == "监测预警组")
						{
							gwid = 5;
						}
						if (y.Position == "人员转移组")
						{
							gwid = 6;
						}
						if (y.Position == "抢险救援组")
						{
							gwid = 7;
						}
						if (y.Position == "宣传报道组")
						{
							gwid = 8;
						}
						if (y.Position == "后勤服务组")
						{
							gwid = 9;
						}
					});
					var rphones = phones.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
					CountryPersonServiceModel cpsm = new CountryPersonServiceModel() {
						UserName = w,
						Id = f.FirstOrDefault().Id,
						Phone = string.Join(";", rphones),
						Post = newpost.TrimEnd(';'),
						adcd=f.FirstOrDefault().adcd,
						adnmparent=city.adnm,

						Gwid=gwid,

						Position = f.FirstOrDefault().Position
					};
					var builder1 = db.From<SpotCheck>();
					builder1.Where(y=> y.adcd == request.adcd && y.year == _year && y.bycheckman == cpsm.UserName && y.bycheckphone == cpsm.Phone).OrderByDescending(o=>o.checktime);
					
					
					//分页
					
					var fbycheckman = db.Single(builder1);
					if (fbycheckman != null) { cpsm.checkresult = fbycheckman.checkstatus + "(" + Convert.ToDateTime(fbycheckman.checktime).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
					else { cpsm.checkresult = "-"; }
					rlist.Add(cpsm);
				});
				return  new BsTableDataSource<CountryPersonServiceModel> { rows = rlist, total = rlist.Count() };
			}
		}
		#endregion

		public ExportExeclCountryPerson GetExportExecl(GetExportExecl requset)
		{
			using (var db=DbFactory.Open())
			{
				var info = new ExportExeclCountryPerson();
				var filesavepath = "Upload/Export";
				var name = DateTime.Now.ToString("yyyyMMddHHmmssfff");// DateTime.Now.Ticks.ToString();
				var fileSrc = "";
				var filePath = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath);

				if (!Directory.Exists(filePath))
					Directory.CreateDirectory(filePath); //创建该文件夹
				var createxcel = new creatExcel();

				var dt = new DataTable();//转成Excel导出使用
				var arr = new ArrayList();//不需要导出的列 索引添加到ArrayList里  从0开始
				var sheetName = "";//工作表名
				var arrs = new string[] { };
				var arrName = new string[] { };
				if (requset.typeId == 6)
				{
					sheetName = requset.year + "县级防汛防台责任人";
					var bulider = db.From<CountryPerson>().Where(x => x.Year == requset.year).LeftJoin<CountryPerson, UserInfo>((x, y) => x.Country == y.RealName).And(x => x.Country == RealName);
					var list = db.Select<CountryPersonServiceModel>(bulider);
					if (list.Count > 0)
					{
						arr.Clear();
						arr.Add(0);
						arr.Add(6);
						//list转table
						dt = Common.ListHelper.ListToDataTable(list,arr);
						arrs = ConfigurationManager.AppSettings["添加县级防汛防台责任人"].Split('|');
						arrName = arrs[0].Split(',');
						//创建table的头 
						if (dt.Columns.Contains("CreateTime")) dt.Columns.Remove("CreateTime");
						if (dt.Columns.Contains("checkresult")) dt.Columns.Remove("checkresult");
						if (dt.Columns.Contains("adnmparent")) dt.Columns.Remove("adnmparent");
						if (dt.Columns.Contains("adnm")) dt.Columns.Remove("adnm");
						if (dt.Columns.Contains("adcd")) dt.Columns.Remove("adcd");
						for (var i = 0; i < dt.Columns.Count; i++)
						{
							dt.Columns[i].Caption = arrName[i];
						}
					}
					else
					{
						info.IsSuccess = false;
						info.DownUrl = "";
						return info;
					}
					var path = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath + "/" + sheetName + "-" + name + ".xls");
					fileSrc = filesavepath + "/" + sheetName + "-" + name + ".xls";
					byte[] data = createxcel.DataTableToExcel(dt, sheetName,requset.mergeCellNum, requset.mergeCellContent);

					if (!File.Exists(path))
					{
						FileStream fs = new FileStream(path, FileMode.CreateNew);
						fs.Write(data, 0, data.Length);
						fs.Close();
						info.DownUrl= fileSrc;
						info.IsSuccess = true;
					}
					return info;
				}
				else
				{
					info.IsSuccess = false;
					info.DownUrl = "";
					return info;
				}
			}
		}
		#region 保存或者更改信息
		public bool SaveCountryPerson(SaveCountryPerson requset)
		{
			using (var db = DbFactory.Open())
			{
				if (string.IsNullOrEmpty(adcd))
				{ throw new Exception("请重新登录"); }
				if (!ValidatorHelper.IsMobile(requset.Mobile))
					throw new Exception("手机号码输入错误，请重新输入");
				var info = new CountryPerson();
				var log = new operateLog();
				log.userName = RealName;
				log.operateTime = DateTime.Now;
				if (requset.Id != 0)
				{
					info.Id = requset.Id;
					//log.operateMsg = "更新" + requset.name + "的镇级责任人信息";
				}
				//else
				//{
				//    var model = GetCountryPersonInfo(requset.name);
				//    if (model != null)
				//        throw new Exception("已存在");
				//    //log.operateMsg = "新增" + requset.name + "的镇级责任人信息";
				//}
				info.UserName = requset.name;
				info.CreateTime = DateTime.Now;
				info.Phone = requset.Mobile;
				info.Position = requset.Position;
				info.Post = requset.Post;
				info.Year = DateTime.Now.Year;
				info.Remark = requset.Remark;
				info.Country = RealName;
				info.UpdateName = requset.name;
				info.UpdateTime = DateTime.Now;
				info.CreateName = requset.name;
				info.adcd = adcd;
				var oldInfo = db.Single<CountryPerson>(x=>x.Id==requset.Id);
				if (requset.Id != 0)
				{
					info.AuditNums = oldInfo.AuditNums + 1;
					info.OldData = JsonTools.ObjectToJson(oldInfo);
					info.NewData = JsonTools.ObjectToJson(info);
					#region 日志
					try
					{
						var adcdInfo = db.Single<ADCDInfo>(w=>w.adcd==oldInfo.adcd);
						StringBuilder sb = new StringBuilder();
						sb.Append("在栏目{组织责任/县级防汛防台责任人}下,更新数据{");
						sb.Append("县级adcd：" + adcdInfo.adnm + "");
						sb.Append("姓名：" + oldInfo.UserName + "");
						sb.Append("创建的时间：" + oldInfo.CreateTime + "");
						sb.Append("电话：" + oldInfo.Phone + "");
						sb.Append("岗位：" + oldInfo.Position + "");
						sb.Append("职务：" + oldInfo.Post + "");
						sb.Append("年份：" + oldInfo.Year + "");
						sb.Append("标记：" + oldInfo.Remark + "");
						sb.Append("县的名字：" + oldInfo.Country + "");
						sb.Append("更改人的名字：" + oldInfo.UpdateName + "");
						sb.Append("更改的时间：" + oldInfo.UpdateTime + "");
						sb.Append("创建的人：" + oldInfo.CreateName + "");
						sb.Append("}为{");
						sb.Append("县级adcd：" + adcdInfo.adnm + "");
						sb.Append("姓名：" + info.UserName + "");
						sb.Append("创建的时间：" + info.CreateTime + "");
						sb.Append("电话：" + info.Phone + "");
						sb.Append("岗位：" + info.Position + "");
						sb.Append("职务：" + info.Post + "");
						sb.Append("年份：" + info.Year + "");
						sb.Append("标记：" + info.Remark + "");
						sb.Append("县的名字：" + info.Country + "");
						sb.Append("更改人的名字：" + info.UpdateName + "");
						sb.Append("更改的时间：" + info.UpdateTime + "");
						sb.Append("创建的人：" + info.CreateName + "");
						sb.Append("}");
						logHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
					}catch(Exception ex) { }
					#endregion
					//"select * from CountryPerson where Phone='" + info.Phone + "' and Position='" + info.Position + "' and UserName='" + info.UserName + "' and "
					var r = db.Single<CountryPerson>(w => w.Phone == requset.Mobile && w.Position == info.Position && w.UserName == info.UserName && w.Remark == info.Remark && w.Post == info.Post);
					if (r != null)
						return false;
					else
					return db.Update(info)==1;
				}
				else
				{
					int count;
					var list = db.Select<AuditCounty>("select * from dbo.AuditCounty where CountyADCD='" + adcd + "' ");
					if (list.Count == 0)
					{
						count = 1;
					}
					else
					{
						count=Convert.ToInt32(list[0].AuditNums) + 1;
					}
					info.AuditNums =count ;
					info.OldData = "";
					info.NewData = "";
					#region 日志
					try
					{
						StringBuilder sb = new StringBuilder();
						var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == info.adcd);
						sb.Append("在栏目{组织责任/县级防汛防台责任人}下,新增数据{");
						sb.Append("县级adcd：" + adcdInfo.adnm + "");
						sb.Append("姓名：" + info.UserName + "");
						sb.Append("创建的时间：" + info.CreateTime + "");
						sb.Append("电话：" + info.Phone + "");
						sb.Append("岗位：" + info.Position + "");
						sb.Append("职务：" + info.Post + "");
						sb.Append("年份：" + info.Year + "");
						sb.Append("标记：" + info.Remark + "");
						sb.Append("县的名字：" + info.Country + "");
						sb.Append("更改人的名字：" + info.UpdateName + "");
						sb.Append("更改的时间：" + info.UpdateTime + "");
						sb.Append("创建的人：" + info.CreateName + "");
						sb.Append("}");
						logHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
					}catch(Exception ex)
					{

					}
					#endregion
					if (db.Select<CountryPerson>("select * from CountryPerson where Phone='" + info.Phone + "' and Position='" + info.Position + "' and UserName='" + info.UserName + "'").ToList().Count == 1)
						return false;
					else
						return db.Insert(info) == 1;
				}
				   

			}
		}
		#endregion
		#region 获取当前的对象
		private CountryPerson GetCountryPersonInfo(string name)
		{
			using (var db = DbFactory.Open())
			{
				return db.Single<CountryPerson>(x=>x.UserName==name&&x.Country==RealName);
			}       
		}
		#endregion
		#region 删除文件
		public bool DelectCountryPerson(DeleteCountryPerson request)
		{
			using (var db=DbFactory.Open())
			{
				ArrayList arr = new ArrayList();
				string[] arrs = request.ids.Split(',');
				for (int i = 0; i < arrs.Length; i++)
				{
					var id = int.Parse(arrs[i]);
					arr.Add(id);
					#region 日志
					try
					{
						var info = db.Single<CountryPerson>(w => w.Id == id);
						var adcdInfo = db.Single<ADCDInfo>(w=>w.adcd==info.adcd);
						StringBuilder sb = new StringBuilder();
						sb.Append("在栏目{组织责任/县级防汛防台责任人}下,删除数据{");
						sb.Append("县级adcd：" + adcdInfo.adnm + "");
						sb.Append("姓名：" + info.UserName + "");
						sb.Append("创建的时间：" + info.CreateTime + "");
						sb.Append("电话：" + info.Phone + "");
						sb.Append("岗位：" + info.Position + "");
						sb.Append("职务：" + info.Post + "");
						sb.Append("年份：" + info.Year + "");
						sb.Append("标记：" + info.Remark + "");
						sb.Append("县的名字：" + info.Country + "");
						sb.Append("更改人的名字：" + info.UpdateName + "");
						sb.Append("更改的时间：" + info.UpdateTime + "");
						sb.Append("创建的人：" + info.CreateName + "");
						sb.Append("}");
						logHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
					}catch(Exception ex)
					{

					}
					#endregion
				}
				return db.Delete<CountryPerson>(x => Sql.In(x.Id, arr)) > 0;
			}
		}
		#endregion
		#region 提交审核的逻辑---写的有点乱后期优化
		public ReturnCountyCheck AddCheck(AddCountryCheck requset)
		{
			using (var db = DbFactory.Open())
			{
				if (string.IsNullOrEmpty(adcd))
				{ throw new Exception("请重新登录"); }
				var auditCounty = new AuditCounty();
				var returnCountryCheck = new ReturnCountyCheck();
				
				auditCounty.Year = requset.year;
				auditCounty.CountyADCD = adcd;
				auditCounty.CountyAddTime = DateTime.Now;
				var oldAuditCountry = db.Single<AuditCounty>(x=>x.CountyADCD==adcd);
				int auditnum = 1;
				if (null != oldAuditCountry) auditnum = oldAuditCountry.AuditNums.Value + 1;
				//查询数据有没有更改
				var bulid = db.From<CountryPerson>().Where(x => x.adcd == adcd).And(x => x.AuditNums == auditnum);
				var list = db.Select<CountryPerson>(bulid);
				//2种状态一种是第一次提交另外就是第二次提交
				if (oldAuditCountry!=null)
				{
					//审核不通过的2种状态一种更改过数据一种没更改数据
					if (oldAuditCountry.Status == -1)
					{                        
						if (list.Count ==0)
						{
							returnCountryCheck.IsSuccess = false;
							returnCountryCheck.CheckStatus = oldAuditCountry.Status;
							returnCountryCheck.CheckSuggest = db.Select<AuditCountyDetails>("select Remarks,max(AuditTime) from dbo.AuditCountyDetails where  CountyID='" + oldAuditCountry.ID + "' group by Remarks ")[0].Remarks;
							return returnCountryCheck;
						}
					}
					//审核种
					else if(oldAuditCountry.Status==1)
					{
						returnCountryCheck.IsSuccess = false;
						returnCountryCheck.CheckStatus = oldAuditCountry.Status;
						returnCountryCheck.CheckSuggest = "";
						return returnCountryCheck;
					}
					auditCounty.AuditNums = oldAuditCountry.AuditNums + 1;
					auditCounty.Status = 1;
					auditCounty.operateLog = null;
					auditCounty.ID = db.Select<AuditCounty>("select * from dbo.AuditCounty  where CountyADCD='"+adcd+"' and Year='"+DateTime.Now.Year+"'")[0].ID;
				}
				else
				{
					var bulidFirst = db.From<CountryPerson>().Where(x => x.adcd == adcd);
					var listFirst = db.Select<CountryPerson>(bulidFirst);
					//第一次没有记录就提交
					if (listFirst.Count == 0)
					{
						returnCountryCheck.IsSuccess = false;
						returnCountryCheck.CheckStatus = 3;
						returnCountryCheck.CheckSuggest = "提交后未更改数据";
						return returnCountryCheck;
					}
					//第一次提交
					auditCounty.AuditNums = 1;
					auditCounty.Status = 1;
					auditCounty.operateLog = null;
					if (db.Insert<AuditCounty>(auditCounty) == 1)
					{
						returnCountryCheck.IsSuccess = true;
						returnCountryCheck.CheckStatus = 3;
						returnCountryCheck.CheckSuggest = "";
						#region 日志
						StringBuilder sb = new StringBuilder();
						sb.Append("在栏目{组织责任/县级防汛防台责任人}下,{"+RealName+"}第{"+auditCounty.AuditNums+"}次提交了责任人审核申请");
						logHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
						#endregion
					}
					else
					{
						returnCountryCheck.IsSuccess = false;
						returnCountryCheck.CheckStatus = 3;
						returnCountryCheck.CheckSuggest = "";
					}
					return returnCountryCheck;
				}
				//审核失败未更改数据
				if (list.Count == 0)
				{
					returnCountryCheck.IsSuccess = false;
					returnCountryCheck.CheckStatus = 3;
					returnCountryCheck.CheckSuggest = "提交后未更改数据";
					return returnCountryCheck;
				}
				//更新数据库
				if (db.Update<AuditCounty>(auditCounty) ==1)
				{
					returnCountryCheck.IsSuccess = true;
					returnCountryCheck.CheckStatus = 3;
					returnCountryCheck.CheckSuggest = "";
					#region 日志
					StringBuilder sb = new StringBuilder();
					sb.Append("在栏目{组织责任/县级防汛防台责任人}下,{" + RealName + "}第{" + auditCounty.AuditNums + "}次提交了责任人审核申请");
					logHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
					#endregion
				}
				else
				{
					 returnCountryCheck.IsSuccess = false;
					 returnCountryCheck.CheckStatus = 3;
					 returnCountryCheck.CheckSuggest = "";                    
				}
				return returnCountryCheck;
			}
		}
		#endregion
		#region 核对审核的状态
		public ReturnCountyCheck CountyCheck(CountryStatus requset)
		{
			using (var db = DbFactory.Open())
			{
				var build = db.From<AuditCounty>();
				var returnCountryCheck = new ReturnCountyCheck();
				build = build.Where(x => x.Year == DateTime.Now.Year).And(y=>y.CountyADCD==adcd);
				var list = db.Select<AuditCounty>(build);
				if (list.Count>0)
				{
					//-1审核失败增加失败原因否则没有失败原因
					if (list[0].Status == -1)
					{
						returnCountryCheck.CheckStatus = list[0].Status;
						returnCountryCheck.CheckSuggest = db.Select<AuditCountyDetails>("select Remarks,max(AuditTime) from dbo.AuditCountyDetails where  CountyID='" + list[0].ID + "' group by Remarks ")[0].Remarks;
						returnCountryCheck.IsSuccess = false;
					}
					else
					{
						returnCountryCheck.CheckStatus = list[0].Status;
						returnCountryCheck.CheckSuggest = "";
						returnCountryCheck.IsSuccess = false;
					}               
				}
				else
				{
					returnCountryCheck.CheckStatus = 2;
					returnCountryCheck.CheckSuggest = "";
					returnCountryCheck.IsSuccess = false;
				}              
				return returnCountryCheck;
			}
		}
		#endregion
	}
}

