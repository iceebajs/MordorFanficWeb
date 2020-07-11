using AutoMapper;

namespace MordorFanficWeb.Common.AutoMapper
{
    public class AutoMapper
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });
            return config.CreateMapper();
        }
    }
}
