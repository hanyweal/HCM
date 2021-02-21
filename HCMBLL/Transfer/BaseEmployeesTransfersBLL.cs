using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class BaseTransfersBLL : CommonEntity, IEntity
    {
        public virtual int TransferID { get; set; }

        public virtual DateTime TransferDate { get; set; }

        public virtual string Destination { get; set; }

        public virtual KSACitiesBLL KSACity { get; set; }

        public virtual TransfersTypesBLL TransferType { get; set; }

        public virtual EmployeesCodesBLL EmployeeCode { get; set; }

        public virtual EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public bool? IsProcessed { get; set; }

        internal virtual Transfers DALInstance
        {
            get
            {
                Transfers EmployeeTransfer = new Transfers();
                if (this.TransferID <= 0)
                {
                    EmployeeTransfer.CreatedDate = DateTime.Now;
                    EmployeeTransfer.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                }
                else
                {
                    EmployeeTransfer.TransferID = this.TransferID;
                    EmployeeTransfer.LastUpdatedDate = DateTime.Now;
                    EmployeeTransfer.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                }

                EmployeeTransfer.Destination = this.Destination;
                EmployeeTransfer.EmployeeCareerHistoryID = this.EmployeeCareerHistory.EmployeeCareerHistoryID;
                EmployeeTransfer.KSACityID = this.KSACity.KSACityID;
                EmployeeTransfer.TransferTypeID = this.TransferType.TransferTypeID;
                EmployeeTransfer.TransferDate = this.TransferDate;
                EmployeeTransfer.IsProcessed = false;

                return EmployeeTransfer;
            }
        }

        public virtual Result Add()
        {
            Result result = null;

            #region checking new transfer record date must be less than 'Join Date' of hiring record
            EmployeesCareersHistoryBLL HiringRecord = new EmployeesCareersHistoryBLL().GetByEmployeeCareerHistoryID(this.EmployeeCareerHistory.EmployeeCareerHistoryID);
            if (HiringRecord != null && HiringRecord.JoinDate.Date >= this.TransferDate.Date)
            {
                result = new Result();
                result.Entity = HiringRecord;
                result.EnumType = typeof(CareersHistoryValidationEnum);
                result.EnumMember = TransfersValidationEnum.RejectedBecauseOfTransferDateIsLessThanHiringDateDate.ToString();
                return result;
            }
            #endregion

            return result;
        }

        public virtual Result Update()
        {
            Result result = null;

            #region Checking new transfer record date must be less than 'Join Date' of hiring record
            EmployeesCareersHistoryBLL HiringRecord = new EmployeesCareersHistoryBLL().GetByEmployeeCareerHistoryID(this.EmployeeCareerHistory.EmployeeCareerHistoryID);
            if (HiringRecord != null && HiringRecord.JoinDate.Date >= this.TransferDate.Date)
            {
                result = new Result();
                result.Entity = HiringRecord;
                result.EnumType = typeof(CareersHistoryValidationEnum);
                result.EnumMember = TransfersValidationEnum.RejectedBecauseOfTransferDateIsLessThanHiringDateDate.ToString();
                return result;
            }
            #endregion

            #region Checking the transfer record is proceede or not
            bool IsDone = this.IsAlreadyProcessed(this.TransferID);
            if (IsDone)
            {
                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(CareersHistoryValidationEnum);
                result.EnumMember = TransfersValidationEnum.RejectedBecauseOfAlreadyProcessed.ToString();
                return result;
            }
            #endregion

            return result;
        }

        public virtual Result Remove(int TransferID)
        {
            try
            {
                Result result = null;

                #region Checking the transfer record is proceede or not
                bool IsDone = this.IsAlreadyProcessed(TransferID);
                if (IsDone)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(CareersHistoryValidationEnum);
                    result.EnumMember = TransfersValidationEnum.RejectedBecauseOfAlreadyProcessed.ToString();
                    return result;
                }
                #endregion

                new TransfersDAL().Delete(new Transfers() { TransferID = TransferID }, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(TransfersValidationEnum),
                    EnumMember = TransfersValidationEnum.Done.ToString()
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// it will be called by windows service to disactive employee based on transfer date
        /// </summary>
        /// <returns></returns>
        public virtual Result StartProcess()
        {
            try
            {
                Result result = new Result();
                List<BaseTransfersBLL> TransfersNotProcessed = this.GetTransfersNotProcessed();
                foreach (var item in TransfersNotProcessed)
                {
                    CareersHistoryTypesEnum CareerHistoryTypeEnum = item.TransferType.TransferTypeID == Convert.ToInt16(TransfersTypesEnum.TransferEmployeeWithJob) ? CareersHistoryTypesEnum.TransfareWithJob : CareersHistoryTypesEnum.TransfareWithoutJob;

                    if (DateTime.Now.Date >= item.TransferDate.Date)
                    {
                        #region Adding new record with new career history type (Transfare With Job) in career history of employees
                        // check if the job was is exists or not
                        EmployeesCareersHistoryBLL EmployeeCareerHistory = new EmployeesCareersHistoryBLL().Get(item.EmployeeCareerHistory.OrganizationJob, CareerHistoryTypeEnum, item.EmployeeCareerHistory.EmployeeCode);
                        if (EmployeeCareerHistory == null)
                        {
                            result = new EmployeesCareersHistoryBLL()
                            {
                                EmployeeCode = item.EmployeeCareerHistory.EmployeeCode,
                                OrganizationJob = item.EmployeeCareerHistory.OrganizationJob,
                                CareerHistoryType = new CareersHistoryTypesBLL() { CareerHistoryTypeID = (int)CareerHistoryTypeEnum },
                                CareerDegree = item.EmployeeCareerHistory.CareerDegree,
                                JoinDate = item.TransferDate,
                                TransactionStartDate = item.TransferDate,
                                IsActive = false,
                                LoginIdentity = item.CreatedBy,
                            }.Add();

                            if (result.EnumMember != CareersHistoryValidationEnum.Done.ToString())
                                return result;
                            else
                            {
                                #region Disactive all career hsitory of employee
                                new EmployeesCareersHistoryBLL() { LoginIdentity = item.CreatedBy }.DeactivateAllCareerHistoryOfEmployee(item.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
                                #endregion
                            }
                        }
                        #endregion

                        #region Disactive employee
                        result = new EmployeesCodesBLL() { LoginIdentity = item.CreatedBy }.DeactivateEmployee(item.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
                        if (result.EnumMember != CareersHistoryValidationEnum.Done.ToString())
                            return result;
                        #endregion

                        #region Disactive his job in case of transfer type is "Transfare with his job"
                        if (item.TransferType.TransferTypeID == (int)TransfersTypesEnum.TransferEmployeeWithJob)
                        {
                            result = new OrganizationsJobsBLL() { LoginIdentity = item.CreatedBy }.Transferring(item.EmployeeCareerHistory.OrganizationJob.OrganizationJobID);
                            if (result.EnumMember != CareersHistoryValidationEnum.Done.ToString())
                                return result;
                        }
                        #endregion

                        #region Set his job as job vacant in case of transfer type is "Transfare without his job"
                        if (item.TransferType.TransferTypeID == (int)TransfersTypesEnum.TransferEmployeeWithoutJob)
                        {
                            result = new OrganizationsJobsBLL() { LoginIdentity = item.CreatedBy }.SetJobAsVacant(item.EmployeeCareerHistory.OrganizationJob.OrganizationJobID);
                        }
                        #endregion

                        #region Update is procees to true
                        result = new BaseTransfersBLL() { LoginIdentity = item.CreatedBy }.MarkIsProcessToDone(item.TransferID);
                        if (result.EnumMember != CareersHistoryValidationEnum.Done.ToString())
                            return result;
                        #endregion
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal virtual Result MarkIsProcessToDone(int TransferID)
        {
            try
            {
                Result result = null;
                new TransfersDAL().UpdateIsProcess(new Transfers()
                {
                    TransferID = TransferID,
                    IsProcessed = true,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                });
                return result = new Result()
                {
                    EnumType = typeof(TransfersValidationEnum),
                    EnumMember = TransfersValidationEnum.Done.ToString()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<BaseTransfersBLL> GetTransfers()
        {
            try
            {
                List<BaseTransfersBLL> EmployeeTransferBLLList = new List<BaseTransfersBLL>();
                List<Transfers> Transfers = new TransfersDAL().GetTransfers();
                Transfers.ForEach(x => EmployeeTransferBLLList.Add(MapTransfer(x)));

                return EmployeeTransferBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal virtual List<BaseTransfersBLL> GetTransfersNotProcessed()
        {
            try
            {
                List<BaseTransfersBLL> EmployeeTransferBLLList = new List<BaseTransfersBLL>();
                List<Transfers> Transfers = new TransfersDAL().GetTransfersNotProccessed();
                Transfers.ForEach(x => EmployeeTransferBLLList.Add(MapTransfer(x)));

                return EmployeeTransferBLLList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        internal virtual bool IsAlreadyProcessed(int TransferID)
        {
            BaseTransfersBLL nn = this.GetByTransferID(TransferID);
            if (nn.IsProcessed.HasValue && nn.IsProcessed.Value == true)
                return true;

            return false;
        }

        public BaseTransfersBLL GetByTransferID(int TransferID)
        {
            try
            {
                BaseTransfersBLL EmployeeTransferBLL = null;
                Transfers EmployeeTransfer = new TransfersDAL().GetByTransferID(TransferID);
                if (EmployeeTransfer != null)
                    EmployeeTransferBLL = MapTransfer(EmployeeTransfer);

                return EmployeeTransferBLL;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        internal BaseTransfersBLL MapTransfer(Transfers Transfer)
        {
            try
            {
                BaseTransfersBLL BaseTransfersBLL = null;
                if (Transfer != null)
                {
                    if (Transfer.TransferTypeID == Convert.ToInt16(TransfersTypesEnum.TransferEmployeeWithJob))
                    {
                        BaseTransfersBLL = GenericFactoryPattern<BaseTransfersBLL, TransferEmployeesWithJobBLL>.CreateInstance();
                    }

                    else if (Transfer.TransferTypeID == Convert.ToInt16(TransfersTypesEnum.TransferEmployeeWithoutJob))
                    {
                        BaseTransfersBLL = GenericFactoryPattern<BaseTransfersBLL, TransferEmployeesWithoutJobBLL>.CreateInstance();
                        ((TransferEmployeesWithoutJobBLL)BaseTransfersBLL).JobName = Transfer.JobName;
                        ((TransferEmployeesWithoutJobBLL)BaseTransfersBLL).RankName = Transfer.RankName;
                        ((TransferEmployeesWithoutJobBLL)BaseTransfersBLL).JobCode = Transfer.JobCode;
                        ((TransferEmployeesWithoutJobBLL)BaseTransfersBLL).CareerDegreeName = Transfer.CareerDegreeName;
                        ((TransferEmployeesWithoutJobBLL)BaseTransfersBLL).OrganizationName = Transfer.OrganizationName;
                    }

                    //BaseTransfersBLL = new BaseTransfersBLL();

                    BaseTransfersBLL.Destination = Transfer.Destination;
                    BaseTransfersBLL.KSACity = new KSACitiesBLL() { KSACityID = Transfer.KSACityID };
                    BaseTransfersBLL.TransferID = Transfer.TransferID;
                    BaseTransfersBLL.TransferType = new TransfersTypesBLL() { TransferTypeID = Transfer.TransferTypeID, TransferType = Transfer.TransfersTypes?.TransferTypeName };
                    BaseTransfersBLL.TransferDate = Transfer.TransferDate;
                    BaseTransfersBLL.EmployeeCareerHistory = Transfer.EmployeesCareersHistory != null ? new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(Transfer.EmployeesCareersHistory) : null;
                    BaseTransfersBLL.IsProcessed = Transfer.IsProcessed;
                    BaseTransfersBLL.CreatedDate = Transfer.CreatedDate;
                    BaseTransfersBLL.CreatedBy = Transfer.CreatedByNav != null ? new EmployeesCodesBLL().MapEmployeeCode(Transfer.CreatedByNav) : null;
                    BaseTransfersBLL.LastUpdatedDate = Transfer.LastUpdatedDate;
                    BaseTransfersBLL.LastUpdatedBy = Transfer.LastUpdatedByNav != null ? new EmployeesCodesBLL().MapEmployeeCode(Transfer.LastUpdatedByNav) : null;
                }
                return BaseTransfersBLL;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
