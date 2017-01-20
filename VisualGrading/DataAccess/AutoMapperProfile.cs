using System;
using System.Collections.Generic;
using AutoMapper;
using VisualGrading.Grades;
using VisualGrading.Model.Data;
using VisualGrading.Settings;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.DataAccess
{
    public class AutoMapperProfile : Profile
    {
        public Dictionary<Type, Type> MappingDictionary;

        public AutoMapperProfile()
        {
            MappingDictionary = new Dictionary<Type, Type>();
            MappingDictionary.Add(typeof(StudentDTO), typeof(Student));
            MappingDictionary.Add(typeof(Student), typeof(StudentDTO));
            MappingDictionary.Add(typeof(TestDTO), typeof(Test));
            MappingDictionary.Add(typeof(Test), typeof(TestDTO));
            MappingDictionary.Add(typeof(GradeDTO), typeof(Grade));
            MappingDictionary.Add(typeof(Grade), typeof(GradeDTO));
            MappingDictionary.Add(typeof(SettingsProfile), typeof(SettingsProfileDTO));
            MappingDictionary.Add(typeof(SettingsProfileDTO), typeof(SettingsProfile));

            CreateMapsForAutoMapper();
        }

        protected override void Configure()
        {
            //CreateMap<string, DateTime>().ConvertUsing(new IntToDateTimeTypeConverter());
            //CreateMap<DateTime, string>().ConvertUsing(new DateTimeToIntTypeConverter());
            //CreateMap<StudentDTO, Student>();
            //CreateMap<Student, StudentDTO>();
            //CreateMap<TestDTO, Test>();
            //CreateMap<Test, TestDTO>();
            //CreateMap<GradeDTO, Grade>();
            //CreateMap<Grade, GradeDTO>();
        }

        private void CreateMapsForAutoMapper()
        {
            foreach (var kvp in MappingDictionary)
                CreateMap(kvp.Key, kvp.Value);
        }
    }

    public class StringToDateTimeTypeConverter : ITypeConverter<int, DateTime>
    {
        public DateTime Convert(int source, DateTime destination, ResolutionContext context)
        {
            return System.Convert.ToDateTime(source);
        }
    }

    public class DateTimeToStringTypeConverter : ITypeConverter<DateTime, int>
    {
        public int Convert(DateTime source, int destination, ResolutionContext context)
        {
            return System.Convert.ToInt32(source);
        }
    }
}