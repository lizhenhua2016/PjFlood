using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Post;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Post
{
    public interface IPostManager
    {
        BsTableDataSource<PostViewModel> GetPostList(GetPostList request);
    }
}
