using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Data.Models
{
    public interface IObjFileRepository
    {
        ObjFile Add(ObjFile objFile);
        IQueryable<ObjFile> GetAllFiles();
        List<ObjFile> GetAllFilesBind();
        ObjFile GetById(params object[] id);
        bool Update(ObjFile objFile);
        bool Delete(ObjFile objFile);

    }
}
