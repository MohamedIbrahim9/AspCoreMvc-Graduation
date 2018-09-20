using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Data.Models
{
    public interface IDxfFileRepository
    {
        DxfFile Add(DxfFile dxfFile);
        IQueryable<DxfFile> GetAllFiles();
        List<DxfFile> GetAllFilesBind();
        DxfFile GetById(params object[] id);
        bool Update(DxfFile  dxfFile);
        bool Delete(DxfFile dxfFile);

    }
}
