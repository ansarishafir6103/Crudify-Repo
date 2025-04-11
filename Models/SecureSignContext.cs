using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crudify.Models;

public partial class SecureSignContext : DbContext
{
    public SecureSignContext()
    {
    }

    public SecureSignContext(DbContextOptions<SecureSignContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCity> TblCities { get; set; }

    public virtual DbSet<TblCountry> TblCountries { get; set; }

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblEmployee> TblEmployees { get; set; }

    public virtual DbSet<TblFavouriteHobby> TblFavouriteHobbies { get; set; }

    public virtual DbSet<TblHobby> TblHobbies { get; set; }

    public virtual DbSet<TblState> TblStates { get; set; }

    public virtual DbSet<UserMaster> UserMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){ }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCity>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__tbl_Citi__031491A898E87571");

            entity.ToTable("tbl_Cities");

            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("city_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("city_name");
            entity.Property(e => e.StateId).HasColumnName("state_id");

            entity.HasOne(d => d.State).WithMany(p => p.TblCities)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_question_states");
        });

        modelBuilder.Entity<TblCountry>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__tbl_Coun__7E8CD055B72D3D64");

            entity.ToTable("tbl_Countries");

            entity.Property(e => e.CountryId)
                .ValueGeneratedNever()
                .HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("country_name");
            entity.Property(e => e.CountryPhonecode).HasColumnName("country_phonecode");
            entity.Property(e => e.CountryShortname)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("country_shortname");
        });

        modelBuilder.Entity<TblDepartment>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__tbl_Depa__C223242249A88E8E");

            entity.ToTable("tbl_Departments");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("department_name");
        });

        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__tbl_Empl__C52E0BA86ABA7885");

            entity.ToTable("tbl_Employees");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.EmployeeDateOfBirth)
                .HasColumnType("datetime")
                .HasColumnName("employee_date_of_birth");
            entity.Property(e => e.EmployeeEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("employee_email");
            entity.Property(e => e.EmployeeFirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("employee_first_name");
            entity.Property(e => e.EmployeeGender)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("employee_gender");
            entity.Property(e => e.EmployeeLastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("employee_last_name");
            entity.Property(e => e.EmployeePhone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("employee_phone");
            entity.Property(e => e.EmployeePhoto)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("employee_photo");
            entity.Property(e => e.EmployeePincode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("employee_pincode");
            entity.Property(e => e.StateId).HasColumnName("state_id");

            entity.HasOne(d => d.City).WithMany(p => p.TblEmployees)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("fk_question_cities_103");

            entity.HasOne(d => d.Country).WithMany(p => p.TblEmployees)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("fk_question_countries_101");

            entity.HasOne(d => d.Department).WithMany(p => p.TblEmployees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("fk_question_department_103");

            entity.HasOne(d => d.State).WithMany(p => p.TblEmployees)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("fk_question_states_102");
        });

        modelBuilder.Entity<TblFavouriteHobby>(entity =>
        {
            entity.HasKey(e => e.FavouriteHobbiesId);

            entity.ToTable("tbl_Favourite_Hobbies");

            entity.Property(e => e.FavouriteHobbiesId).HasColumnName("favourite_hobbies_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.HobbyId).HasColumnName("hobby_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.TblFavouriteHobbies)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_tbl_Favourite_Hobbies_tbl_Employees");

            entity.HasOne(d => d.Hobby).WithMany(p => p.TblFavouriteHobbies)
                .HasForeignKey(d => d.HobbyId)
                .HasConstraintName("FK_tbl_Favourite_Hobbies_tbl_Hobbies");
        });

        modelBuilder.Entity<TblHobby>(entity =>
        {
            entity.HasKey(e => e.HobbyId).HasName("PK__tbl_Hobb__ABCB3D348678A134");

            entity.ToTable("tbl_Hobbies");

            entity.Property(e => e.HobbyId).HasColumnName("hobby_id");
            entity.Property(e => e.HobbyText)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hobby_Text");
        });

        modelBuilder.Entity<TblState>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__tbl_Stat__81A47417CFE3755A");

            entity.ToTable("tbl_States");

            entity.Property(e => e.StateId)
                .ValueGeneratedNever()
                .HasColumnName("state_id");
            entity.Property(e => e.CountryId)
                .HasDefaultValueSql("('1')")
                .HasColumnName("country_id");
            entity.Property(e => e.StateName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("state_name");

            entity.HasOne(d => d.Country).WithMany(p => p.TblStates)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_question_countries");
        });

        modelBuilder.Entity<UserMaster>(entity =>
        {
            entity.ToTable("User_Master");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
