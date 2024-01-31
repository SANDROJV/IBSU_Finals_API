using AutoMapper;
using Business.DTOs;
using Database.Entities;

namespace Business.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.BookAuthors.Select(ba => ba.Author).ToList()));
        CreateMap<BookAuthor, BookAuthorDTO>();
        CreateMap<Author, AuthorDTO>();

        CreateMap<EditBookDTO, Book>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => GetImageByteArray(src.ImageFile)))
            .ForMember(dest => dest.BookAuthors, opt => opt.Ignore()); 
        CreateMap<IFormFile, byte[]>().ConvertUsing(src => GetImageByteArray(src));
    }
    
    private byte[] GetImageByteArray(IFormFile imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                imageFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        return null;
    }
}