using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using ServiceStack;
using Dy.Common;
using GrassrootsFloodCtrl.Logic.Common;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.DangerZone;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Excel;
using GrassrootsFloodCtrl.ServiceModel.Route;
using NPOI.HSSF.UserModel;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Grid;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.ServiceModel.Post;
using GrassrootsFloodCtrl.Logic.Village;
using GrassrootsFloodCtrl.Logic.Town;
using GrassrootsFloodCtrl.Model.CountryPerson;
using GrassrootsFloodCtrl.Logic.Country;
using System.Text;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using Aspose.Cells;

namespace GrassrootsFloodCtrl.Logic.Excel
{
    public class ExcelManager : ManagerBase, IExcelManager
    {
        public IZZTXManager ZZTXManager { get; set; }
        public IVillageTransferPersonManager VillageTransferPersonManager { get; set; }
        public ITownManager TownManager { get; set; }
        public ILogHelper _ILogHelper { get; set; }

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UploadFileViewModel downLoadExcelModel(downLoadExcelModel request)
        {
            using (var db = DbFactory.Open())
            {
                var createxcel = new creatExcel();
                UploadFileViewModel _ufvm = new UploadFileViewModel();
                var name = DateTime.Now.ToString("yyyyMMddhhmmss") + new Random(DateTime.Now.Second).Next(10000);
                var filesavepath = "Files";

                /*******************************/
                HSSFWorkbook workBook = new HSSFWorkbook();
                var cunList = new List<string>();
                #region 获取配置的表头以及表头中第几个为下拉选择

                var fileds = "";
                switch (request.typeId)
                {
                    //下载的类型（0:行政村信息，1：行政村防汛防台工作组，2：行政村网格，3：行政村转移人员清单，4：镇级防汛防台责任，，6.添加县级城防体系人员）
                    case 0: //行政村受灾信息
                        fileds = ConfigurationManager.AppSettings["行政村受灾信息"];
                        filesavepath = "Files/village";
                        name = "行政村受灾信息" + name;
                        cunList = db.Select<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd).Select(x => x.adnm).ToList();

                        break;
                    case 1: //行政村防汛防台工作组
                        fileds = ConfigurationManager.AppSettings["行政村防汛防台工作组"];
                        filesavepath = "Files/workinggroup";
                        name = "行政村防汛防台工作组" + name;
                        break;
                    case 2: //行政村网格
                        fileds = ConfigurationManager.AppSettings["行政村网格责任人"];
                        filesavepath = "Files/grid";
                        name = "行政村网格责任人" + name;
                        break;
                    case 3: //行政村转移人员清单
                        fileds = ConfigurationManager.AppSettings["行政村危险区人员转移清单"];
                        filesavepath = "Files/transferPerson";
                        name = "行政村危险区人员转移清单" + name;
                        cunList= db.Select<ADCDInfo>(x=>x.adcd.StartsWith(adcd.Substring(0,9))&&x.adcd!=adcd).Select(x => x.adnm).ToList();

                        break;
                    case 4: //镇级防汛防台责任
                        fileds = ConfigurationManager.AppSettings["镇级防汛防台责任人"];
                        filesavepath = "Files/town";
                        name = "乡(镇、街道)防汛防台责任人" + name;
                        break;
                    case 5: //行政村信息
                        fileds = ConfigurationManager.AppSettings["行政村信息"];
                        filesavepath = "Files/Adcd";
                        name = "行政村信息" + name;
                        cunList = db.Select<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd).Select(x => x.adnm).ToList();
                        break;
                    case 6:
                        fileds = ConfigurationManager.AppSettings["添加县级防汛防台责任人"];
                        
                        filesavepath = "Files/CountryPerson";
                        name = "添加县级防汛防台责任人" + name;
                        break;
                    default:
                        throw new Exception("下载类型错误");
                }
                var filePath = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath);
                var path = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath + "/" + name + ".xls");
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath); //创建该文件夹
                //需要下拉的表头
                var list = new Dictionary<int, string>();
                //表头
                List<string> listTitle = new List<string>();
                if (!string.IsNullOrEmpty(fileds))
                {
                    var arrs = fileds.Split('|');
                    var arr = arrs[0].Split(',');

                    if (arrs.Length > 1)
                    {
                        var arrDropDownList = arrs[1].Split(',');
                        for (var i = 0; i < arrDropDownList.Length; i++)
                        {
                            var index = dyConverter.ToInt32(arrDropDownList[i]);
                            list.Add(index, arr[index]);
                        }
                    }
                    for (var i = 0; i < arr.Length; i++)
                    {
                        listTitle.Add(arr[i]);
                    }
                }
                else
                    throw new Exception("配置异常！");

                #endregion

                var sheet = createxcel.createSheet1(workBook, request.fileName, request.mergeCellContent,
                    request.mergeCellNum - 1, listTitle);

                #region 设置下拉框 （目前只设置了岗位，需要的可以在继续添加。如：网格类型）

                foreach (var dic in list)
                {
                    var strList = new List<string>();
                    var value = dic.Value;
                    switch (value)
                    {
                        case "岗位":
                           if(request.typeId == 4)
                                strList = db.Select<Model.Post.Post>(x=>x.PostType==GrassrootsFloodCtrlEnums.ZZTXEnums.镇级防汛防台责任人.ToString()).Select(x => x.PostName).ToList();
                           else if(request.typeId == 6)
                                strList = db.Select<Model.Post.Post>(x => x.PostType == GrassrootsFloodCtrlEnums.ZZTXEnums.县级防汛防台责任人.ToString()).Select(x => x.PostName).ToList();
                            else
                            strList = db.Select<Model.Post.Post>().Select(x => x.PostName).ToList();
                            break;
                        case "网格类型":
                            //var builder = db.From<Model.Post.Post>();
                            //strList = db.Select(builder).Select(x => x.PostName).ToList();
                            break;
                        case "危险区类型":
                            strList = db.Select<DangerZone>().Select(x => x.DangerZoneName).ToList();
                            break;
                        case "防汛任务轻重":
                            strList.Add(FXFTRW.较轻.ToString());
                            strList.Add(FXFTRW.较重.ToString());
                            break;
                    }

                    createxcel.setSheet2(workBook, sheet, dic.Value, dic.Key, strList);
                }

                #endregion


                if (request.typeId == 0|| request.typeId == 3|| request.typeId == 5)
                {
                    createxcel.setSheet3(workBook, "行政村名录",  cunList);
                }
                using (FileStream file = new FileStream(path, FileMode.Create))
                {
                    workBook.Write(file); //创建Excel文件。
                    file.Close();
                }
                _ufvm.fileSrc = filesavepath + "/" + name + ".xls";
                _ufvm.isSuccess = true;
                _ufvm.fileName = request.fileName;
                /*******************************/
                return _ufvm;
            }
        }
        
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExcelViewModel ImportExcel(ImportExcel request)
        {
            using (var db = DbFactory.Open())
            {
                var info = new ExcelViewModel();
                var dic = new Dictionary<string, string>();
                var dicList=new  List<Dictionary<string, string>>();
                   var newpath = System.Web.HttpContext.Current.Server.MapPath(request.filePath);
                try
                {
                    //导入数据
                    //var dt = Common.ExcelHelper.GetDataTable(newpath);
                    Workbook workbook = new Workbook();
                    workbook.Open(newpath);
                    Cells cells = workbook.Worksheets[0].Cells;
                    var dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);

                    if (dt.Columns[0].ToString().Trim().IndexOf("请")<0)
                        info.Total = dt.Rows.Count;
                    else
                        info.Total = dt.Rows.Count - 1;
                    var successCount = 0;//成功的条数
                    var failCount = 0;//失败的条数
                    //存储第一行表头
                    var drList = new List<string>();
                    var titleDr = dt.Rows[0];
                    var itemCount = dt.Columns.Count;
                    for (var j = 0; j < itemCount; j++)
                    {
                        drList.Add(titleDr[j].ToString());
                    }

                    //移出表头
                    if (dt.Columns[0].ToString().Trim().IndexOf("请") >= 0)
                        dt.Rows.RemoveAt(0);

                    #region 基础数据集 


                    //取出网格
                    var builderGrid = db.From<Model.Post.Post>();
                    var postList = db.Select<PostViewModel>(builderGrid);

                    //取出岗位
                    var builderPost = db.From<Model.Grid.Grid>();
                    var gridList = db.Select<GridViewModel>(builderPost);

                    //取出危险区类型
                    var builderDangerZone = db.From<DangerZone>();
                    var dangerZoneList = db.Select(builderDangerZone);

                    //取出adcd
                    #region 取出adcd
                    var builderadcd = db.From<ADCDInfo>();
                    if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        builderadcd.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) );//&& x.adcd != adcd.ToString()
                    else
                    {
                        throw new Exception("登陆用户的所属行政区划编码不正确");
                    }
                    var adcdList = db.Select<VillageViewModel>(builderadcd);
                    #endregion

                    var ADCDInfoList = new List<ADCDInfo>();//行政村
                    var ADCDDisasterInfoList = new List<ADCDDisasterInfo>();//行政村受灾信息
                    var VillageWorkingGroupList = new List<VillageWorkingGroup>();//行政村防汛防台工作组
                    var VillageGridPersonLiableList = new List<VillageGridViewModel>();//行政村网格
                    var VillageTransferPersonList = new List<VillageTransferPerson>();//行政村危险区转移人员清单
                    var TownPersonLiableList = new List<TownPersonLiable>();//镇级防汛防台责任人
                    var CountryPersonList = new List<CountryPerson>();//县级城防体系责任人
                    //取出责任人
                    #region 取出责任人

                    var builder = db.From<ADCDInfo>();
                    if (request.typeId == 0)
                    {
                        builder.LeftJoin<ADCDInfo, ADCDDisasterInfo>((x, y) => x.adcd == y.adcd);
                        if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        {
                            builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd.ToString());
                        }
                        else
                        {
                            throw new Exception("登陆用户的所属行政区划编码不正确");
                        }
                        builder.Where<ADCDDisasterInfo>(y => y.adcd != null && y.Year == request.year);
                        ADCDDisasterInfoList = db.Select<ADCDDisasterInfo>(builder);
                    }
                    else if (request.typeId == 1)
                    {
                        builder.LeftJoin<ADCDInfo, VillageWorkingGroup>((x, y) => x.adcd == y.VillageADCD);
                        if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        {
                            builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd.ToString());
                        }
                        else
                        {
                            throw new Exception("登陆用户的所属行政区划编码不正确");
                        }
                        builder.Where<ADCDDisasterInfo>(y => y.adcd != null && y.Year == request.year);
                        VillageWorkingGroupList = db.Select<VillageWorkingGroup>(builder);

                    }
                    else if (request.typeId == 2)
                    {
                        builder.LeftJoin<ADCDInfo, VillageGridPersonLiable>((x, y) => x.adcd == y.VillageADCD);
                        if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        {
                            builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd.ToString());
                        }
                        else
                        {
                            throw new Exception("登陆用户的所属行政区划编码不正确");
                        }
                        builder.Where<VillageGridPersonLiable>(y => y.VillageADCD != null && y.Year == request.year);
                        VillageGridPersonLiableList = db.Select<VillageGridViewModel>(builder);
                    }
                    else if (request.typeId == 3)//行政村危险区转移人员清单
                    {
                        builder.LeftJoin<ADCDInfo, VillageTransferPerson>((x, y) => x.adcd == y.adcd);
                        if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        {
                            builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd.ToString());
                        }
                        else
                        {
                            throw new Exception("登陆用户的所属行政区划编码不正确");
                        }
                        builder.Where<VillageTransferPerson>(y => y.adcd != null && y.Year == request.year);
                        VillageTransferPersonList = db.Select<VillageTransferPerson>(builder);
                    }
                    else if (request.typeId == 4)//镇级防汛防台责任人
                    {
                        builder.LeftJoin<ADCDInfo, TownPersonLiable>((x, y) => x.adcd == y.adcd);
                        if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        {
                            builder.Where<ADCDInfo>(x => x.adcd == adcd);
                        }
                        else
                        {
                            throw new Exception("登陆用户的所属行政区划编码不正确");
                        }
                        builder.Where<TownPersonLiable>(y => y.adcd != null && y.Year == request.year);
                        TownPersonLiableList = db.Select<TownPersonLiable>(builder);

                    }
                    else if (request.typeId == 5)//行政村
                    {

                        if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        {
                            builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd).OrderByDescending(x => x.adcd);
                        }
                        else
                        {
                            throw new Exception("登陆用户的所属行政区划编码不正确");
                        }
                        ADCDInfoList = db.Select(builder);
                    }
                    else
                        throw new Exception("类型不正确");

                    #endregion

                    #endregion

                    #region 校验
                    int i = 3;
                    foreach (DataRow dr in dt.Rows)
                    {
                        bool flag = true;
                        //村名匹配
                        #region 村名或镇名校验
                        var villageName = dr[0].ToString().Trim();
                        var _adcd = "";
                        if (string.IsNullOrEmpty(villageName))
                        {
                            flag = false;
                            dic = new Dictionary<string, string>();
                            dic.Add(i.ToString(), "第  " + i + "  行村名为空！");
                            dicList.Add(dic);
                            failCount++;
                        }
                        else if(request.typeId!=5)
                        {
                            var model = adcdList.Where(w => w.adnm == villageName).FirstOrDefault();
                            if (null == model || string.IsNullOrEmpty(model.adcd))
                            {
                                flag = false;
                                dic = new Dictionary<string, string>();
                                dic.Add("msg", "第 " + i + " 行 乡镇(街道)名称和系统给出名称不匹配！");
                                dicList.Add(dic);
                                failCount++;
                            }
                            else if (request.typeId == 3 && db.Single<VillageTransferPerson>(w => w.IfTransfer == 1 && w.adcd == model.adcd) != null)
                            {
                                //判断是否有提交过无转移人员清单
                                flag = false;
                                dic = new Dictionary<string, string>();
                                dic.Add("msg", "第 " + i + " 行 村“"+ villageName + "”以前有提交过“无转移人员”操作，请在已上报中删除该村,在导入转移人员");
                                dicList.Add(dic);
                                failCount++;
                            }
                            else
                            {
                                _adcd = model.adcd;
                                flag = true;
                            }
                        }
                        #endregion

                        //网格类型校验

                        #region 网格类型校验

                        var gridName = "";//网格类型
                        if (request.typeId == 2 && flag)
                        {
                            gridName = dr[2].ToString().Trim();
                            if (string.IsNullOrEmpty(gridName))
                            {
                                flag = false;
                                dic = new Dictionary<string, string>();
                                dic.Add(i.ToString(), "第  " + i + "  网格类型名为空！");
                                dicList.Add(dic);
                                failCount++;
                            }
                            else
                            {
                                flag = true;
                            }
                        }

                        #endregion

                        #region 岗位校验

                        var post = "";
                        if (flag &&( request.typeId == 1|| request.typeId == 4))
                        {
                            if (request.typeId == 1 )
                                post = dr[2].ToString().Trim();
                            if ( request.typeId == 4)
                                post = dr[1].ToString().Trim();
                            var postModel = postList.Where(w => w.PostName == post).FirstOrDefault();
                            if (null == postModel && string.IsNullOrEmpty(postModel.PostName))
                            {
                                flag = false;
                                dic = new Dictionary<string, string>();
                                dic.Add("msg", "第 " + i + " 行岗位名称和系统里的岗位名称不匹配！新增岗位请先到“岗位管理”栏目设置后,重新下载模板");
                                dicList.Add(dic);
                                failCount++;
                            }
                            else
                            {
                                flag = true;
                            }
                        }

                        #endregion

                        //责任人
                        #region 责任人校验
                        var personName ="";
                        if (flag&&request.typeId!=0&& request.typeId!=3&&request.typeId!=5)
                        {
                            if (request.typeId == 1)
                                personName = dr[3].ToString().Trim();
                            if (request.typeId == 4)
                                personName = dr[2].ToString().Trim();
                            if (string.IsNullOrEmpty(personName))
                            {
                                flag = false;
                                dic = new Dictionary<string, string>();
                                dic.Add("msg", "第  " + i + "  责任人为空！");
                                dicList.Add(dic);
                                failCount++;
                            }
                            else
                            {
                                flag = true;
                                personName = dr[3].ToString().Trim();
                            }
                        }

                        #endregion

                        #region 危险区类型校验

                        var DangerZoneType = "";
                        if (request.typeId == 3)
                        {
                            DangerZoneType = dr[2].ToString().Trim();                       
                            var dangerZoneModel = dangerZoneList.Where(w => w.DangerZoneName == DangerZoneType).FirstOrDefault();
                            if (null == dangerZoneModel)
                            {
                                flag = false;
                                dic = new Dictionary<string, string>();
                                dic.Add("msg", "第 " + i + " 行危险区类型和系统里的危险区类型不匹配！新增危险区类型请先到“危险区类型管理”栏目设置后,重新下载模板");
                                dicList.Add(dic);
                                failCount++;
                            }
                            else
                            {
                                flag = true;
                            }
                        }

                        #endregion

                        //相同的数据校验
                        #region 相同的数据校验


                        if (flag)
                        {
                            if (request.typeId == 0)//行政村受灾信息
                            {
                                //var adcdDisasterInfo =
                                //    ADCDDisasterInfoList.Where(w => w.adcd == _adcd && w.Year == request.year).ToList();
                                //if (adcdDisasterInfo != null && adcdDisasterInfo.Count > 0)
                                //{
                                //    dic = new Dictionary<string, string>();
                                //    dic.Add(i.ToString(), "第  " + i + " 行，行政村信息的重复数据！");
                                //    dicList.Add(dic);
                                //    failCount++;
                                //}

                                //var adcdDisasterInfo =
                                //    ADCDDisasterInfoList.Where(w => w.adcd == _adcd && w.Year == request.year).ToList();
                                //if (adcdDisasterInfo != null&& adcdDisasterInfo.Count>0)
                                //{
                                //    dic = new Dictionary<string, string>();
                                //    dic.Add(i.ToString(), "第  " + i + " 行，行政村信息的重复数据！");
                                //    dicList.Add(dic);
                                //    failCount++;
                                //}
                            }
                            else if (request.typeId == 1)//防汛防台工作组
                            {
                                var villageWorkingGroupList = VillageWorkingGroupList.Where(w => w.VillageADCD == _adcd && w.Year == request.year && w.Post == post && w.PersonLiable == personName).ToList();
                                if (villageWorkingGroupList != null && villageWorkingGroupList.Count > 0)
                                {
                                    dic = new Dictionary<string, string>();
                                    dic.Add(i.ToString(), "第  " + i + " 行有，同村、同责任人、同岗位的行政村防汛防台工作组重复数据！");
                                    dicList.Add(dic);
                                    failCount++;
                                }
                            }
                            else if (request.typeId == 2)//网格
                            {
                                var villageGridPersonLiableList = VillageGridPersonLiableList.Where(w => w.VillageADCD == _adcd && w.Year == request.year.ToString() && w.GridName == gridName && w.PersonLiable == personName && w.VillageGridName == dr[2].ToString()).ToList();
                                if (villageGridPersonLiableList != null && villageGridPersonLiableList.Count > 0)
                                {
                                    dic = new Dictionary<string, string>();
                                    dic.Add(i.ToString(), "第  " + i + " 行有，同村、同责任人、同网格名、同网格类型的行政村防汛防台工作组重复数据！");
                                    dicList.Add(dic);
                                    failCount++;
                                }
                            }
                            else if (request.typeId == 3)//行政村转移人员清单
                            {
                                //空判断
                                if (string.IsNullOrEmpty(dr[1].ToString().Trim()))
                                {
                                    dic = new Dictionary<string, string>();
                                    dic.Add("msg", "第  " + i + " 行危险区名为空！");
                                    dicList.Add(dic);
                                    failCount++;
                                }
                                else if (string.IsNullOrEmpty(dr[2].ToString().Trim()))
                                {
                                    dic = new Dictionary<string, string>();
                                    dic.Add("msg", "第  " + i + " 行危险区(点)类型为空！");
                                    dicList.Add(dic);
                                    failCount++;
                                }
                                else
                                {
                                    //同值判断
                                    var villageTransferPersonList = VillageTransferPersonList.Where(w => w.adcd == _adcd && w.Year == request.year && w.DangerZoneName == dr[1].ToString().Trim() && w.DangerZoneType == DangerZoneType && w.HouseholderName == dr[4].ToString().Trim()).ToList();
                                    if (villageTransferPersonList != null && villageTransferPersonList.Count > 0)
                                    {
                                        dic = new Dictionary<string, string>();
                                        dic.Add("msg", "第  " + i + " 行有，同村、同危险区名、同危险区类型、同户主姓名的行政村危险区转移人员重复数据！");
                                        dicList.Add(dic);
                                        failCount++;
                                    }
                                }
                            }
                            else if (request.typeId == 4)//镇级防汛防台责任
                            {
                                var townPersonLiableList = TownPersonLiableList.Where(w => w.adcd == _adcd && w.Year == request.year && w.Name == dr[2].ToString().Trim() && w.Position == post).ToList();
                                if (townPersonLiableList != null && townPersonLiableList.Count > 0)
                                {
                                    dic = new Dictionary<string, string>();
                                    dic.Add("msg", "第  " + i + " 行有，同镇街、同责任人、同岗位的镇级防汛防台责任人重复数据！");
                                    dicList.Add(dic);
                                    failCount++;
                                }
                            }
                            else if (request.typeId == 5)//行政村
                            {
                                var adcdInfoList = ADCDInfoList.Where(x => x.adnm == dr[2].ToString().Trim() && x.adcd.StartsWith(adcd.Substring(0, 9))).ToList();
                                if (adcdInfoList != null && adcdInfoList.Count > 0)
                                {
                                    dic = new Dictionary<string, string>();
                                    dic.Add(i.ToString(), "第  " + i + " 行有，相同行政村的重复数据！");
                                    dicList.Add(dic);
                                    failCount++;
                                }
                            }
                        }
                        #endregion
                        i++;
                    }
                    
                    if (failCount > 0)
                    {
                        info.failNum = failCount;
                        info.ErrorList = dicList;
                        return info;
                    }
                    #endregion

                    #region 取出数据写入实体类

                    successCount = 0;//成功的条数
                    failCount = 0;//失败的条数

                    info = new ExcelViewModel();
                    dic = new Dictionary<string, string>();
                    dicList = new List<Dictionary<string, string>>();
                    info.Total = dt.Rows.Count;
                    
                    StringBuilder sb = new StringBuilder();
                    var name = "";
                    for (var k = 0; k < dt.Rows.Count; k++)
                    {
                        #region 导入数据库

                        var dr = dt.Rows[k];
                        var rowNo = k + 1;//第几行 
                        var msg = "第 "+rowNo+" 行的数据导入失败！";
                        //foreach (DataRow dr in dt.Rows)
                        //{
                        if (request.typeId == 0)
                        {
                            #region 行政村受灾点信息

                            var adcdDisasterInfo = new ADCDDisasterInfo();

                            adcdDisasterInfo.adcd = adcdList.Where<VillageViewModel>(w => w.adnm == dr[0].ToString().Trim()).FirstOrDefault().adcd;
                            adcdDisasterInfo.Year = request.year;
                            adcdDisasterInfo.CreateTime = DateTime.Now;
                            if (!string.IsNullOrEmpty(dr[1].ToString()))
                                adcdDisasterInfo.TotalNum = dyConverter.ToInt32(dr[1].ToString().Trim());
                            if (!string.IsNullOrEmpty(dr[2].ToString()))
                                adcdDisasterInfo.PointNum = dyConverter.ToInt32(dr[2].ToString().Trim());
                            if (!string.IsNullOrEmpty(dr[3].ToString()))
                                adcdDisasterInfo.PopulationNum = dyConverter.ToInt32(dr[3].ToString().Trim());
                            if (!string.IsNullOrEmpty(dr[4].ToString()))
                                adcdDisasterInfo.FXFTRW = dr[4].ToString().Trim();
                            var log = new operateLog();
                            log.userName = RealName;
                            log.operateTime = DateTime.Now;
                            log.operateMsg = "导入" + dr[0].ToString() + "的行政村信息";
                            var logList = new List<operateLog>();
                            logList.Add(log);
                            adcdDisasterInfo.operateLog = JsonTools.ObjectToJson(logList);
                           var model= db.Single<ADCDDisasterInfo>(
                                x => x.adcd == adcdDisasterInfo.adcd && x.Year == adcdDisasterInfo.Year);
                            if (model != null)
                            {
                                adcdDisasterInfo.Id = model.Id;
                                if (db.Update(adcdDisasterInfo) == 1)
                                    successCount++;
                                else
                                {
                                    dic = new Dictionary<string, string>();
                                    dic.Add(rowNo.ToString(), msg);
                                    dicList.Add(dic);
                                    failCount++;
                                }
                            }
                            else
                            {
                                dic = new Dictionary<string, string>(); dic.Add(rowNo.ToString(), msg);
                                dicList.Add(dic);
                                failCount++;
                                if (db.Insert(adcdDisasterInfo) == 1)
                                    successCount++;
                                else
                                {
                                    dic = new Dictionary<string, string>();
                                    dic.Add(rowNo.ToString(), msg);
                                    dicList.Add(dic);
                                    failCount++;
                                }
                            }
                            #endregion
                            name = "行政村受灾点信息";
                        }
                        else if (request.typeId == 3)
                        {
                            #region 行政村危险区转移人员清单

                            var villageTransferPerson = new VillageTransferPerson();

                            villageTransferPerson.adcd = adcdList.Where(w => w.adnm == dr[0].ToString().Trim()).FirstOrDefault().adcd;
                            villageTransferPerson.Year = request.year;
                            villageTransferPerson.CreateTime = DateTime.Now;
                            //if (!string.IsNullOrEmpty(dr[1].ToString()))
                            villageTransferPerson.DangerZoneName = ConverterHelper.ObjectToString(dr[1].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[2].ToString()))
                            villageTransferPerson.DangerZoneType = ConverterHelper.ObjectToString(dr[2].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[3].ToString()))
                            villageTransferPerson.Position = ConverterHelper.ObjectToString(dr[3].ToString().Trim());
                            if (!string.IsNullOrEmpty(dr[4].ToString()))
                                villageTransferPerson.Lng = dyConverter.ToDouble(dr[4].ToString().Trim());
                            if (!string.IsNullOrEmpty(dr[5].ToString()))
                                villageTransferPerson.Lat = dyConverter.ToDouble(dr[5].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[6].ToString()))
                            villageTransferPerson.HouseholderName = ConverterHelper.ObjectToString(dr[6].ToString().Trim());
                            if (!string.IsNullOrEmpty(dr[7].ToString()))
                                villageTransferPerson.HouseholderNum = dyConverter.ToInt32(dr[7].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[8].ToString()))
                            villageTransferPerson.HouseholderMobile = ConverterHelper.ObjectToString(dr[8].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[9].ToString()))
                            villageTransferPerson.PersonLiableName = ConverterHelper.ObjectToString(dr[9].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[10].ToString()))
                            villageTransferPerson.PersonLiablePost = ConverterHelper.ObjectToString(dr[10].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[11].ToString()))
                            villageTransferPerson.PersonLiableMobile = ConverterHelper.ObjectToString(dr[11].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[12].ToString()))
                            villageTransferPerson.WarnPersonLiableName = ConverterHelper.ObjectToString(dr[12].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[13].ToString()))
                            villageTransferPerson.WarnPersonLiablePost = ConverterHelper.ObjectToString(dr[13].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[14].ToString()))
                            villageTransferPerson.WarnPersonLiableMobile = ConverterHelper.ObjectToString(dr[14].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[15].ToString()))
                            villageTransferPerson.DisasterPreventionName = ConverterHelper.ObjectToString(dr[15].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[16].ToString()))
                            villageTransferPerson.SafetyIdentification = dr[16].ToString().Trim() == "有";
                            //if (!string.IsNullOrEmpty(dr[17].ToString()))
                            villageTransferPerson.DisasterPreventionManager = ConverterHelper.ObjectToString(dr[17].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[18].ToString()))
                            villageTransferPerson.DisasterPreventionManagerMobile = ConverterHelper.ObjectToString(dr[18].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[19].ToString()))
                            villageTransferPerson.Remark = ConverterHelper.ObjectToString(dr[19].ToString().Trim());
                            if (null != AuditNums && AuditNums.Value > 1) villageTransferPerson.AuditNums = AuditNums.Value + 1;
                            var log = new operateLog();
                            log.userName = RealName;
                            log.operateTime = DateTime.Now;
                            log.operateMsg = "导入危险区名为：" + villageTransferPerson.DangerZoneName + "，类型为：" + villageTransferPerson.DangerZoneType + "，户主为：" + villageTransferPerson.HouseholderName + "的危险区转移人员信息";
                            var logList = new List<operateLog>();
                            logList.Add(log);
                            villageTransferPerson.operateLog = JsonTools.ObjectToJson(logList);
                            if (db.Insert(villageTransferPerson) == 1)
                                successCount++;
                            else
                            {
                                dic = new Dictionary<string, string>(); dic.Add(rowNo.ToString(), msg);
                                dicList.Add(dic);
                                failCount++;
                            }
                            #endregion
                         
                            name = "行政村危险区转移人员清单";
                        }
                        else if (request.typeId == 4)
                        {
                            #region 镇级防汛防台责任人

                            var townPersonLiable = new TownPersonLiable();

                            townPersonLiable.adcd = adcdList.Where(w => w.adnm == dr[0].ToString().Trim()).FirstOrDefault().adcd;
                            townPersonLiable.Year = request.year;
                            townPersonLiable.CreateTime = DateTime.Now;
                            //if (!string.IsNullOrEmpty(dr[1].ToString()))
                            townPersonLiable.Position = ConverterHelper.ObjectToString(dr[1].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[2].ToString()))
                            townPersonLiable.Name = ConverterHelper.ObjectToString(dr[2].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[3].ToString()))
                            townPersonLiable.Post = ConverterHelper.ObjectToString(dr[3].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[4].ToString()))
                            townPersonLiable.Mobile = ConverterHelper.ObjectToString(dr[4].ToString().Trim());
                            //if (!string.IsNullOrEmpty(dr[5].ToString()))
                            townPersonLiable.Remark = ConverterHelper.ObjectToString(dr[5].ToString().Trim());
                            if (null != AuditNums && AuditNums.Value > 1) townPersonLiable.AuditNums = AuditNums.Value + 1;
                            var log = new operateLog();
                            log.userName = RealName;
                            log.operateTime = DateTime.Now;
                            log.operateMsg = "导入名为：" + townPersonLiable.Name + "，岗位为：" + townPersonLiable.Position + "的镇级防汛防台责任人信息";
                            var logList = new List<operateLog>();
                            logList.Add(log);
                            townPersonLiable.operateLog = JsonTools.ObjectToJson(logList);
                            if (db.Insert(townPersonLiable) == 1)
                                successCount++;
                            else
                            {
                                dic = new Dictionary<string, string>(); dic.Add(rowNo.ToString(), msg);
                                dicList.Add(dic);
                                failCount++;
                            }
                            #endregion
                            
                            name = "镇级防汛防台责任人";
                        }
                        else if (request.typeId == 5)
                        {
                            #region 行政村信息

                            var adcdInfo = new ADCDInfo();
                            var tempAdcd = ADCDInfoList.Count > 0 ? ADCDInfoList[0].adcd : adcd;
                            adcdInfo.adcd = long.Parse(tempAdcd.Substring(0, 12)) + rowNo + "000";
                            adcdInfo.adnm = ConverterHelper.ObjectToString(dr[0].ToString().Trim());
                            if (!string.IsNullOrEmpty(dr[1].ToString().Trim())&&ValidatorHelper.IsDouble(dr[1].ToString().Trim()))
                                adcdInfo.lng = double.Parse(dr[1].ToString().Trim());
                            if (!string.IsNullOrEmpty(dr[2].ToString().Trim()) && ValidatorHelper.IsDouble(dr[2].ToString().Trim()))
                                adcdInfo.lat = double.Parse(dr[2].ToString().Trim());
                            adcdInfo.CreateTime = DateTime.Now;
                            
                            var log = new operateLog();
                            log.userName = RealName;
                            log.operateTime = DateTime.Now;
                            log.operateMsg = "导入名为：" + adcdInfo.adnm + "的行政村信息";
                            var logList = new List<operateLog>();
                            logList.Add(log);
                            adcdInfo.operateLog = JsonTools.ObjectToJson(logList);
                            try
                            {
                                adcdInfo.parentId = db.Single<ADCDInfo>(w => w.adcd == adcd).Id;
                                adcdInfo.grade = 4;
                            }
                            catch (Exception ex) { }
                            if (db.Insert(adcdInfo) == 1)
                            {
                                successCount++;
                                var adcdDisasterInfo = new ADCDDisasterInfo();
                                adcdDisasterInfo.adcd = adcdInfo.adcd;
                                adcdDisasterInfo.Year = DateTime.Now.Year;
                                adcdDisasterInfo.CreateTime = DateTime.Now;
                                db.Insert(adcdDisasterInfo);
                            }
                            else
                            {
                                dic = new Dictionary<string, string>();
                                dic.Add(rowNo.ToString(), msg);
                                dicList.Add(dic);
                                failCount++;
                            }

                            #endregion
                           
                            name = "行政村信息";
                        }

                        #endregion
                    }
                    #region 日志
                    sb.Append("在栏目{组织责任/"+name+"}下,导入数据{" + dt.Rows.Count + "}条");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                    #endregion
                    info.SuccessNum = successCount;
                    info.failNum=failCount;
                    #endregion
                }
                catch (Exception ex)
                {
                    dic = new Dictionary<string, string>();
                    dic.Add("msg", "导入失败，异常信息如下："+ex.Message);
                    dicList.Add(dic);
                    
                }
                finally
                {
                    info.ErrorList = dicList;
                    File.Delete(newpath);
                }
                return info;
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public  UploadFileViewModel exportExcel(exportExcel request)
        {
            using (var db=DbFactory.Open())
            {
                var info=new UploadFileViewModel();
                var filesavepath = "Upload/Export";
                var name = DateTime.Now.ToString("yyyyMMddHHmmssfff");// DateTime.Now.Ticks.ToString();
                var fileSrc = "";
                var filePath = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath);
              
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath); //创建该文件夹
                var createxcel = new creatExcel();
                var dt=new DataTable();//转成Excel导出使用
                var arr=new ArrayList();//不需要导出的列 索引添加到ArrayList里  从0开始
             
                var dic =new Dictionary<bool,string>();//ture fasle需要转成的中文
                var dicList=new List<Dictionary<bool, string>> ();//ture fasle需要转成的中文
                var sheetName = "";//工作表名
                var arrs = new string[] {};
                var arrName = new string[] { };
                StringBuilder sb = new StringBuilder();
                switch (request.typeId)
                {
                    case 0:
                        #region 行政村受灾信息

                        sheetName = request.year+"年行政村受灾信息";
                        var list = ZZTXManager.GetADCDDisasterInfo(new GetADCDDisasterInfo() {year=request.year,PageSize = int.MaxValue});
                        if (list.rows.Count > 0)
                        {
                            arr.Add(0);
                            arr.Add(1);
                            arr.Add(6);
                            arr.Add(7);
                            arr.Add(8);
                            dt = Common.ListHelper.ListToDataTable(list.rows,arr);
                             
                             arrs = ConfigurationManager.AppSettings["行政村受灾信息"].Split('|');
                             arrName = arrs[0].Split(',');
                            if (dt.Columns.Contains("IfTransfer")) dt.Columns.Remove("IfTransfer");
                            if (dt.Columns.Contains("onperson")) dt.Columns.Remove("onperson");
                            for (var i=0;i< dt.Columns.Count;i++)
                            {
                                dt.Columns[i].Caption = arrName[i];
                            }
                           
                        }
                        #endregion
                        #region 日志
                        sb.Append("在栏目{组织责任/行政村受灾信息}下,导出数据{" + dt.Rows.Count + "}条");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.导出);
                        #endregion
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        #region 行政村危险区人员转移清单
                        var getVillageTransferPerson=new GetVillageTransferPerson();
                        if (!string.IsNullOrEmpty(request.adcd))
                            getVillageTransferPerson.adcd = request.adcd;
                        getVillageTransferPerson.year = request.year;
                        getVillageTransferPerson.PageSize = int.MaxValue;
                        sheetName = request.year + "年行政村危险区人员转移清单";
                        var villageTransferPersonlist = VillageTransferPersonManager.GetVillageTransferPerson(getVillageTransferPerson);
                        if (villageTransferPersonlist.rows.Count > 0)
                        {
                            arr.Clear();
                            arr.Add(0);
                            arr.Add(1);
                            arr.Add(21);
                            arr.Add(23);
                            arr.Add(24);
                            dic.Add(true,"有");
                            dic.Add(false, "无");
                            dicList.Add(dic);
                            dt = Common.ListHelper.ListToDataTable(villageTransferPersonlist.rows, arr, dicList);
                        }
                         arrs = ConfigurationManager.AppSettings["行政村危险区人员转移清单"].Split('|');
                         arrName = arrs[0].Split(',');
                        if(dt.Columns.Contains("Did")) dt.Columns.Remove("Did");
                        if (dt.Columns.Contains("IfTransfer")) dt.Columns.Remove("IfTransfer");
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].Caption = arrName[i];
                        }

                        #endregion
                        #region 日志
                        sb.Append("在栏目{组织责任/行政村危险区人员转移清单}下,导出数据{" + dt.Rows.Count + "}条");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.导出);
                        #endregion
                        break;
                    case 4:
                        #region 镇级防汛防台责任人

                        sheetName = request.year + "年镇级防汛防台责任人";
                        var adnm = ZZTXManager.GetADCDInfoByADCD(adcd).adnm;
                        var townListlist = TownManager.GetTownList(new GetTownList() { year = request.year,PageSize = int.MaxValue});
                        if (townListlist.rows.Count > 0)
                        {
                            arr.Clear();
                            arr.Add(0);
                            arr.Add(1);
                            arr.Add(8);
                            arr.Add(9);
                            arr.Add(10);
                            dt = Common.ListHelper.ListToDataTable(townListlist.rows, arr);
                            
                             arrs = ConfigurationManager.AppSettings["镇级防汛防台责任人"].Split('|');
                             arrName = arrs[0].Split(',');
                           if(dt.Columns.Contains("operateLog")) dt.Columns.Remove("operateLog");
                            if (dt.Columns.Contains("adnmparent")) dt.Columns.Remove("adnmparent");
                            if (dt.Columns.Contains("checkresult")) dt.Columns.Remove("checkresult");
                            for (var i = 0; i < dt.Columns.Count; i++)
                            {
                                dt.Columns[i].Caption = arrName[i];
                            }
                        }

                        #endregion
                        #region 日志
                        sb.Append("在栏目{组织责任/镇级防汛防台责任人}下,导出数据{" + dt.Rows.Count + "}条");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.导出);
                        #endregion
                        break;
                    case 5:
                        #region 行政村信息

                        sheetName = "行政村信息";
                       
                        var adcdListlist = ZZTXManager.GetADCDInfo(new GetADCDInfo (){ levle=5, PageSize = int.MaxValue });
                        if (adcdListlist.rows.Count > 0)
                        {
                            arr.Clear();
                            arr.Add(0);
                            arr.Add(1);
                            arr.Add(5);
                            arr.Add(6);
                            dt = Common.ListHelper.ListToDataTable(adcdListlist.rows, arr);

                            arrs = ConfigurationManager.AppSettings["行政村信息"].Split('|');
                            arrName = arrs[0].Split(',');
                            if (dt.Columns.Contains("grade")) dt.Columns.Remove("grade");
                            if (dt.Columns.Contains("parentid")) dt.Columns.Remove("parentid");
                            if (dt.Columns.Contains("operateLog")) dt.Columns.Remove("operateLog");
                            for (var i = 0; i < dt.Columns.Count; i++)
                            {
                                dt.Columns[i].Caption = arrName[i];
                            }
                        }
                        #endregion
                        #region 日志
                        sb.Append("在栏目{组织责任/行政村信息}下,导出数据{" + adcdListlist.rows.Count + "}条");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.导出);
                        #endregion
                        break;
                }
                var path = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath + "/" + sheetName +"-"+ name + ".xls");
                fileSrc = filesavepath + "/" + sheetName + "-"+ name + ".xls";
                byte[] data = createxcel.DataTableToExcel(dt, sheetName, request.mergeCellNum, request.mergeCellContent);
              
                if (!File.Exists(path))
                {
                    FileStream fs = new FileStream(path, FileMode.CreateNew);
                    fs.Write(data, 0, data.Length);
                    fs.Close();
                    info.fileSrc = fileSrc;
                }
                return info;
            }
        }
        
    }
}
