using System;

namespace Vue.Model
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public partial class users
    {
        public users()
        { }
        #region Model
        private int _id;
        private string _loginpwd = string.Empty;
        private int _friendshippolicyid = 0;
        private string _nickname = string.Empty;
        private int _faceid = 0;
        private string _sex = string.Empty;
        private int _age = 0;
        private string _name = string.Empty;
        private int _starid = 0; 
        private int _bloodtypeid = 0;

        /// <summary>
        /// 自增ID
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string LoginPwd
        {
            set { _loginpwd = value; }
            get { return _loginpwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int FriendshipPolicyId
        {
            set { _friendshippolicyid = value; }
            get { return _friendshippolicyid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string NickName
        {
            set { _nickname = value; }
            get { return _nickname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int FaceId
        {
            set { _faceid = value; }
            get { return _faceid; }
        }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age
        {
            set { _age = value; }
            get { return _age; }
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StarId
        {
            set { _starid = value; }
            get { return _starid; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BloodTypeId
        {
            set { _bloodtypeid = value; }
            get { return _bloodtypeid; }
        }
        #endregion
    }
}