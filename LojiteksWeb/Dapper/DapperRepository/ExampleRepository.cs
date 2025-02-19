using Dapper;
using LojiteksWeb.Dapper.DapperClass;

namespace LojiteksWeb.Dapper.DapperRepository
{
    public class ExampleRepository : DataService
    {
        public ExampleRepository() : base() { }
        public ExampleRepository(string connectionStringName) : base(connectionStringName)
        {

        }
        /*
        public ChartModel GetKalipCount(string? year)
        {
            try
            {
                OpenConn();
                ChartModel report;
                if (year != null)
                {
                    report = db.QueryFirstOrDefault<ChartModel>("select COUNT(NeredeId) AS 'Count' from FaultTrackings where NeredeId in (21,30) And Year(TalepTarihi)=" + year, param);
                }
                else
                {
                    report = db.QueryFirstOrDefault<ChartModel>("select COUNT(NeredeId) AS 'Count' from FaultTrackings where NeredeId in (21,30)", param);
                }
                param = new DynamicParameters();
                if (IfHaveTransactionReturn() == null && isBeginedConnection == false)
                    CloseConn();

                return report;

            }
            catch (Exception ex)
            {

                return null;
            }

        }
        */
    }
}
