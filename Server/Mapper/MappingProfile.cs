using AutoMapper;
using Server.Dtos;
using Server.Models;

namespace Server.Mapper
{
    /// <summary>
    /// Mapping based on PostDto and Post
    /// </summary>
	public class MappingProfile:Profile
	{
        public MappingProfile()
        {
            CreateMap<PostDto, Post>().ReverseMap();
        }
    }
}

