
using System.Text.Json;
namespace SuperLaba2._0
{

    public partial class MainPage : ContentPage
    {
        private readonly JsonFileHandler _fileHandler;
        private readonly JsonDataManager _dataManager;
        private readonly DataPresenter _dataPresenter;

        public MainPage()
        {

            InitializeComponent();
            _fileHandler = new JsonFileHandler();
            _dataManager = new JsonDataManager(_fileHandler);
            _dataPresenter = new DataPresenter();
        }
        private void DisplayDataInGrid(List<Material> materials)
        {
            DataGrid.Children.Clear();
            DataGrid.RowDefinitions.Clear(); 
            DataGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            AddGridCell(new Label { Text = "Автор", FontAttributes = FontAttributes.Bold }, 0, 0, true);
            AddGridCell(new Label { Text = "Названа предмету", FontAttributes = FontAttributes.Bold }, 0, 1, true);
            AddGridCell(new Label { Text = "Факультет", FontAttributes = FontAttributes.Bold }, 0, 2, true);
            AddGridCell(new Label { Text = "Кафедра", FontAttributes = FontAttributes.Bold }, 0, 3, true);
            AddGridCell(new Label { Text = "Тип", FontAttributes = FontAttributes.Bold }, 0, 4, true);
            AddGridCell(new Label { Text = "Дата", FontAttributes = FontAttributes.Bold }, 0, 5, true);

            int row = 1;
            foreach (var material in materials)
            {
             
                DataGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                AddGridCell(new Label { Text = material.Author }, row, 0);
                AddGridCell(new Label { Text = material.Title }, row, 1);
                AddGridCell(new Label { Text = material.Faculty }, row, 2);
                AddGridCell(new Label { Text = material.Department }, row, 3);
                AddGridCell(new Label { Text = material.Type }, row, 4);
                AddGridCell(new Label { Text = material.CreationDate }, row, 5);

                row++; 
            }
        }//робота з грідом

        private void OnShowDataClicked(object sender, EventArgs e)
        {
            var materials = _dataManager.GetMaterials();
            DisplayDataInGrid(materials);
        }//відображення

        private void AddGridCell(View view, int row, int column, bool isHeader = false)
        {
            if (view is Label label)
            {
                label.FontSize = isHeader ? 14 : 12; 
                label.HorizontalTextAlignment = TextAlignment.Center; 
                label.VerticalTextAlignment = TextAlignment.Center; 
                label.LineBreakMode = LineBreakMode.WordWrap; 
                label.Padding = new Thickness(5); 
            }

            if (isHeader)
            {
                view.BackgroundColor = Colors.LightGray; 
            }

            Grid.SetRow(view, row);
            Grid.SetColumn(view, column);
            DataGrid.Children.Add(view);
        }// add cell
        //////////////////////////////////// обробка пошуку 
        private async void OnSearchByAuthorClicked(object sender, EventArgs e)
        {
            string author = await DisplayPromptAsync("Пошук", "Введіть ім'я автора:");
            if (!string.IsNullOrWhiteSpace(author))
            {
                var results = _dataManager.SearchByAuthor(author);
                DisplayDataInGrid(results);
            }
        }
        private async void OnSearchByTypeClicked(object sender, EventArgs e)
        {
            string type = await DisplayPromptAsync("Пошук", "Введите тип предмету:");
            if (!string.IsNullOrWhiteSpace(type))
            {
                var results = _dataManager.SearchByType(type);
                DisplayDataInGrid(results);
            }
        }
        private async void OnSearchByDateClicked(object sender, EventArgs e)
        {
            string date = await DisplayPromptAsync("Пошук", "Введіть дату (рррр-мм-дд):");
            if (!string.IsNullOrWhiteSpace(date))
            {
                var results = _dataManager.SearchByCreationDate(date);
                DisplayDataInGrid(results);
            }
        }
        ///////////////////////////////////// обробка пошуку 
        private async void OnOpenFileClicked(object sender, EventArgs e)
        {
            bool success = await _fileHandler.OpenJsonFileAsync();
            if (success)
            {
                await _dataManager.LoadDataAsync();
                await DisplayAlert("Успіх", "Файл завантажено", "OK");
            }
            else
                await DisplayAlert("Помилка", "Не вдалося завантажити файл", "OK");
        }//обробка відкриття файлу
        private async void OnAddDataClicked(object sender, EventArgs e)
        {
            var newMaterial = await EditMaterialAsync(new Material());
            if (newMaterial != null)
            {
                _dataManager.AddMaterial(newMaterial);
                await DisplayAlert("Успіх", "Запис додано", "OK");
            }
        }//обробка додавання матеріалу
        private async void OnEditDataClicked(object sender, EventArgs e)
        {
            string author = await DisplayPromptAsync("Редагувати запис", "Ввести ім'я автора:");
            var existingMaterial = _dataManager.GetMaterials().FirstOrDefault(m => m.Author == author);

            if (existingMaterial != null)
            {
                var updatedMaterial = await EditMaterialAsync(existingMaterial);
                if (updatedMaterial != null && _dataManager.EditMaterial(author, updatedMaterial))
                {
                    await DisplayAlert("Успіх", "Запис оновлено", "OK");
                }
            }
            else
            {
                await DisplayAlert("Помилка", "Запис не знайдено", "OK");
            }
        }//обробка редагування вже існуючого матеріалу
        private async void OnDeleteDataClicked(object sender, EventArgs e)
        {
            string author = await DisplayPromptAsync("Видалити запис", "Ввести ім'я автора:");
            if (_dataManager.DeleteMaterial(author))
            {
                await DisplayAlert("Успіх", "Запис видалено", "OK");
            }
            else
            {
                await DisplayAlert("Помилка", "Запис не знайдено", "OK");
            }
        }//обробка видалення
        private async void OnSaveDataClicked(object sender, EventArgs e)
        {
            try
            {
                await _dataManager.SaveDataAsync();
                await DisplayAlert("Успіх", "Збережено", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Трапилась помилка: {ex.Message}", "OK");
            }
        }//обробка збереження у файл
        private async Task<Material?> EditMaterialAsync(Material material)
        {
            material.Author = await DisplayPromptAsync("Автор", "Додайте ФІО автора:", initialValue: material.Author);
            material.Title = await DisplayPromptAsync("Название", "Додайте назву предмету:", initialValue: material.Title);
            material.Faculty = await DisplayPromptAsync("Факультет", "Додайте Факультет:", initialValue: material.Faculty);
            material.Department = await DisplayPromptAsync("Кафедра", "Додайте кафедру:", initialValue: material.Department);
            material.Type = await DisplayPromptAsync("Тип матеріалу", "Додайте тип матеріалу:", initialValue: material.Type);
           
            material.CreationDate = await DisplayPromptAsync("Дата", "Додайте дату:", initialValue: material.CreationDate);

            return material;
        }//Форма для редагування материалу
        private async void OnLobbyClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Про програму", "Студент: Герасименко Богдан Сергійович \nКурс: 2 \nГрупа: К-25 \nРік: 2024\nКоротко про програму: Варіант 4, реалізація роботи з JSON файлами", "OK");
        }//Про програму
    }
}
