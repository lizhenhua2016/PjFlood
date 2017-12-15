using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Org;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.Org;
using GrassrootsFloodCtrl.ServiceModel.Common;
using System.Collections;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;

namespace GrassrootsFloodCtrl.Logic.Org
{
    public class ColumnManager : ManagerBase,IColumnManager
    {
        /// <summary>
        /// 列表获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<ColumnManageViewModel> GetColumnList(GetColumnList request)
        {
            if (RowID != 1) throw new Exception("权限异常");
            using (var db=DbFactory.Open())
            {
                var builder = db.From<Column>();
                if (null != request.id && request.id.Value > 0) {
                    builder.Where(w=>w.ColumnID == request.id);
                }
                var count = db.Select(builder).Count;
                
                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderBy(o => o.ColumnID);
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<ColumnManageViewModel>(builder);
                return new BsTableDataSource<ColumnManageViewModel>() { rows = RList, total = count };
            }
        }

        public BaseResult ColumnDel(ColumnDel request)
        {
            BaseResult br = new BaseResult();
            try
            {
                using (var db = DbFactory.Open())
                {
                    if (string.IsNullOrEmpty(request.id))
                        throw new Exception("参数异常！");
                    ArrayList arr = new ArrayList();
                    var ids = request.id.Split(',');
                    for (int i = 0; i < ids.Count(); i++)
                    {
                        var id = int.Parse(ids[i]);
                        arr.Add(id);
                    }
                    br.IsSuccess = db.Delete<Column>(w => Sql.In(w.ColumnID, arr)) > 0;
                }
            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.ErrorMsg = ex.Message;
                throw new Exception(ex.Message);
            }
            return br;
        }
        /// <summary>
        /// 新增，编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResult ColumnSave(ColumnSave request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<Column>();
                BaseResult br = new BaseResult();
                if (string.IsNullOrEmpty(request.cname)) { br.IsSuccess = false; br.ErrorMsg = "栏目名不能为空"; return br; }
                if (string.IsNullOrEmpty(request.actions)) { br.IsSuccess = false; br.ErrorMsg = "栏目权限不能为空"; return br; }
                if (string.IsNullOrEmpty(request.url)) { br.IsSuccess = false; br.ErrorMsg = "栏目地址不能为空"; return br; }
                Column cm = new Column()
                {
                    ColumnName = request.cname,
                    Actions = request.actions,
                    Icon = request.ico,
                    IsVisible = request.visible == 0 ? false : true,
                    Level = request.level,
                    LocalUrl = request.url,
                    ParentID = request.pid,
                    Sort = request.csort
                };
                if (null != request.cid && request.cid.Value > 0)
                {
                    br.IsSuccess = db.Update<Column>(cm, x => x.ColumnID == request.cid) == 1 ? true : false;
                }
                else
                {
                    br.IsSuccess = db.Insert(cm) == 1 ? true : false;
                }
                return br;
            }
        }
        /// <summary>
        /// 角色权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<RoleDetailViewModel> GetRoleDetaileList(GetRoleDetaileList request)
        {
            if (RowID != 1) throw new Exception("权限异常");
            using (var db = DbFactory.Open())
            {
                var builder = db.From<RoleDetail>();
                if (null != request.rid && request.rid.Value > 0)
                {
                    builder.Where(w => w.RoleID == request.rid);
                }
                var count = db.Select(builder).Count;

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderBy(o => o.RoleDetailID);
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<RoleDetailViewModel>(builder);
                return new BsTableDataSource<RoleDetailViewModel>() { rows = RList, total = count };
            }
        }
    }
}
