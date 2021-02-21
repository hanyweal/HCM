using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMDAL;
using HCMBLL.Enums;

namespace HCMBLL
{
    public class SectorsTypesBLL : CommonEntity, IEntity
    {
        public virtual int SectorTypeID { get; set; }
        public virtual string SectorTypeName { get; set; }

        public List<SectorsTypesBLL> GetSectorsTypes()
        {
            List<SectorsTypes> SectorsTypesList = new SectorsTypesDAL().GetSectorsTypes();
            List<SectorsTypesBLL> SectorsTypesListBLL = new List<SectorsTypesBLL>();
            foreach (var item in SectorsTypesList)
            {
                SectorsTypesListBLL.Add(new SectorsTypesBLL().MapSectorType(item));
            }
            return SectorsTypesListBLL;
        }

        internal SectorsTypesBLL MapSectorType(SectorsTypes SectorType)
        {
            try
            {
                SectorsTypesBLL SectorsTypeBLL = null;
                if (SectorType != null)
                {
                    SectorsTypeBLL = new SectorsTypesBLL()
                    {
                        SectorTypeID = SectorType.SectorTypeID,
                        SectorTypeName = SectorType.SectorTypeName
                    };
                }
                return SectorsTypeBLL;
            }
            catch
            {
                throw;
            }
        }

    }

    }

