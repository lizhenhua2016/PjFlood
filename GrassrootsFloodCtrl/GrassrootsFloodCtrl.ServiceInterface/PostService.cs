using GrassrootsFloodCtrl.Logic.Post;
using GrassrootsFloodCtrl.Logic.Village;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Post;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public class PostService:ServiceBase
    {
        public IPostManager PostManager { get; set; }

        public BsTableDataSource<PostViewModel> Get(GetPostList request)
        {
            return PostManager.GetPostList(request);
        }
    }
}
