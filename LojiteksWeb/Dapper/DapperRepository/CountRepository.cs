using Dapper;
using LojiteksWeb.Dapper.DapperClass;
using LojiteksWeb.Dapper.DapperModel;

namespace LojiteksWeb.Dapper.DapperRepository
{
    public class CountRepository : DataService
    {
        public CountRepository() : base() { }
        public CountRepository(string connectionStringName) : base(connectionStringName)
        {

        }
        
        public CountModel GetCount(long firmaID)
        {
            try
            {
                OpenConn();
                CountModel report = new CountModel();
                var sql = "";
                if (firmaID !=1)
                {
                    sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM tblBaslik WHERE firma = @FirmaID AND silindi = 0) AS OrderCount,
                    (SELECT COUNT(*) FROM tblKoli WHERE firma = @FirmaID AND silindi = 0) AS KoliCount,
                    (SELECT COUNT(*) FROM tblEpc WHERE firma = @FirmaID AND silindi = 0) AS EpcCount,
                    (SELECT COUNT(*) FROM tblCihaz Where firma = @FirmaID) AS CihazCount;
                    ";
                    report = db.QueryFirstOrDefault<CountModel>(sql, new { FirmaID = firmaID });
                }
                else
                {
                    sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM tblBaslik WHERE silindi = 0) AS OrderCount,
                    (SELECT COUNT(*) FROM tblKoli WHERE silindi = 0) AS KoliCount,
                    (SELECT COUNT(*) FROM tblEpc WHERE silindi = 0) AS EpcCount,
                    (SELECT COUNT(*) FROM tblCihaz) AS CihazCount;
                    ";
                    report = db.QueryFirstOrDefault<CountModel>(sql);
                }
                


                if (IfHaveTransactionReturn() == null && isBeginedConnection == false)
                    CloseConn();

                return report;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
