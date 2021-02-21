namespace HCMDAL
{
    public class CommonEntityDAL
    {
        public string search { get; set; }
        public string order { get; set; }
        public string orderByColumnName { get; set; }
        public string orderDir { get; set; }
        public int startRec { get; set; }
        public int pageSize { get; set; }
        public int totalRecords { get; set; }
        public int recFilter { get; set; }
    }
}
