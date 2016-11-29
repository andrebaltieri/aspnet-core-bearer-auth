using Balta.io.Domain.Entities;

namespace Balta.io.Domain.Repositories
{
    public interface IStudentRepository
    {
        void Save(Student student);
    }
}
