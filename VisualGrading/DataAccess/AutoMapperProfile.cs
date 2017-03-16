#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// AutoMapperProfile.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  +===========================================================================+

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Net.Mail;
using AutoMapper;
using VisualGrading.Grades;
using VisualGrading.Model.Data;
using VisualGrading.Settings;
using VisualGrading.Students;
using VisualGrading.Tests;

#endregion

namespace VisualGrading.DataAccess
{
    public class AutoMapperProfile : Profile
    {
        #region Fields

        public Dictionary<Type, Type> MappingDictionary;

        #endregion

        #region Constructors

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

        #endregion

        #region Private Methods

        protected override void Configure()
        {
            CreateMap<string, MailAddress>().ConvertUsing(new StringToMailAddressTypeConverter());
            CreateMap<MailAddress, string>().ConvertUsing(new MailAddressToStringTypeConverter());

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

        #endregion
    }

    public class MailAddressToStringTypeConverter : ITypeConverter<MailAddress, string>
    {
        #region Public Methods

        public string Convert(MailAddress source, string destination, ResolutionContext context)
        {
            return source.Address;
        }

        #endregion
    }

    public class StringToMailAddressTypeConverter : ITypeConverter<string, MailAddress>
    {
        #region Public Methods

        public MailAddress Convert(string source, MailAddress destination, ResolutionContext context)
        {
            return new MailAddress(source);
        }

        #endregion
    }

    public class StringToDateTimeTypeConverter : ITypeConverter<int, DateTime>
    {
        #region Public Methods

        public DateTime Convert(int source, DateTime destination, ResolutionContext context)
        {
            return System.Convert.ToDateTime(source);
        }

        #endregion
    }

    public class DateTimeToStringTypeConverter : ITypeConverter<DateTime, int>
    {
        #region Public Methods

        public int Convert(DateTime source, int destination, ResolutionContext context)
        {
            return System.Convert.ToInt32(source);
        }

        #endregion
    }
}