using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model.SumAppUser;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Logic.Common;
using GrassrootsFloodCtrl.Model;
using System.Data;
using GrassrootsFloodCtrl.Model.ZZTX;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace GrassrootsFloodCtrl.Logic.SumAppUser
{
    public class SunAppUserLogic : ManagerBase, ISunAppUserLogic
    {
        ConcurrentBag<SelectSumAppUserList> concurrentList = new ConcurrentBag<SelectSumAppUserList>();
        /// <summary>
        /// 获取注册情况统计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<SelectSumAppUserList> GetSelectSumAppUserList(RouteSumAppUser request)
        {
            using (var db = DbFactory.Open())
            {
                string userSql = "select COUNT(*) as userCount,a.adcd,a.adcdId,b.adnm,b.parentId from AppAllUserView a left join ADCDInfo b on a.adcdId=b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000' ";
                string appSql = "select COUNT(*) as appcount,b.adcd,a.adcdId,b.adnm,b.parentId from AppGetReg a left join ADCDInfo b on  a.adcdId=b.Id where a.adcdId is not null  and RIGHT(b.adcd, 11)!= '00000000000' ";
                if (AdcdHelper.GetByAdcdRole(request.adcd) == "省级")
                {
                    return GetProvinceList(request.adcd, request.adcdName, db);
                    //userSql += "  and a.adcd like '33%'";
                    //appSql += "  and b.adcd like '33%'";
                }
                if (AdcdHelper.GetByAdcdRole(request.adcd) == "市级")
                {
                    return GetCityList(request.adcd,request.adcdName,db);

                }
                if (AdcdHelper.GetByAdcdRole(request.adcd) == "县级")
                {
                    return GetCountryList(request.adcd, request.adcdName, db);
                    //userSql += "  and a.adcd like '" + request.adcd.Substring(0, 6) + "%'";
                    //appSql += "  and b.adcd like '" + request.adcd.Substring(0, 6) + "%'";
                }
                if (AdcdHelper.GetByAdcdRole(request.adcd) == "镇级")
                {
                    return GetTownList(request.adcd, request.adcdName, db);
                    //userSql += "  and a.adcd like '" + request.adcd.Substring(0, 9) + "%'";
                    //appSql += "  and b.adcd like '" + request.adcd.Substring(0, 9) + "%'";
                }
                if (request.adcdName != null && request.adcdName != "")
                {
                    userSql += "  and b.adnm like '%"+request.adcdName+"%'";
                    appSql += "  and b.adnm like  '%" + request.adcdName + "%'";
                }
                userSql += "  group by a.adcd,a.adcdId,b.adnm,b.parentId order by a.adcdId";
                appSql += "  group by b.adcd,a.adcdId,b.adnm,b.parentId order by a.adcdId";
                List <SelectSumAppUserList> allList = db.SqlList<SelectSumAppUserList>(userSql);
                List<SelectSumAppUserList> appList = db.SqlList<SelectSumAppUserList>(appSql);
                return GetSumAppUserList(allList,appList,request.adcd);
            }
        }

        public List<SelectSumAppUserList> GetSelectSumAppUserList2(RouteSumAppUser2 request)
        {
                if (AdcdHelper.GetByAdcdRole(request.adcd) == "省级")
                {
                    return GetProvinceList2(request.adcd, request.adcdName);
                    //userSql += "  and a.adcd like '33%'";
                    //appSql += "  and b.adcd like '33%'";
                }
                return null;
        }

        private List<SelectSumAppUserList> GetProvinceList2(string adcd, string adcdName)
        {
            List<SelectSumAppUserList> list = new List<SelectSumAppUserList>();
            List<SelectSumAppUserList> adcdList = new List<SelectSumAppUserList>();
            List<SelectSumAppUserList> uList = new List<SelectSumAppUserList>();
            using (var db = DbFactory.Open())
            {
                //县级按照市级用户分类统计
                string sql = "select COUNT(*) as userCount,left(a.adcd, 4) as adcd from AppAllUserView a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000' and a.adcd like  '" + adcd.Substring(0, 2) + "%' ";
                sql += "group by left(a.adcd, 4) order by left(a.adcd, 4)";
                string sqlAdcdInfo = " select adcd,adnm,id as adcdId,0 as parentId from ADCDInfo where parentId in( select id from ADCDInfo where adcd='" + adcd + "') order by adcd ";
                string userSql = "select COUNT(*) as appcount,left(b.adcd, 4) as adcd from AppGetReg a  left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and RIGHT(b.adcd, 11)!= '00000000000' and b.adcd like  '" + adcd.Substring(0, 2) + "%' group by left(b.adcd, 4) order by left(b.adcd, 4)";
                list = db.SqlList<SelectSumAppUserList>(sql);
                adcdList = db.SqlList<SelectSumAppUserList>(sqlAdcdInfo);
                uList = db.SqlList<SelectSumAppUserList>(userSql);
            }
            //所有县级的list
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db1 = DbFactory.Open())
            {
                Task task1 = new Task(() => { InsertConcurrentList(0, 6, list, uList, adcdList); });
                Task task2 = new Task(() => { InsertConcurrentList(6, list.Count, list, uList, adcdList); });
                task1.Start();
                task2.Start();
                Task.WaitAll(task1, task2);
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            //将所有的县级加入市级列表
            list.AddRange(concurrentList);
            return list;
        }
        /// <summary>
        /// 省级统计情况
        /// </summary>
        /// <param name="adcd"></param>
        /// <param name="adcdName"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private List<SelectSumAppUserList> GetProvinceList(string adcd, string adcdName, IDbConnection db)
        {
            //县级按照市级用户分类统计
            string sql = "select COUNT(*) as userCount,left(a.adcd, 4) as adcd from AppAllUserView a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000' and a.adcd like  '" + adcd.Substring(0, 2) + "%' ";
            sql += "group by left(a.adcd, 4) order by left(a.adcd, 4)";
            string sqlAdcdInfo = " select adcd,adnm,id as adcdId,0 as parentId from ADCDInfo where parentId in( select id from ADCDInfo where adcd='" + adcd + "') order by adcd ";
            string userSql = "select COUNT(*) as appcount,left(b.adcd, 4) as adcd from AppGetReg a  left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and RIGHT(b.adcd, 11)!= '00000000000' and b.adcd like  '" + adcd.Substring(0, 2) + "%' group by left(b.adcd, 4) order by left(b.adcd, 4)";
            List<SelectSumAppUserList> list = db.SqlList<SelectSumAppUserList>(sql);
            List<SelectSumAppUserList> adcdList = db.SqlList<SelectSumAppUserList>(sqlAdcdInfo);
            List<SelectSumAppUserList> uList = db.SqlList<SelectSumAppUserList>(userSql);
            //所有县级的list
            //计划按照并行操作每个市级开线程访问
            List<SelectSumAppUserList> countryAllList = new List<SelectSumAppUserList>();
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            for (int i = 0; i < list.Count; i++)
            {
                //注册人员的统计放入list
                var userItem = uList.Find(x => x.adcd == list[i].adcd);
                if (userItem != null)
                {
                    list[i].appcount = userItem.appcount;
                }
                else
                {
                    list[i].appcount = 0;
                }
                //查找市级按照县级分类的统计放入countryList
                string countrySql = "select COUNT(*) as userCount,left(a.adcd, 6) as adcd from AppAllUserView a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000'and a.adcd like  '" + list[i].adcd + "%' group by left(a.adcd, 6) order by left(a.adcd, 6)";
                string countryAdcdSql = " select adcd,adnm,id as adcdId, parentId from ADCDInfo where parentId in( select id from ADCDInfo where adcd='" + adcdList[i].adcd + "') order by adcd ";
                string countryUList = "select COUNT(*) as appcount,left(b.adcd, 6) as adcd from AppGetReg a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and RIGHT(b.adcd, 11)!= '00000000000' and b.adcd like  '" + list[i].adcd + "%' group by left(b.adcd, 6) order by left(b.adcd, 6)";
                List<SelectSumAppUserList> countrylist = db.SqlList<SelectSumAppUserList>(countrySql);
                List<SelectSumAppUserList> adcdCountryList = db.SqlList<SelectSumAppUserList>(countryAdcdSql);
                List<SelectSumAppUserList> uCountryList = db.SqlList<SelectSumAppUserList>(countryUList);
                for (int j = 0; j < countrylist.Count; j++)
                {
                    var countryItem = uCountryList.Find(x => x.adcd == countrylist[j].adcd);
                    if (countryItem != null)
                    {
                        countrylist[j].appcount = countryItem.appcount;
                    }
                    else
                    {
                        countrylist[j].appcount = 0;
                    }
                    countrylist[j].adcd = adcdCountryList[j].adcd;
                    countrylist[j].adnm = adcdCountryList[j].adnm;
                    countrylist[j].adcdId = adcdCountryList[j].adcdId;
                    countrylist[j].parentId = i + 1;
                }
                countryAllList.AddRange(countrylist);
                list[i].adcd = adcdList[i].adcd;
                list[i].adcdId = i + 1;
                list[i].adnm = adcdList[i].adnm;
                list[i].parentId = 0;
            }
            sw2.Stop();
            TimeSpan ts2 = sw2.Elapsed;
            //将所有的县级加入市级列表
            list.AddRange(countryAllList);
            return list;
        }
        public void InsertConcurrentList(int start,int end, List<SelectSumAppUserList> list, List<SelectSumAppUserList> uList, List<SelectSumAppUserList> adcdList, IDbConnection db)
        {
            //注册人员的统计放入list
            for (int i = start; i < end; i++)
            {
                //注册人员的统计放入list
                var userItem = uList.Find(x => x.adcd == list[i].adcd);
                if (userItem != null)
                {
                    list[i].appcount = userItem.appcount;
                }
                else
                {
                    list[i].appcount = 0;
                }
                //查找市级按照县级分类的统计放入countryList
                string countrySql = "select COUNT(*) as userCount,left(a.adcd, 6) as adcd from AppAllUserView a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000'and a.adcd like  '" + list[i].adcd + "%' group by left(a.adcd, 6) order by left(a.adcd, 6)";
                string countryAdcdSql = " select adcd,adnm,id as adcdId, parentId from ADCDInfo where parentId in( select id from ADCDInfo where adcd='" + adcdList[i].adcd + "') order by id ";
                string countryUList = "select COUNT(*) as appcount,left(b.adcd, 6) as adcd from AppGetReg a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and RIGHT(b.adcd, 11)!= '00000000000' and b.adcd like  '" + list[i].adcd + "%' group by left(b.adcd, 6) order by left(b.adcd, 6)";
                List<SelectSumAppUserList> countrylist = db.SqlList<SelectSumAppUserList>(countrySql);
                List<SelectSumAppUserList> adcdCountryList = db.SqlList<SelectSumAppUserList>(countryAdcdSql);
                List<SelectSumAppUserList> uCountryList = db.SqlList<SelectSumAppUserList>(countryUList);
                for (int j = 0; j < countrylist.Count; j++)
                {
                    var countryItem = uCountryList.Find(x => x.adcd == countrylist[j].adcd);
                    if (countryItem != null)
                    {
                        countrylist[j].appcount = countryItem.appcount;
                    }
                    else
                    {
                        countrylist[j].appcount = 0;
                    }
                    countrylist[j].adcd = adcdCountryList[j].adcd;
                    countrylist[j].adnm = adcdCountryList[j].adnm;
                    countrylist[j].adcdId = adcdCountryList[j].adcdId;
                    countrylist[j].parentId = i + 1;
                    concurrentList.Add(countrylist[j]);
                }
                list[i].adcd = adcdList[i].adcd;
                list[i].adcdId = i + 1;
                list[i].adnm = adcdList[i].adnm;
                list[i].parentId = 0;
            }
        }
        public void InsertConcurrentList(int start, int end, List<SelectSumAppUserList> list, List<SelectSumAppUserList> uList, List<SelectSumAppUserList> adcdList)
        {
            using (var db = DbFactory.Open())
            {
                //注册人员的统计放入list
                for (int i = start; i < end; i++)
                {
                    //注册人员的统计放入list
                    var userItem = uList.Find(x => x.adcd == list[i].adcd);
                    if (userItem != null)
                    {
                        list[i].appcount = userItem.appcount;
                    }
                    else
                    {
                        list[i].appcount = 0;
                    }
                    //查找市级按照县级分类的统计放入countryList
                    string countrySql = "select COUNT(*) as userCount,left(a.adcd, 6) as adcd from AppAllUserView a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000'and a.adcd like  '" + list[i].adcd + "%' group by left(a.adcd, 6) order by left(a.adcd, 6)";
                    string countryAdcdSql = " select adcd,adnm,id as adcdId, parentId from ADCDInfo where parentId in( select id from ADCDInfo where adcd='" + adcdList[i].adcd + "') order by adcd ";
                    string countryUList = "select COUNT(*) as appcount,left(b.adcd, 6) as adcd from AppGetReg a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and RIGHT(b.adcd, 11)!= '00000000000' and b.adcd like  '" + list[i].adcd + "%' group by left(b.adcd, 6) order by left(b.adcd, 6)";
                    List<SelectSumAppUserList> countrylist = db.SqlList<SelectSumAppUserList>(countrySql);
                    List<SelectSumAppUserList> adcdCountryList = db.SqlList<SelectSumAppUserList>(countryAdcdSql);
                    List<SelectSumAppUserList> uCountryList = db.SqlList<SelectSumAppUserList>(countryUList);
                    for (int j = 0; j < countrylist.Count; j++)
                    {
                        var countryItem = uCountryList.Find(x => x.adcd == countrylist[j].adcd);
                        if (countryItem != null)
                        {
                            countrylist[j].appcount = countryItem.appcount;
                        }
                        else
                        {
                            countrylist[j].appcount = 0;
                        }
                        countrylist[j].adcd = adcdCountryList[j].adcd;
                        countrylist[j].adnm = adcdCountryList[j].adnm;
                        countrylist[j].adcdId = adcdCountryList[j].adcdId;
                        countrylist[j].parentId = i + 1;
                        concurrentList.Add(countrylist[j]);
                    }
                    list[i].adcd = adcdList[i].adcd;
                    list[i].adcdId = i + 1;
                    list[i].adnm = adcdList[i].adnm;
                    list[i].parentId = 0;
                }
            }

        }
        private List<SelectSumAppUserList> GetTownList(string adcd, string adcdName, IDbConnection db)
        {
            string sql = "select COUNT(*) as userCount,left(a.adcd, 12) as adcd from AppAllUserView a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000' and a.adcd like  '" + adcd.Substring(0, 9) + "%' ";
            sql += "group by left(a.adcd, 12) order by left(a.adcd, 12)";
            string sqlAdcdInfo = " select adcd,adnm,id as adcdId,0 as parentId from ADCDInfo where parentId in( select id from ADCDInfo where adcd='" + adcd + "') order by adcd ";
            string userSql = "select COUNT(*) as appcount,left(b.adcd, 12) as adcd from AppGetReg a  left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and RIGHT(b.adcd, 11)!= '00000000000' and b.adcd like  '" + adcd.Substring(0, 9) + "%' group by left(b.adcd, 12) order by left(b.adcd, 12)";
            List<SelectSumAppUserList> list = db.SqlList<SelectSumAppUserList>(sql);
            List<SelectSumAppUserList> adcdList = db.SqlList<SelectSumAppUserList>(sqlAdcdInfo);
            List<SelectSumAppUserList> uList = db.SqlList<SelectSumAppUserList>(userSql);
            for (int i = 0; i < list.Count; i++)
            {
                var userItem = uList.Find(x => x.adcd == list[i].adcd);
                if (userItem != null)
                {
                    list[i].appcount = userItem.appcount;
                }
                if (i == 0)
                {
                    list[i].adnm = "镇本级";
                    list[i].adcd = adcd;
                    list[i].adcdId = list.Count + 1;
                    list[i].parentId = 0;
                }
                else
                {
                    list[i].adcd = adcdList[i-1].adcd;
                    list[i].adcdId = i + 1;
                    list[i].adnm = adcdList[i-1].adnm;
                    list[i].parentId = 0;
                }

            }
            return list;
        }
        private List<SelectSumAppUserList> GetCountryList(string adcd, string adcdName, IDbConnection db)
        {
            string sql = "select COUNT(*) as userCount,left(a.adcd, 9) as adcd from AppAllUserView a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000' and a.adcd like  '" + adcd.Substring(0, 6) + "%' ";
            sql += "group by left(a.adcd, 9) order by left(a.adcd, 9)";
            string sqlAdcdInfo = " select adcd,adnm,id as adcdId,0 as parentId from ADCDInfo where parentId in( select id from ADCDInfo where adcd='" + adcd + "') order by adcd ";
            string userSql = "select COUNT(*) as appcount,left(b.adcd, 9) as adcd from AppGetReg a  left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and RIGHT(b.adcd, 11)!= '00000000000' and b.adcd like  '" + adcd.Substring(0, 6) + "%' group by left(b.adcd, 9) order by left(b.adcd, 9)";
            List<SelectSumAppUserList> list = db.SqlList<SelectSumAppUserList>(sql);
            List<SelectSumAppUserList> adcdList = db.SqlList<SelectSumAppUserList>(sqlAdcdInfo);
            List<SelectSumAppUserList> uList = db.SqlList<SelectSumAppUserList>(userSql);
            //所有县级的list
            List<SelectSumAppUserList> countryAllList = new List<SelectSumAppUserList>();
            for (int i = 1; i < list.Count; i++)
            {
                //注册人员的统计放入list
                var userItem = uList.Find(x => x.adcd == list[i].adcd);
                if (userItem != null)
                {
                    list[i].appcount = userItem.appcount;
                }
                else
                {
                    list[i].appcount = 0;
                }
                //查找每个县的统计放入countryList
                string countrySql = "select COUNT(*) as userCount,left(a.adcd, 12) as adcd from AppAllUserView a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000'and a.adcd like  '" + list[i].adcd + "%' group by left(a.adcd, 12) order by left(a.adcd, 12)";
                string countryAdcdSql = " select adcd,adnm,id as adcdId, parentId from ADCDInfo where parentId in( select id from ADCDInfo where adcd='" + adcdList[i-1].adcd + "') order by adcd ";
                string countryUList = "select COUNT(*) as appcount,left(b.adcd, 12) as adcd from AppGetReg a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and RIGHT(b.adcd, 11)!= '00000000000' and b.adcd like  '" + list[i].adcd + "%' group by left(b.adcd, 12) order by left(b.adcd, 12)";
                List<SelectSumAppUserList> countrylist = db.SqlList<SelectSumAppUserList>(countrySql);
                List<SelectSumAppUserList> adcdCountryList = db.SqlList<SelectSumAppUserList>(countryAdcdSql);
                List<SelectSumAppUserList> uCountryList = db.SqlList<SelectSumAppUserList>(countryUList);
                for (int j = 0; j < countrylist.Count; j++)
                {
                    var countryItem = uCountryList.Find(x => x.adcd == countrylist[j].adcd);
                    if (countryItem != null)
                    {
                        countrylist[j].appcount = countryItem.appcount;
                    }
                    else
                    {
                        countrylist[j].appcount = 0;
                    }
                    if (j != 0)
                    {
                        countrylist[j].adcd = adcdCountryList[j - 1].adcd;
                        countrylist[j].adnm = adcdCountryList[j - 1].adnm;
                        countrylist[j].adcdId = adcdCountryList[j - 1].adcdId;
                        countrylist[j].parentId = i + 1;
                    }
                    else
                    {
                        countrylist[j].adcd = countrylist[j].adcd + "000";
                        countrylist[j].adnm = "镇本级";
                        List<ADCDInfo> parentList = db.SqlList<ADCDInfo>(" select id,parentId from ADCDInfo where adcd='" + countrylist[j].adcd + "'");
                        if (parentList != null)
                        {
                            countrylist[j].adcdId = parentList[0].Id;
                            countrylist[j].parentId = i + 1;
                        }
                    }
                }
                countryAllList.AddRange(countrylist);
                list[i].adcd = adcdList[i-1].adcd;
                list[i].adcdId = i + 1;
                list[i].adnm = adcdList[i-1].adnm;
                list[i].parentId = 0;
            }
            if (list.Count > 0)
            {

                list[0].adnm = "县本级";
                list[0].adcd = adcd;
                list[0].adcdId = list.Count + 1;
                if (uList.Count > 0)
                {
                    var userItem = uList.Find(x => x.adcd == adcd.Substring(0,9));
                    if (userItem != null)
                    {
                        list[0].appcount = userItem.appcount;
                    }
                }
                list[0].parentId = 0;
            }
            //list.Remove(list[0]);
            //将所有的县级加入市级列表
            list.AddRange(countryAllList);
            return list;
        }
        private List<SelectSumAppUserList> GetCityList(string adcd,string adcdName, IDbConnection db)
        {
            string sql = "select COUNT(*) as userCount,left(a.adcd, 6) as adcd from AppAllUserView a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000' and a.adcd like  '"+adcd.Substring(0,4)+"%' ";
            sql += "group by left(a.adcd, 6) order by left(a.adcd, 6)";
            string sqlAdcdInfo = " select adcd,adnm,id as adcdId,0 as parentId from ADCDInfo where parentId in( select id from ADCDInfo where adcd='" + adcd+ "') order by adcd ";
            string userSql = "select COUNT(*) as appcount,left(b.adcd, 6) as adcd from AppGetReg a  left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and RIGHT(b.adcd, 11)!= '00000000000' and b.adcd like  '" + adcd.Substring(0, 4) + "%' group by left(b.adcd, 6) order by left(b.adcd, 6)";
            List<SelectSumAppUserList> list = db.SqlList<SelectSumAppUserList>(sql);         
            List<SelectSumAppUserList> adcdList = db.SqlList<SelectSumAppUserList>(sqlAdcdInfo);
            List<SelectSumAppUserList> uList= db.SqlList<SelectSumAppUserList>(userSql);
            //所有县级的list
            List<SelectSumAppUserList> countryAllList = new List<SelectSumAppUserList>();
            for(int i=0;i<list.Count;i++)
            {
                //注册人员的统计放入list
                var userItem = uList.Find(x => x.adcd == list[i].adcd);
                if (userItem != null)
                {
                    list[i].appcount = userItem.appcount;
                }
                else
                {
                    list[i].appcount = 0;
                }
                //查找每个县的统计放入countryList
                string countrySql = "select COUNT(*) as userCount,left(a.adcd, 9) as adcd from AppAllUserView a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000'and a.adcd like  '"+list[i].adcd+"%' group by left(a.adcd, 9) order by left(a.adcd, 9)";
                string countryAdcdSql = " select adcd,adnm,id as adcdId, parentId from ADCDInfo where parentId in( select id from ADCDInfo where adcd='"+ adcdList[i].adcd + "') order by adcd ";
                string countryUList = "select COUNT(*) as appcount,left(b.adcd, 9) as adcd from AppGetReg a left join ADCDInfo b on a.adcdId = b.Id where a.adcdId is not null and RIGHT(b.adcd, 11)!= '00000000000' and b.adcd like  '"+list[i].adcd+"%' group by left(b.adcd, 9) order by left(b.adcd, 9)";
                List<SelectSumAppUserList> countrylist = db.SqlList<SelectSumAppUserList>(countrySql);
                List<SelectSumAppUserList> adcdCountryList = db.SqlList<SelectSumAppUserList>(countryAdcdSql);
                List<SelectSumAppUserList> uCountryList = db.SqlList<SelectSumAppUserList>(countryUList);
                for (int j = 0; j < countrylist.Count; j++)
                {
                    var countryItem = uCountryList.Find(x => x.adcd == countrylist[j].adcd);
                    if (countryItem != null)
                    {
                        countrylist[j].appcount = countryItem.appcount;
                    }
                    else
                    {
                        countrylist[j].appcount = 0;
                    }
                    if (j != 0)
                    {
                        countrylist[j].adcd = adcdCountryList[j-1].adcd;
                        countrylist[j].adnm = adcdCountryList[j-1].adnm;
                        countrylist[j].adcdId = adcdCountryList[j-1].adcdId;
                        countrylist[j].parentId = i + 1;
                    }
                    else
                    {
                        countrylist[j].adcd = countrylist[j].adcd + "000000";
                        countrylist[j].adnm = "县本级";
                        List<ADCDInfo> parentList = db.SqlList<ADCDInfo>(" select id,parentId from ADCDInfo where adcd='" + countrylist[j].adcd + "'");
                        if (parentList != null)
                        {
                            countrylist[j].adcdId = parentList[0].Id;
                            countrylist[j].parentId = i + 1;
                        }
                    }
                }
                countryAllList.AddRange(countrylist);
                list[i].adcd = adcdList[i].adcd;
                list[i].adcdId = i+1;
                list[i].adnm = adcdList[i].adnm;
                list[i].parentId = 0;
            }
            //将所有的县级加入市级列表
            list.AddRange(countryAllList);
            return list;
        }
        /// <summary>
        /// 统计为注册人的信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<Model.SumAppUser.SumAppUser> GetSumAppUserList(RouteNoAppUser request)
        {
            using (var db = DbFactory.Open())
            {
                string userSql = "select a.adcd,a.adcdId,b.adnm,b.parentId,a.phone,a.UserName from AppAllUserView a left join ADCDInfo b on a.adcdId=b.Id  where a.adcdId is not null and a.adcd is not null and RIGHT(a.adcd, 11)!= '00000000000' ";
                string appSql = "select b.adcd,a.adcdId,b.adnm,b.parentId,a.Mobile as phone,a.UserName from AppGetReg a left join ADCDInfo b on  a.adcdId=b.Id where a.adcdId is not null  and RIGHT(b.adcd, 11)!= '00000000000'  ";
                if (AdcdHelper.GetByAdcdRole(request.adcd) == "省级")
                {
                    userSql += "  and a.adcd like '33%'";
                    appSql += "  and b.adcd like '33%'";
                }
                if (AdcdHelper.GetByAdcdRole(request.adcd) == "市级")
                {
                    userSql += "  and a.adcd like '" + request.adcd.Substring(0, 4) + "%'";
                    appSql += "  and b.adcd like '" + request.adcd.Substring(0, 4) + "%'";
                }
                if (AdcdHelper.GetByAdcdRole(request.adcd) == "县级")
                {
                    userSql += "  and a.adcd like '" + request.adcd.Substring(0, 6) + "%'";
                    appSql += "  and b.adcd like '" + request.adcd.Substring(0, 6) + "%'";
                }
                if (AdcdHelper.GetByAdcdRole(request.adcd) == "镇级")
                {
                    userSql += "  and a.adcd like '" + request.adcd.Substring(0, 9) + "%'";
                    appSql += "  and b.adcd like '" + request.adcd.Substring(0, 9) + "%'";
                }
                if (AdcdHelper.GetByAdcdRole(request.adcd) == "村级")
                {
                    userSql += "  and a.adcd like '" + request.adcd + "%'";
                    appSql += "  and b.adcd like '" + request.adcd + "%'";
                }
                userSql += "  group by a.adcd,a.adcdId,b.adnm,b.parentId,a.phone,a.UserName order by a.adcdId";
                appSql += "  group by b.adcd,a.adcdId,b.adnm,b.parentId,a.Mobile,a.UserName order by a.adcdId";
                List<Model.SumAppUser.SumAppUser> allList = 
                    db.SqlList<Model.SumAppUser.SumAppUser>(userSql);
                List<Model.SumAppUser.SumAppUser> appList = 
                    db.SqlList<Model.SumAppUser.SumAppUser>(appSql);
                var list = GetUserList(allList, appList);
                var pageList = list.Skip(request.PageSize * (request.PageIndex - 1))
                   .Take(request.PageSize).ToList();
                return new BsTableDataSource<Model.SumAppUser.SumAppUser> { total = list.Count(), rows = pageList };
            }
        }

        private List<Model.SumAppUser.SumAppUser> GetUserList(List<Model.SumAppUser.SumAppUser> allList, List<Model.SumAppUser.SumAppUser> appList)
        {
            foreach (var item in appList)
            {
                int a=allList.RemoveAll(x=>x.phone==item.phone&&x.UserName==item.UserName&&x.adcd==item.adcd);
            }
            return allList;
        }
        /// <summary>
        /// 统计用户增加已录入的信息
        /// </summary>
        /// <param name="allList"></param>
        /// <param name="appList"></param>
        /// <param name="adcd"></param>
        /// <returns></returns>
        private List<SelectSumAppUserList> GetSumAppUserList(List<SelectSumAppUserList> allList, List<SelectSumAppUserList> appList,string adcd)
        {
            foreach(var item in appList)
            {
                var userItem=allList.Find(x => x.adcd == item.adcd);
                userItem.appcount = item.appcount;
            }
            if (AdcdHelper.GetByAdcdRole(adcd) == "县级"&&allList.Count>0)
            {
                allList[0].parentId = 0;
            }
            if (AdcdHelper.GetByAdcdRole(adcd) == "镇级" && allList.Count > 0)
            {
                allList[0].parentId = 0;
            }
            return allList;
        }
    }
}
