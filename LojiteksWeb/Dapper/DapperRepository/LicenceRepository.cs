using Dapper;
using LojiteksWeb.Dapper.DapperClass;
using LojiteksWeb.Dapper.DapperModel;

namespace LojiteksWeb.Dapper.DapperRepository
{
    public class LicenceRepository : DataService
    {
        public LicenceRepository() : base() { }
        public LicenceRepository(string connectionStringName) : base(connectionStringName)
        {

        }

        public LicenceModel GetLicenceData(long firmaID)
        {
            try
            {
                OpenConn();
                LicenceModel report = new LicenceModel();
                var sql = "";
                if (firmaID != 1)
                {
                    sql = @"
                        SELECT 
                            (SELECT COUNT(*) FROM tblLisans WHERE firma = @FirmaID) AS LicenceCount,
                            (SELECT MIN(bitisTarih) FROM tblLisans WHERE firma = @FirmaID) AS ClosestLicenceEndDate,
                            (SELECT C.tanim 
                             FROM tblLisans L 
                             JOIN tblCihaz C ON L.cihaz = C.ckno 
                             WHERE L.firma = @FirmaID AND L.bitisTarih = 
                             (SELECT MIN(bitisTarih) FROM tblLisans WHERE firma = @FirmaID)
                            ) AS Device
                        ";
                    report = db.QueryFirstOrDefault<LicenceModel>(sql, new { FirmaID = firmaID });
                }
                else
                {
                    sql = @"
                        SELECT 
                            (SELECT COUNT(*) FROM tblLisans) AS LicenceCount,
                            (SELECT MIN(bitisTarih) FROM tblLisans) AS ClosestLicenceEndDate,
                            (SELECT C.tanim 
                             FROM tblLisans L 
                             JOIN tblCihaz C ON L.cihaz = C.ckno 
                             WHERE L.bitisTarih = 
                             (SELECT MIN(bitisTarih) FROM tblLisans)
                            ) AS Device
                        ";
                    report = db.QueryFirstOrDefault<LicenceModel>(sql);
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
