using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class VacationsDAL
    {
        public int Insert(Vacations Vacation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Vacations.Add(Vacation);
                    db.SaveChanges();
                    return Vacation.VacationID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Update(Vacations Vacation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Vacations VacationObj = db.Vacations.FirstOrDefault(x => x.VacationID.Equals(Vacation.VacationID));
                    VacationObj.VacationStartDate = Vacation.VacationStartDate;
                    VacationObj.VacationEndDate = Vacation.VacationEndDate;
                    VacationObj.OldBalanceConsumed = Vacation.OldBalanceConsumed;
                    VacationObj.IsCanceled = Vacation.IsCanceled;
                    VacationObj.LastUpdatedBy = Vacation.LastUpdatedBy;
                    VacationObj.LastUpdatedDate = Vacation.LastUpdatedDate;
                    VacationObj.StudiesVacationStartDate = Vacation.StudiesVacationStartDate;
                    VacationObj.SickVacationTypeID = Vacation.SickVacationTypeID;
                    VacationObj.IsSickLeaveAmountPaid = Vacation.IsSickLeaveAmountPaid;
                    VacationObj.NormalVacationTypeID = Vacation.NormalVacationTypeID;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateVacationDates(Vacations Vacation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Vacations VacationObj = db.Vacations.FirstOrDefault(x => x.VacationID.Equals(Vacation.VacationID));
                    VacationObj.VacationStartDate = Vacation.VacationStartDate;
                    VacationObj.VacationEndDate = Vacation.VacationEndDate;
                    VacationObj.LastUpdatedBy = Vacation.LastUpdatedBy;
                    VacationObj.LastUpdatedDate = Vacation.LastUpdatedDate;
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(int VacationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Vacations VacationObj = db.Vacations.FirstOrDefault(x => x.VacationID.Equals(VacationID));
                    db.Vacations.Remove(VacationObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vacations> GetEmployeeVacations(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations.Include("VacationsDetails")
                                    .Include("VacationsTypes")
                                    .Include("SickVacationsTypes")
                                    .Include("SportsSeasons")
                                    .Include("CreatedByNav.Employees")
                                    .Include("VacationsDetails.VacationsActions")
                                    .Include("VacationsDetails.CreatedByNav.Employees")
                                    .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Vacations GetByVacationID(int VacationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations.Include("EmployeesCareersHistory")
                                    .Include("VacationsDetails")
                                    .Include("VacationsTypes")
                                    .Include("SickVacationsTypes")
                                    .Include("CreatedByNav.Employees")
                                    .Include("VacationsDetails.VacationsActions")
                                    .Include("VacationsDetails.CreatedByNav.Employees")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("EmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                    .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                                    .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks.RanksCategories")
                                    .Include("EmployeesCareersHistory.CareersHistoryTypes")
                                    .Include("EmployeesCareersHistory.CareersDegrees")
                                    .FirstOrDefault(x => x.VacationID == VacationID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<Vacations> GetCanceledByEmployeeCodeIDVacationTypeID(int EmployeeCodeID, int VacationTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations
                                    .Include("VacationsTypes")
                                    .Include("CreatedByNav.Employees")
                                    .Include("VacationsDetails")
                                    .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID
                                            && x.VacationTypeID == VacationTypeID
                                            && x.IsCanceled == true

                               ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<Vacations> GetFinishedByEmployeeCodeIDVacationTypeID(int EmployeeCodeID, int VacationTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations
                                    .Include("VacationsTypes")
                                    .Include("CreatedByNav.Employees")
                                    .Include("VacationsDetails")
                                    .Include("NormalVacationsTypes")
                                    .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID
                                            && x.VacationEndDate < DateTime.Now
                                            && x.VacationsTypes.VacationTypeID == VacationTypeID
                                            && x.IsCanceled != true).OrderByDescending(x => x.VacationStartDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<Vacations> GetValidByEmployeeCodeIDVacationTypeID(int EmployeeCodeID, int VacationTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations
                                    .Include("VacationsTypes")
                                    .Include("CreatedByNav.Employees")
                                    .Include("VacationsDetails")
                                    .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID
                                            && x.VacationsTypes.VacationTypeID == VacationTypeID
                                            && x.VacationEndDate >= DateTime.Now
                                            && x.IsCanceled != true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<Vacations> GetValidByEmployeeCodeIDVacationTypeID(int EmployeeCodeID, int VacationTypeID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations
                                    .Include("VacationsTypes")
                                    .Include("CreatedByNav.Employees")
                                    .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID
                                            && x.VacationsTypes.VacationTypeID == VacationTypeID
                                            && x.VacationStartDate >= StartDate
                                            && x.VacationEndDate <= EndDate
                                            && x.IsCanceled != true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vacations> GetValidByVacationTypeIDDuraingPeriod(int? VacationTypeID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations
                                    .Include("VacationsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("EmployeesCareersHistory.OrganizationsJobs")
                                    .Where(x => (x.VacationTypeID == VacationTypeID || VacationTypeID == null) &&
                                                 ((StartDate >= x.VacationStartDate && StartDate <= x.VacationEndDate) ||
                                                 (EndDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                                                 (StartDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                                                 (StartDate <= x.VacationStartDate && EndDate >= x.VacationEndDate))
                                            && x.IsCanceled != true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<Vacations> GetLastNVacationsEmployeeCodeID(int EmployeeCodeID, int DaysCount)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations
                                    .Include("VacationsTypes")
                                    .Include("CreatedByNav.Employees")
                                    .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID
                                            && x.IsCanceled != true)
                                            .OrderByDescending(c => c.VacationStartDate)
                                            .Take(DaysCount)
                                            .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vacations> GetValidByHolidayAttendanceDetailID(int HolidayAttendanceDetailID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations
                                    .Include("VacationsTypes")
                                    .Include("CreatedByNav.Employees")
                                    .Where(x => x.HolidayAttendanceDetailID == HolidayAttendanceDetailID
                                            && x.IsCanceled != true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vacations> GetVacationsBySportsSeasonID(int SportsSeasonID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations.Include("EmployeesCareersHistory")
                                    .Include("VacationsDetails")
                                    .Include("VacationsTypes")
                                    .Include("SickVacationsTypes")
                                    .Include("CreatedByNav.Employees")
                                    .Include("VacationsDetails.VacationsActions")
                                    .Include("VacationsDetails.CreatedByNav.Employees")
                                    .Where(x => x.SportsSeasonID == SportsSeasonID
                                            && x.IsCanceled != true).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vacations> GetEmployeeVacationsByVacationTypeNotCancelled(int EmployeeCodeID, int VacationTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations.Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID
                                                && x.IsCanceled == false
                                                && x.VacationTypeID == VacationTypeID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetCountByTypeEmployeeCareerHistoryIDAndDateDuration(int VacationTypeID, int EmployeeCareerHistoryID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var db = new HCMEntities();
                return db.Vacations.Where(x => x.EmployeeCareerHistoryID == EmployeeCareerHistoryID &&
                                                x.VacationTypeID == VacationTypeID &&
                                                 ((StartDate >= x.VacationStartDate && StartDate <= x.VacationEndDate) ||
                                                 (EndDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                                                 (StartDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                                                 (StartDate <= x.VacationStartDate && EndDate >= x.VacationEndDate))
                                               ).Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vacations> GetEmployeesVacationsByDate(DateTime VacationDate, int VacationTypeID, List<int> EmployeesCareerHistoryIDs)
        {
            var db = new HCMEntities();
            return db.Vacations.Include("VacationsTypes")
                               .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks.RanksCategories")
                               .Where(x => (VacationDate >= x.VacationStartDate && VacationDate <= x.VacationEndDate)
                                            && (VacationTypeID != 0 ? x.VacationTypeID == VacationTypeID : x.VacationTypeID == x.VacationTypeID)
                                            && EmployeesCareerHistoryIDs.Contains(x.EmployeeCareerHistoryID)
                                            && x.IsCanceled == false).ToList();
        }

        public List<Vacations> GetEmployeesVacationsByTwoDates(List<int> EmployeesCareersHistoryIDs, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var db = new HCMEntities();
                List<Vacations> VacationList = db.Vacations.Where
                    (
                        x => x.IsCanceled == false
                        && EmployeesCareersHistoryIDs.Contains(x.EmployeeCareerHistoryID)
                        && ((StartDate >= x.VacationStartDate && StartDate <= x.VacationEndDate) ||
                            (EndDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                            (StartDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                            (StartDate <= x.VacationStartDate && EndDate >= x.VacationEndDate))
                    ).ToList();

                return VacationList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vacations> GetEmployeeVacationsBetweenTwoDates(int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var db = new HCMEntities();
                List<Vacations> VacationList = db.Vacations.Where
                    (
                        x => x.IsCanceled == false
                        && x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID
                        && ((StartDate >= x.VacationStartDate && StartDate <= x.VacationEndDate) ||
                            (EndDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                            (StartDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                            (StartDate <= x.VacationStartDate && EndDate >= x.VacationEndDate))
                    ).ToList();

                return VacationList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
