using AutoMapper;
using Server.Dtos;
using Server.Models;

namespace Server.Mapper
{
	public class MappingProfile:Profile
	{
        public MappingProfile()
        {
            CreateMap<PostDto, Post>().ReverseMap();
        }
    }
}

