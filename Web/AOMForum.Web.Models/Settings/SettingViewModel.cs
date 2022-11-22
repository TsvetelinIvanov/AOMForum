using AOMForum.Data.Models;
using AOMForum.Services.Mapping;
using AutoMapper;

namespace AOMForum.Web.Models.Settings
{
    public class SettingViewModel : IMapFrom<Setting>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Value { get; set; }

        public string? NameAndValue { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Setting, SettingViewModel>().ForMember(
                m => m.NameAndValue,
                opt => opt.MapFrom(s => s.Name + " = " + s.Content));
        }
    }
}