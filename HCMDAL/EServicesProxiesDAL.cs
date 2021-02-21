using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class EServicesProxiesDAL
    {
        public int Insert(EServicesProxies EServiceProxy)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EServicesProxies.Add(EServiceProxy);
                    db.SaveChanges();
                    return EServiceProxy.EServiceProxyID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Revoke(EServicesProxies EServiceProxy)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EServicesProxies EServiceProxyObj = db.EServicesProxies.FirstOrDefault(x => x.EServiceProxyID.Equals(EServiceProxy.EServiceProxyID));
                    EServiceProxyObj.EServiceProxyStatusID = EServiceProxy.EServiceProxyStatusID;
                    EServiceProxyObj.EndDate = EServiceProxy.EndDate;
                    EServiceProxyObj.IsActive = EServiceProxy.IsActive; 
                    EServiceProxyObj.LastUpdatedDate = EServiceProxy.LastUpdatedDate;
                    EServiceProxyObj.LastUpdatedBy = EServiceProxy.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EServicesProxies> Get(int EServiceTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EServicesProxies.Include("FromEmployeeCode")
                                        .Include("ToEmployeeCode")
                                        .Include("EServicesTypes")
                                        .Where(x => x.EServiceTypeID == EServiceTypeID).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EServicesProxies GetActiveByEServiceProxyID(int EServiceProxyID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EServicesProxies.Include("FromEmployeeCode")
                                        .Include("ToEmployeeCode")
                                        .Include("EServicesTypes")
                                        .FirstOrDefault(x => x.EServiceProxyID == EServiceProxyID && x.IsActive == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EServicesProxies> GetByFromEmployeeCodeID(int FromEmployeeCodeID, int? EServiceTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EServicesProxies.Include("FromEmployeeCode")
                                        .Include("ToEmployeeCode")
                                        .Include("EServicesTypes")
                                        .Where(x => x.FromEmployeeCodeID == FromEmployeeCodeID
                                            && x.EServiceTypeID == (EServiceTypeID.HasValue ? EServiceTypeID.Value : x.EServiceTypeID))
                                        .OrderByDescending(x => x.IsActive)
                                        .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EServicesProxies> GetActiveByFromEmployeeCodeID(int FromEmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EServicesProxies.Include("FromEmployeeCode")
                                        .Include("ToEmployeeCode")
                                        .Include("EServicesTypes")
                                        .Where(x => x.FromEmployeeCodeID == FromEmployeeCodeID
                                            && x.IsActive==true)
                                        .OrderByDescending(x => x.IsActive)
                                        .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EServicesProxies> GetActiveByToEmployeeCodeID(int ToEmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EServicesProxies.Include("FromEmployeeCode")
                                        .Include("ToEmployeeCode")
                                        .Include("EServicesTypes")
                                        .Where(x => x.ToEmployeeCodeID == ToEmployeeCodeID
                                            && x.IsActive == true)
                                        .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EServicesProxies> GetByToEmployeeCodeID(int ToEmployeeCodeID, int? EServiceTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EServicesProxies.Include("FromEmployeeCode")
                                        .Include("ToEmployeeCode")
                                        .Include("EServicesTypes")
                                        .Where(x => x.ToEmployeeCodeID == ToEmployeeCodeID
                                            && x.EServiceTypeID == (EServiceTypeID.HasValue ? EServiceTypeID.Value : x.EServiceTypeID))
                                        .OrderByDescending(x => x.IsActive)
                                        .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
