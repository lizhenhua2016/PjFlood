namespace GrassrootsFloodCtrl.ServiceModel.Common
{
    /// <summary>
    /// 上传文件，返回模型
    /// </summary>
    public class UploadFileViewModel
    {
        /// <summary>
        /// 是否上传成功
        /// </summary>
        public bool isSuccess { get; set; }

        /// <summary>
        /// 上传结果反馈
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string flieSize { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string fileType { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string fileSrc { get; set; }

        /// <summary>
        /// 真实文件名
        /// </summary>
        public string realName { get; set; }

        
    }
}