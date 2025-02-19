using Dapper;
using LojiteksWeb.Dapper.DapperClass;
using LojiteksWeb.Dapper.DapperModel;

namespace LojiteksWeb.Dapper.DapperRepository
{
    public class MonthlyDataRepository : DataService
    {
        public MonthlyDataRepository() : base() { }
        public MonthlyDataRepository(string connectionStringName) : base(connectionStringName)
        {

        }

        public List<MonthlyDataModel> GetMonthlyData(long firmaID)
        {
            try
            {
                OpenConn();
                List<MonthlyDataModel> report = new List<MonthlyDataModel>();
                var startDate = DateTime.Today.AddMonths(-6).Date;
                var endDate = DateTime.Today.Date;
                string sql = "";
                if (firmaID != 1)
                {
                    sql = @"
                         SELECT 
                                YEAR(gonderimTarihi) AS Year, 
                                MONTH(gonderimTarihi) AS Month, 
                                Count(*) AS Total
                            FROM 
                                tblBaslik
                            WHERE 
                                firma = @FirmaID AND gonderimTarihi >= @StartDate AND gonderimTarihi <= @EndDate
                            GROUP BY 
                                YEAR(gonderimTarihi), MONTH(gonderimTarihi)
                            ORDER BY 
                                Year DESC, Month DESC;
                        ";
                    report = db.Query<MonthlyDataModel>(sql, new { FirmaID = firmaID, StartDate = startDate, EndDate = endDate }).ToList();
                }
                else
                {
                    sql = @"
                         SELECT 
                                YEAR(gonderimTarihi) AS Year, 
                                MONTH(gonderimTarihi) AS Month, 
                                Count(*) AS Total
                            FROM 
                                tblBaslik
                            WHERE 
                                gonderimTarihi >= @StartDate AND gonderimTarihi <= @EndDate
                            GROUP BY 
                                YEAR(gonderimTarihi), MONTH(gonderimTarihi)
                            ORDER BY 
                                Year DESC, Month DESC;
                        ";
                    report = db.Query<MonthlyDataModel>(sql, new {StartDate = startDate, EndDate = endDate }).ToList();
                }


                report = db.Query<MonthlyDataModel>(sql, new { FirmaID = firmaID, StartDate = startDate, EndDate = endDate }).ToList();

                if (IfHaveTransactionReturn() == null && isBeginedConnection == false)
                    CloseConn();

                return report;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CustomerOrderCountModel> GetCustomerOrderCountModels(long firmaID)
        {
            try
            {
                OpenConn();
                List<CustomerOrderCountModel> report = new List<CustomerOrderCountModel>();
                var startDate = DateTime.Today.AddMonths(-6).Date;
                var endDate = DateTime.Today.Date;

                string sql = "";
                if (firmaID != 1)
                {
                    sql = @"
                    SELECT 
                        M.Musteri AS MusteriAd ,
                        COUNT(*) AS TotalOrderCount
                    FROM 
                        tblBaslik B
                    JOIN
                        tblMusteri M ON B.musteri = M.mkno
                    WHERE 
                       b.firma = @FirmaID AND B.gonderimTarihi >= @StartDate AND B.gonderimTarihi <= @EndDate
                    GROUP BY 
                        M.musteri;
                ";
                    report = db.Query<CustomerOrderCountModel>(sql, new { FirmaID = firmaID, StartDate = startDate, EndDate = endDate }).ToList();
                }
                else
                {
                    sql = @"
                    SELECT 
                        M.Musteri AS MusteriAd ,
                        COUNT(*) AS TotalOrderCount
                    FROM 
                        tblBaslik B
                    JOIN
                        tblMusteri M ON B.musteri = M.mkno
                    WHERE 
                       B.gonderimTarihi >= @StartDate AND B.gonderimTarihi <= @EndDate
                    GROUP BY 
                        M.musteri;
                ";
                    report = db.Query<CustomerOrderCountModel>(sql, new { StartDate = startDate, EndDate = endDate }).ToList();
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

        public List<CustomerOrderCountModel> GetCustomerOrderEpcCountModels(long firmaID)
        {
            try
            {
                OpenConn();
                List<CustomerOrderCountModel> report = new List<CustomerOrderCountModel>();
                var startDate = DateTime.Today.AddMonths(-6).Date;
                var endDate = DateTime.Today.Date;

                string sql = "";
                if (firmaID != 1)
                {
                    sql = @"
                    SELECT 
                        M.Musteri AS MusteriAd ,
                        SUM(B.gonderiAdedi) AS TotalOrderCount
                    FROM 
                        tblBaslik B
                    JOIN
                        tblMusteri M ON B.musteri = M.mkno
                    WHERE 
                       b.firma = @FirmaID AND B.gonderimTarihi >= @StartDate AND B.gonderimTarihi <= @EndDate
                    GROUP BY 
                        M.musteri;
                ";
                    report = db.Query<CustomerOrderCountModel>(sql, new { FirmaID = firmaID, StartDate = startDate, EndDate = endDate }).ToList();
                }
                else
                {
                    sql = @"
                    SELECT 
                        M.Musteri AS MusteriAd ,
                        SUM(B.gonderiAdedi) AS TotalOrderCount
                    FROM 
                        tblBaslik B
                    JOIN
                        tblMusteri M ON B.musteri = M.mkno
                    WHERE 
                       B.gonderimTarihi >= @StartDate AND B.gonderimTarihi <= @EndDate
                    GROUP BY 
                        M.musteri;
                ";
                    report = db.Query<CustomerOrderCountModel>(sql, new { StartDate = startDate, EndDate = endDate }).ToList();
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
