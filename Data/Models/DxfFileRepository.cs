using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Data.Models
{
    public class DxfFileRepository:IDxfFileRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DxfFileRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public DxfFile Add(DxfFile dxfFile)
        {
            _applicationDbContext.DxfFiles.Add(dxfFile);
            return _applicationDbContext.SaveChanges() > 0 ? dxfFile : null;
        }

        public bool Delete(DxfFile dxfFile)
        {
            _applicationDbContext.DxfFiles.Remove(dxfFile);
            return _applicationDbContext.SaveChanges() > 0;
        }

        public IQueryable<DxfFile> GetAllFiles()
        {
            return _applicationDbContext.DxfFiles;
        }

        public List<DxfFile> GetAllFilesBind()
        {
            return _applicationDbContext.DxfFiles.ToList();
        }

        public DxfFile GetById(params object[] id)
        {
            return _applicationDbContext.DxfFiles.Find(id);
        }

        public bool Update(DxfFile dxfFile)
        {
            _applicationDbContext.DxfFiles.Attach(dxfFile);
            _applicationDbContext.Entry(dxfFile).State = EntityState.Modified;
            return _applicationDbContext.SaveChanges() > 0;
        }
    }
}
