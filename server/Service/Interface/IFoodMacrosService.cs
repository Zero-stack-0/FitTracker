using Service.Helpers;

namespace Service.Interface
{
    public interface IFoodMacrosService
    {
        Task<ApiResponse> GetFoodMacrosByName(string name);
    }
}