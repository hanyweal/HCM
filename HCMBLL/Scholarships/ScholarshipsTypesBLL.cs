﻿using HCMDAL;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class ScholarshipsTypesBLL : CommonEntity
    {
        public virtual int ScholarshipTypeID
        {
            get;
            set;
        }

        public virtual string ScholarshipType
        {
            get;
            set;
        }

        public virtual List<ScholarshipsTypesBLL> GetScholarshipsTypes()
        {
            List<ScholarshipsTypes> ScholarshipsTypesList = new ScholarshipsTypesDAL().GetScholarshipsTypes();
            List<ScholarshipsTypesBLL> ScholarshipsTypesBLLList = new List<ScholarshipsTypesBLL>();
            if (ScholarshipsTypesList.Count > 0)
            {
                foreach (var item in ScholarshipsTypesList)
                {
                    ScholarshipsTypesBLLList.Add(new ScholarshipsTypesBLL()
                    {
                        ScholarshipTypeID = item.ScholarshipTypeID,
                        ScholarshipType = item.ScholarshipType
                    });
                }
            }

            return ScholarshipsTypesBLLList;
        }

        public ScholarshipsTypesBLL GetByScholarshipTypeID(int ScholarshipTypeID)
        {
            return GetScholarshipsTypes().SingleOrDefault(x => x.ScholarshipTypeID.Equals(ScholarshipTypeID));
        }

        public virtual int Add()
        {
            throw new System.NotImplementedException();
        }

        public virtual int Remove()
        {
            throw new System.NotImplementedException();
        }

        public virtual int Update()
        {
            throw new System.NotImplementedException();
        }


        internal ScholarshipsTypesBLL MapScholarshipType(ScholarshipsTypes item)
        {
            return item != null ?
                new ScholarshipsTypesBLL()
                {
                    ScholarshipTypeID = item.ScholarshipTypeID,
                    ScholarshipType = item.ScholarshipType
                }
                : null;
        }
    }
}

