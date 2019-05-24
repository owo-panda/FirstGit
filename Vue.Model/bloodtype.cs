using System;

namespace Vue.Model
{
    /// <summary>
    /// 血型
    /// </summary>
    [Serializable]
    public partial class bloodtype
    {
        public bloodtype()
        { }
        #region Model
        private int _id;
        private string _bloodtype = string.Empty;

        /// <summary>
        /// 自增ID
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BloodType
        {
            set { _bloodtype = value; }
            get { return _bloodtype; }
        }
        #endregion
    }
}