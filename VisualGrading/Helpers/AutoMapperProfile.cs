using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace VisualGrading.Helpers
{
    public class AutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<VisualGrading.Model.Data.Student, VisualGrading.Students.Student>();
            CreateMap<VisualGrading.Students.Student, VisualGrading.Model.Data.Student>();
        }
    }
}
