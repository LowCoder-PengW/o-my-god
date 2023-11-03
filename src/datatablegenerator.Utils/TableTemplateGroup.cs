
namespace datatablegenerator.Common
{
    public class TableTemplateGroup
    {
        static readonly List<string> groups;
        static TableTemplateGroup()
        {
            groups = new List<string>();
            groups.Add("字段名称");
            groups.Add("数据类型");
            groups.Add("必填");
            groups.Add("默认值");
            groups.Add("约束");
            groups.Add("字段说明");

        }


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Exist(string value)
        {
            return groups.Contains(value);
        }


    }
}
