using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;

namespace HCMBLL
{
    public partial class PromotionsRecordsQualificationsPointsBLL : CommonEntity, IEntity
    {
        public int PromotionRecordQualificationPointID { get; set; }

        public PromotionsRecordsBLL PromotionRecord { get; set; }

        public QualificationsDegreesBLL QualificationDegree { get; set; }

        public QualificationsBLL Qualification { get; set; }

        public GeneralSpecializationsBLL GeneralSpecialization { get; set; }

        public PromotionsRecordsQualificationsKindsBLL PromotionRecordQualificationKind { get; set; }

        public decimal? Points { get; set; }

        public int Add(PromotionsRecordsQualificationsPointsBLL PromotionRecordQualificationPoint)
        {
            try
            {
                PromotionsRecordsQualificationsPoints PromotionRecordQualificationPointObj = new PromotionsRecordsQualificationsPoints();
                PromotionRecordQualificationPointObj.PromotionRecordID = PromotionRecordQualificationPoint.PromotionRecord.PromotionRecordID;
                PromotionRecordQualificationPointObj.QualificationDegreeID = PromotionRecordQualificationPoint.QualificationDegree.QualificationDegreeID;
                PromotionRecordQualificationPointObj.QualificationID = PromotionRecordQualificationPoint.Qualification != null ? PromotionRecordQualificationPoint.Qualification.QualificationID : (int?)null;
                PromotionRecordQualificationPointObj.GeneralSpecializationID = PromotionRecordQualificationPoint.GeneralSpecialization != null ? PromotionRecordQualificationPoint.GeneralSpecialization.GeneralSpecializationID : (int?)null;
                PromotionRecordQualificationPointObj.Points = 0;
                PromotionRecordQualificationPointObj.CreatedBy = PromotionRecordQualificationPoint.LoginIdentity.EmployeeCodeID;
                PromotionRecordQualificationPointObj.CreatedDate = DateTime.Now;

                return new PromotionsRecordsQualificationsPointsDAL().Insert(PromotionRecordQualificationPointObj);
                //return new Result
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsQualificationsPointsBLL> GetPromotionsRecordsQualificationsPoints()
        {
            List<PromotionsRecordsQualificationsPoints> PromotionsRecordsQualificationsPointsList = new PromotionsRecordsQualificationsPointsDAL().GetPromotionsRecordsQualificationsPoints();
            List<PromotionsRecordsQualificationsPointsBLL> PromotionsRecordsQualificationsPointsBLLList = new List<PromotionsRecordsQualificationsPointsBLL>();
            if (PromotionsRecordsQualificationsPointsList.Count > 0)
                foreach (var item in PromotionsRecordsQualificationsPointsList)
                    PromotionsRecordsQualificationsPointsBLLList.Add(new PromotionsRecordsQualificationsPointsBLL().MapPromotionRecordQualificationPoint(item));

            return PromotionsRecordsQualificationsPointsBLLList;
        }

        public List<PromotionsRecordsQualificationsPointsBLL> GetByPromotionRecordID(int PromotionRecordID, out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                List<PromotionsRecordsQualificationsPoints> PromotionsRecordsQualificationsPointsList =
                    new PromotionsRecordsQualificationsPointsDAL()
                    {
                        search = Search,
                        order = Order,
                        orderByColumnName = OrderByColumnName,
                        orderDir = OrderDir,
                        startRec = StartRec,
                        pageSize = PageSize
                    }.GetByPromotionRecordID(PromotionRecordID, out totalRecordsOut, out recFilterOut);
                List<PromotionsRecordsQualificationsPointsBLL> PromotionsRecordsQualificationsPointsBLLList = new List<PromotionsRecordsQualificationsPointsBLL>();
                if (PromotionsRecordsQualificationsPointsList.Count > 0)
                    foreach (var item in PromotionsRecordsQualificationsPointsList)
                        PromotionsRecordsQualificationsPointsBLLList.Add(new PromotionsRecordsQualificationsPointsBLL().MapPromotionRecordQualificationPoint(item));

                return PromotionsRecordsQualificationsPointsBLLList;
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsQualificationsPointsBLL> GetByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                List<PromotionsRecordsQualificationsPoints> PromotionsRecordsQualificationsPointsList =
                    new PromotionsRecordsQualificationsPointsDAL().GetByPromotionRecordID(PromotionRecordID);
                List<PromotionsRecordsQualificationsPointsBLL> PromotionsRecordsQualificationsPointsBLLList = new List<PromotionsRecordsQualificationsPointsBLL>();
                if (PromotionsRecordsQualificationsPointsList.Count > 0)
                    foreach (var item in PromotionsRecordsQualificationsPointsList)
                        PromotionsRecordsQualificationsPointsBLLList.Add(new PromotionsRecordsQualificationsPointsBLL().MapPromotionRecordQualificationPoint(item));

                return PromotionsRecordsQualificationsPointsBLLList;
            }
            catch
            {
                throw;
            }
        }

        public int RemoveAllFromPromotionRecord(int PromotionRecordID)
        {
            try
            {
                return new PromotionsRecordsQualificationsPointsDAL().DeleteByPromotionRecordID(PromotionRecordID, LoginIdentity.EmployeeCodeID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update Qualification Kind ID and Points on PromotionsRecordsQualificationsPoints
        /// Points Calculted By this.GetPointsByQualifications() 
        /// </summary>
        /// <returns></returns>
        public Result UpdateQualificationKindAndPoints()
        {
            try
            {
                Result result = new Result();
                PromotionsRecordsQualificationsPoints PromotionRecordQualificationPoint = new PromotionsRecordsQualificationsPoints();

                #region validation
                //if (this.IsDocumentSent(this.PromotionRecordQualificationPointID))
                //{
                //    result.Entity = null;
                //    result.EnumType = typeof(PromotionsRecordsQualificationsPointsValidationEnum);
                //    result.EnumMember = PromotionsRecordsQualificationsPointsValidationEnum.RejectedBecauseOfAlreadySent.ToString();
                //    return result;
                //}
                //else if (this.IsDocumentReceived(this.PromotionRecordQualificationPointID))
                //{
                //    result.Entity = null;
                //    result.EnumType = typeof(PromotionsRecordsQualificationsPointsValidationEnum);
                //    result.EnumMember = PromotionsRecordsQualificationsPointsValidationEnum.RejectedBecauseOfAlreadyReceived.ToString();
                //    return result;
                //} 
                #endregion

                PromotionRecordQualificationPoint.PromotionRecordQualificationPointID = this.PromotionRecordQualificationPointID;
                PromotionRecordQualificationPoint.PromotionRecordQualificationKindID = this.PromotionRecordQualificationKind.PromotionRecordQualificationKindID;
                this.Points = this.GetPointsByQualifications();
                PromotionRecordQualificationPoint.Points = this.Points;
                PromotionRecordQualificationPoint.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                PromotionRecordQualificationPoint.LastUpdatedDate = DateTime.Now;

                new PromotionsRecordsQualificationsPointsDAL().UpdateQualificationKindAndPoints(PromotionRecordQualificationPoint);

                result.Entity = this;
                result.EnumType = typeof(PromotionsRecordsQualificationsPointsValidationEnum);
                result.EnumMember = PromotionsRecordsQualificationsPointsValidationEnum.Done.ToString();

                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This function returns Points according to follow rules:
        /// 1- First We need to check GeneralSpecializationID in PromotionRecordQualificationPoint Table if 'GeneralSpecializationID != null then check points in 'GeneralSpecialization table' and return points. (if points is null return 0)
        /// 2- if 'GeneralSpecializationID == null in PromotionRecordQualificationPoint Table it means we need to check QualificationID in same table, if not null then get points from Qualification table. (if points is null return 0)
        /// 3- if 'QualificationID == null in PromotionRecordQualificationPoint Table it means we need to check QualificationDegreeID in same table, if not null then get points from QualificationDegree table. (if points is null return 0)
        /// </summary>
        /// <returns></returns>
        private decimal GetPointsByQualifications()
        {
            decimal points = 0;

            int KindID = this.PromotionRecordQualificationKind.PromotionRecordQualificationKindID;

            if (this.GeneralSpecialization != null && this.GeneralSpecialization.GeneralSpecializationID > 0)
            {
                GeneralSpecializationsBLL GS = new GeneralSpecializationsBLL().GetByGeneralSpecializationID(this.GeneralSpecialization.GeneralSpecializationID);
                points = GetDirectIndirectPoints(GS.DirectPoints, GS.IndirectPoints, KindID);
            }
            else if (this.Qualification != null && this.Qualification.QualificationID > 0)
            {
                QualificationsBLL Q = new QualificationsBLL().GetByQualificationID(this.Qualification.QualificationID);
                points = GetDirectIndirectPoints(Q.DirectPoints, Q.IndirectPoints, KindID);
            }
            else if (this.QualificationDegree != null && this.QualificationDegree.QualificationDegreeID > 0)
            {
                QualificationsDegreesBLL QD = new QualificationsDegreesBLL().GetByQualificationDegreeID(this.QualificationDegree.QualificationDegreeID);
                points = GetDirectIndirectPoints(QD.DirectPoints, QD.IndirectPoints, KindID);
            }

            return points;
        }

        private decimal GetDirectIndirectPoints(decimal? DirectPoints, decimal? IndirectPoints, int KindID)
        {
            if (KindID == (int)PromotionsRecordsQualificationsKindsEnum.Direct)
                return DirectPoints.HasValue ? DirectPoints.Value : 0;
            else if (KindID == (int)PromotionsRecordsQualificationsKindsEnum.Indirect)
                return IndirectPoints.HasValue ? IndirectPoints.Value : 0;
            else
                return 0;
        }

        internal PromotionsRecordsQualificationsPointsBLL MapPromotionRecordQualificationPoint(PromotionsRecordsQualificationsPoints PromotionRecordEmployee)
        {
            try
            {
                PromotionsRecordsQualificationsPointsBLL PromotionRecordQualificationPoint = null;
                if (PromotionRecordEmployee != null)
                {
                    PromotionRecordQualificationPoint = new PromotionsRecordsQualificationsPointsBLL();
                    PromotionRecordQualificationPoint.PromotionRecordQualificationPointID = PromotionRecordEmployee.PromotionRecordQualificationPointID;
                    PromotionRecordQualificationPoint.PromotionRecord = new PromotionsRecordsBLL().MapPromotionRecord(PromotionRecordEmployee.PromotionsRecords);
                    PromotionRecordQualificationPoint.QualificationDegree = new QualificationsDegreesBLL().MapQualificationDegree(PromotionRecordEmployee.QualificationsDegrees);
                    PromotionRecordQualificationPoint.Qualification = new QualificationsBLL().MapQualification(PromotionRecordEmployee.Qualifications);
                    PromotionRecordQualificationPoint.GeneralSpecialization = new GeneralSpecializationsBLL().MapGeneralSpecialization(PromotionRecordEmployee.GeneralSpecializations);
                    //PromotionRecordQualificationPoint.Points = PromotionRecordEmployee.Points.HasValue ? Math.Round(PromotionRecordEmployee.Points.Value, 2) : PromotionRecordEmployee.Points;
                    PromotionRecordQualificationPoint.Points = PromotionRecordEmployee.Points;
                    PromotionRecordQualificationPoint.PromotionRecordQualificationKind = new PromotionsRecordsQualificationsKindsBLL().MapPromotionRecordQualificationKind(PromotionRecordEmployee.PromotionsRecordsQualificationsKinds);
                }
                return PromotionRecordQualificationPoint;
            }
            catch
            {
                throw;
            }
        }
    }
}