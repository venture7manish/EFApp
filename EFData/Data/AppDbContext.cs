using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFData.Models;
using Microsoft.EntityFrameworkCore;

namespace EFData.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Student> Students => Set<Student>();
        public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<StudentsCourses> Enrollments => Set<StudentsCourses>();
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<CourseInstructor> CourseInstructors => Set<CourseInstructor>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Soft delete global filter for all entities derived from BaseEntity except StudentProfile
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)
                    && entityType.ClrType != typeof(StudentProfile)) // exclude StudentProfile
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                    var genericMethod = method!.MakeGenericMethod(entityType.ClrType);
                    genericMethod.Invoke(null, new object[] { modelBuilder });
                }
            }

            // Add matching query filter to StudentProfile for soft-deleted Students
            modelBuilder.Entity<StudentProfile>()
                .HasQueryFilter(sp => !sp.Student.IsDeleted);

            // Exclude UpdatedAt and IsDeleted from StudentProfile
            modelBuilder.Entity<StudentProfile>()
                .Ignore(sp => sp.UpdatedAt)
                .Ignore(sp => sp.DeletedAt)
                .Ignore(sp => sp.IsDeleted);

            // Configure one-to-one Student - StudentProfile
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Profile)
                .WithOne(p => p.Student)
                .HasForeignKey<StudentProfile>(p => p.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Enrollment composite key
            modelBuilder.Entity<StudentsCourses>()
                .ToTable("StudentsCourses")
                .HasKey(e => e.Id);

            modelBuilder.Entity<StudentsCourses>()
                .HasIndex(e => new { e.StudentId, e.CourseId })
                .IsUnique();

            modelBuilder.Entity<StudentsCourses>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<StudentsCourses>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<CourseInstructor>()
                .HasKey(ci => new { ci.CourseId, ci.InstructorId });

            modelBuilder.Entity<CourseInstructor>()
                .HasOne(ci => ci.Course)
                .WithMany(c => c.CourseInstructors)
                .HasForeignKey(ci => ci.CourseId)
                .IsRequired(false);

            modelBuilder.Entity<CourseInstructor>()
                .HasOne(ci => ci.Instructor)
                .WithMany(i => i.CourseInstructors)
                .HasForeignKey(ci => ci.InstructorId)
                .IsRequired(false);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("dbcs",
                    b => b.MigrationsAssembly("EFData"));
            }
        }

        private static void SetSoftDeleteFilter<T>(ModelBuilder builder) where T : BaseEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
