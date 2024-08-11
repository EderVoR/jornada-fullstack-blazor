using Fina.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;

namespace Fina.App.Pages.Categories
{
    public partial class CreateCategoryPage : ComponentBase
    {
        #region Properties

        public bool IsBusy { get; set; } = false;
        public CreateCategoryRequest InputModel { get; set; } = new();

        #endregion
    }
}
