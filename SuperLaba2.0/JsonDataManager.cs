using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace SuperLaba2._0;
public class JsonDataManager
{
    private readonly JsonFileHandler _fileHandler;
    private List<Material> _materials;

    public JsonDataManager(JsonFileHandler fileHandler)
    {
        _fileHandler = fileHandler;
        _materials = new List<Material>();
    }
    // наступні 3 методі - пошуки
    public List<Material> SearchByAuthor(string author)
    {
        return _materials.Where(m => m.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    
    public List<Material> SearchByType(string type)
    {
        return _materials.Where(m => m.Type.Contains(type, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    
    public List<Material> SearchByCreationDate(string date)
    {
        return _materials.Where(m => m.CreationDate.Contains(date)).ToList();
    }
    
    public async Task LoadDataAsync()//підгрузка з файлу
    {
        _materials = _fileHandler.ParseJsonData();
    }

    
    public List<Material> GetMaterials()//всі матеріали
    {
        return _materials;
    }

    
    public Material AddMaterial(Material material)//додавання запису
    {
        _materials.Add(material);
        return material;
    }

    
    public bool EditMaterial(string author, Material updatedMaterial)// редагування
    {
        var material = _materials.FirstOrDefault(m => m.Author == author);
        if (material != null)
        {
            int index = _materials.IndexOf(material);
            _materials[index] = updatedMaterial;
            return true;
        }
        return false;
    }

    public bool DeleteMaterial(string author)//видалення
    {
        var material = _materials.FirstOrDefault(m => m.Author == author);
        if (material != null)
        {
            _materials.Remove(material);
            return true;
        }
        return false;
    }

    
    public async Task SaveDataAsync()// збереження
    {
        await _fileHandler.SaveJsonDataAsync(_materials);
    }
}


