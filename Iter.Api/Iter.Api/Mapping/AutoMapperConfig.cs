using AutoMapper;

namespace Iter.Api.Mapping
{
    public class AutoMapperConfig
    {
        public static IMapper CreateMapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<IterAutoMapperProfile>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }
}
