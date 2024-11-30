using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace SuperLaba2._0
{
    public class JsonFileHandler
    {
        public string JsonData { get; private set; } = string.Empty;
        private string _filePath = string.Empty; 

        public async Task<bool> OpenJsonFileAsync()
        {
            try
            {
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "application/json" } },
                { DevicePlatform.iOS, new[] { "public.json" } },
                { DevicePlatform.WinUI, new[] { ".json" } }
            });

                var fileResult = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Оберіть JSON файл",
                    FileTypes = customFileType
                });

                if (fileResult != null)
                {
                    _filePath = fileResult.FullPath; 
                    JsonData = await File.ReadAllTextAsync(_filePath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            return false;
        }

        public async Task SaveJsonDataAsync(List<Material> materials)
        {
            try
            {
                if (string.IsNullOrEmpty(_filePath))
                    throw new InvalidOperationException("Файл для збереження не вибраний.");

                string jsonString = JsonSerializer.Serialize(materials, new JsonSerializerOptions { WriteIndented = true });

                
                await File.WriteAllTextAsync(_filePath, jsonString);

                Console.WriteLine($"Збережено в: {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
                throw;
            }
        }

        public List<Material> ParseJsonData()
        {
            try
            {
                if (string.IsNullOrEmpty(JsonData))
                    throw new InvalidOperationException("JSON не завантажено.");

                return JsonSerializer.Deserialize<List<Material>>(JsonData) ?? new List<Material>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час парсингу: {ex.Message}");
                return new List<Material>();
            }
        }
    }
}
