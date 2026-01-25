using UD5_Usuarios_SQLite.ViewModels;

namespace UD5_Usuarios_SQLite.Views
{
    /// <summary>
    /// Página principal de la aplicación.
    /// El code-behind queda mínimo ya que la lógica está en el ViewModel.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
