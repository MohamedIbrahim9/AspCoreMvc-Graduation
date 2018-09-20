using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Data.Models
{
    public class ObjFileRepository : IObjFileRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ObjFileRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public ObjFile Add(ObjFile objFile)
        {
            _applicationDbContext.ObjFiles.Add(objFile);
            return _applicationDbContext.SaveChanges() > 0 ? objFile : null;
        }

        public bool Delete(ObjFile objFile)
        {
            _applicationDbContext.ObjFiles.Remove(objFile);
            return _applicationDbContext.SaveChanges() > 0;
        }

        public IQueryable<ObjFile> GetAllFiles()
        {
            return _applicationDbContext.ObjFiles;
        }

        public List<ObjFile> GetAllFilesBind()
        {
            return _applicationDbContext.ObjFiles.ToList();
        }

        public ObjFile GetById(params object[] id)
        {
            return _applicationDbContext.ObjFiles.Find(id);
        }

        public bool Update(ObjFile objFile)
        {
            _applicationDbContext.ObjFiles.Attach(objFile);
            _applicationDbContext.Entry(objFile).State = EntityState.Modified;
            return _applicationDbContext.SaveChanges() > 0;
        }
    }
}
