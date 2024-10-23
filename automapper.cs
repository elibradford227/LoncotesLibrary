using AutoMapper;
namespace LoncotesLibrary.models;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {

        CreateMap<Material, MaterialDTO>();
        CreateMap<MaterialDTO, Material>();
        CreateMap<MaterialType, MaterialTypeDTO>();
        CreateMap<MaterialTypeDTO, MaterialType>();
        CreateMap<Genre, GenreDTO>();
        CreateMap<GenreDTO, Genre>();
        CreateMap<Patron, PatronDTO>();
        CreateMap<PatronDTO, Patron>();
        CreateMap<Checkout, CheckoutDTO>();
        CreateMap<CheckoutDTO, Checkout>();
        CreateMap<Checkout, CheckoutWithLateFeeDTO>();
        CreateMap<CheckoutWithLateFeeDTO, Checkout>();
    }
}