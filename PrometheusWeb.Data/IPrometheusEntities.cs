using PrometheusWeb.Data.DataModels;
using System.Data.Entity;

namespace PrometheusWeb.Data
{
    public interface IPrometheusEntities
    {
        DbSet<Assignment> Assignments { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Enrollment> Enrollments { get; set; }
        DbSet<Homework> Homework { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Teacher> Teachers { get; set; }
        DbSet<Teach> Teaches { get; set; }
        DbSet<User> Users { get; set; }
    }
}