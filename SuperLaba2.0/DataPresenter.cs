using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLaba2._0
{
    public class DataPresenter
    {
        public string FormatMaterials(List<Material> materials)
        {
            if (materials == null || materials.Count == 0)
                return "Немає даних для відображення.";

            return string.Join("\n\n", materials.Select(m =>
                $"Автор: {m.Author}\n" +
                $"Назва: {m.Title}\n" +
                $"Факультет: {m.Faculty}\n" +
                $"Кафедра: {m.Department}\n" +
                $"Тип матеріалу: {m.Type}\n" +
                
                $"Дата створення: {m.CreationDate}"));
        }
    }
}
